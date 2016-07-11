using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace bigeyedev.Controllers
{
    public class AdminController : Controller
    {
        sugareyeEntities _db = new sugareyeEntities();

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Admin");
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string returnUrl)
        {
            if (Session["adminLogin"] != null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.returnUrl = returnUrl;
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string user,string pwd , string returnUrl)
        {
            if (user == null || pwd == null)
            {
                RedirectToAction("Login", new { returnUrl = returnUrl});
            }

            var userModel = _db.user.Where(m => m.username == user && m.password == pwd).FirstOrDefault();

            if (userModel != null)
            {
                Session["adminLogin"] = userModel;
            }
            else
            {
                return RedirectToAction("Login", new { returnUrl = returnUrl});
            }

            return RedirectToLocal(returnUrl);
        }

        public async Task<ActionResult> ManagerOrder()
        {
            if (Session["adminLogin"] == null)
            {
                return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
            }
            var modelOrder = await _db.bigeyedev_order.ToListAsync();
            var listName = new List<string>();
            foreach (var item in modelOrder)
            {
                listName.Add(_db.bigeyedev_member.Where(m => m.member_id == item.member_id).Select(u => u.name).Single());
            }

            return View(new Tuple<List<bigeyedev_order>,List<string>>(modelOrder,listName));
        }








        public ActionResult _orderViewAddress(int addressID)
        {
            var address = _db.bigeyedev_member_address.Find(addressID);

            return PartialView(address);
        }




        
        public ActionResult _orderViewItem(int orderID)
        {
            var orderItem = _db.bigeyedev_order_fashion_item.Where(m => m.order_id == orderID).ToList();
            var productItem = new List<string>();
            foreach (var item in orderItem)
            {
                try
                {
                    productItem.Add(_db.product.Where(m => m.id == item.product_id).Select(u => u.image_url).Single());
                }
                catch { }

            }

            return PartialView(new Tuple<List<bigeyedev_order_fashion_item>, List<string>>(orderItem, productItem));
        }




        public ActionResult OrderDetial(int? id)
        {
            if (Session["adminLogin"] == null)
            {
                return RedirectToAction("Login", new { returnUrl = Request.Url.AbsolutePath });
            }

            if (id == null)
            {
                return RedirectToAction("ManagerOrder");
            }

            //ค้นหาคำสั่งซื้อ
            int orderID = id.Value;
            var order = new bigeyedev_order();
            try
            {
                order = _db.bigeyedev_order.Where(m => m.order_id == orderID).Single();
            }
            catch
            {
                return RedirectToAction("ManagerOrder");
            }
            

            return View(order);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayConfirm(int orderID)
        {
            if (Session["adminLogin"] == null)
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/Order/"+orderID });
            }

            var order = _db.bigeyedev_order.Find(orderID);
            order.pay_finish = 1;
            _db.SaveChanges();

            return RedirectToAction("OrderDetial", new { id = orderID });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateStatus(int orderID,int status)
        {
            if (Session["adminLogin"] == null)
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/Order/" + orderID });
            }

            var order = _db.bigeyedev_order.Find(orderID);
            order.status = status;
            _db.SaveChanges();

            return RedirectToAction("OrderDetial", new { id = orderID });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult orderAmount(int orderID,int? amount)
        {
            if (amount == null)
            {
                return RedirectToAction("OrderDetial", new { id = orderID });
            }
            if (Session["adminLogin"] == null)
            {
                return RedirectToAction("Login", new { returnUrl = "/Admin/Order/" + orderID });
            }

            var order = _db.bigeyedev_order.Find(orderID);
            order.order_amount = amount.Value;
            _db.SaveChanges();

            return RedirectToAction("OrderDetial", new { id = orderID });
        }
    }
}