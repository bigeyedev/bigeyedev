using bigeyedev.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

//เปลี่ยนภาษา
using System.Globalization;
//using System.Threading;
using System.Resources;


namespace bigeyedev.Controllers
{
    public class HomeController : Controller
    {

        private sugareyeEntities _db = new sugareyeEntities();



        public ActionResult Index()
        {
            return RedirectToAction("Fashion_Select");
        }

        


        public ActionResult Fashion_Select_Dialog()
        {
            //ViewBag.Message = "Your contact page.";

            return View("Fashion_Select_Dialog");
        }



        public async Task<ActionResult> Fashion_Select(int? id)
        {
            // ในส่วน controller ต้องมีตรงนี้ด้วย แยกกับ html
            if (Session["Culture"] != null)
            {
                string Culture = Session["Culture"] as string;
                //ใช้อันไหนก็ได้ระหว่าง Thread กับ Resources
                //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                Resources.Resource.Culture = new System.Globalization.CultureInfo(Culture);
            }
            else
            {
                Resources.Resource.Culture = new System.Globalization.CultureInfo("");
            }

            ViewBag.Title = " " + Resources.Resource.menu_fashion + " -> " + Resources.Resource.menu_fashion_select + " ";
            ViewBag.Des = Resources.Resource.head_des;



           var  model = await _db.product.Where(m => m.visible == 1 && (m.black > 0 || m.choco > 0 || m.gray > 0 || m.brown > 0 || m.blue > 0 || m.green > 0 || m.violet > 0 || m.pink > 0 || m.silver > 0 || m.gold > 0 || m.sky > 0 || m.red > 0)).Select(u => new stockBindingModel
            {
                id = u.id,
                model = u.model,
                brand = u.brand,
                imageUrl = u.image_url,
                Black = u.black.Value,
                Choco = u.choco.Value,
                Gray = u.gray.Value,
                Brown = u.brown.Value,
                Blue = u.blue.Value,
                Green = u.green.Value,
                Violet = u.violet.Value,
                Pink = u.pink.Value,
                Silver = u.silver.Value,
                Sky = u.sky.Value,
                Gold = u.gold.Value,
                Red = u.red.Value,
                near = u.near
            }).ToListAsync();

            var brand = await _db.bigeyedev_brand.ToListAsync();

            if (id == 1)
            {
                var modelItem = new List<stockBindingModel>();
                brand = brand.Where(m => m.brand_group == 1).ToList();

                foreach(var itemBrand in brand)
                {
                    modelItem.AddRange(modelItem.Where(m => m.brand == itemBrand.brand).ToList());
                }
            }

            
            return View(new Tuple<List<stockBindingModel>, List<bigeyedev_brand>>(model, brand));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Fashion_Select(List<stockBindingModel> model)
        {
            if (model == null)
            {
                return RedirectToAction("Fashion_Select");
            }

            //select order item
            var itemOrder = new List<stockBindingModel>();
            foreach (var item in model)
            {
                if (item.Black > 0 || item.Blue > 0 || item.Brown > 0 || item.Choco > 0 || item.Gold > 0 || item.Gray > 0 || item.Green > 0 || item.Pink > 0 || item.Red > 0 || item.Silver > 0 || item.Sky > 0 || item.Violet > 0)
                {
                    itemOrder.Add(item);
                }
            }
            //add to cookie
            foreach (var item in itemOrder)
            {
                // if cookie order:id is exist
                if (Request.Cookies["order:" + item.id] != null)
                {
                    item.Black = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Black"]) + item.Black;
                    item.Blue = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Blue"]) + item.Blue;
                    item.Brown = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Brown"]) + item.Brown;
                    item.Choco = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Choco"]) + item.Choco;
                    item.Gold = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Gold"]) + item.Gold;
                    item.Gray = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Gray"]) + item.Gray;
                    item.Green = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Green"]) + item.Green;
                    item.Pink = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Pink"]) + item.Pink;
                    item.Red = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Red"]) + item.Red;
                    item.Silver = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Silver"]) + item.Silver;
                    item.Sky = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Sky"]) + item.Sky;
                    item.Violet = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Violet"]) + item.Violet;
                }

                //addookie
                addCookieStock(item, 0);
            }

            return RedirectToAction("Cart");
        }


        public async Task<ActionResult> Cart()
        {
            var model = bindCookieStock();
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            var stock = new List<stockBindingModel>();
            for (int i = model.Count - 1; i >= 0; i--)
            {
                int id = model[i].id;
                var item = await _db.product.Where(m => m.id == id).Select(u => new stockBindingModel
                {
                    id = u.id,
                    model = u.model,
                    brand = u.brand,
                    imageUrl = u.image_url,
                    Black = u.black.Value,
                    Choco = u.choco.Value,
                    Gray = u.gray.Value,
                    Brown = u.brown.Value,
                    Blue = u.blue.Value,
                    Green = u.green.Value,
                    Violet = u.violet.Value,
                    Pink = u.pink.Value,
                    Silver = u.silver.Value,
                    Sky = u.sky.Value,
                    Gold = u.gold.Value,
                    Red = u.red.Value,
                    near = u.near
                }).SingleAsync();

                if (item.Black > 0 || item.Blue > 0 || item.Brown > 0 || item.Choco > 0 || item.Gold > 0 || item.Gray > 0 || item.Green > 0 || item.Pink > 0 || item.Red > 0 || item.Silver > 0 || item.Sky > 0 || item.Violet > 0)
                {
                    stock.Add(item);
                }
                else
                {
                    model.RemoveAt(i);
                }
            }
            var multiModel = new Tuple<List<stockBindingModel>, List<stockBindingModel>>(stock, model);
            return View(multiModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cart(List<stockBindingModel> model)
        {
            if (model == null)
            {
                return RedirectToAction("Cart");
            }
            //delete all order cookie
            for (int i = Request.Cookies.Count - 1; i >= 0; i--)
            {
                if (Request.Cookies[i].Name.Substring(0, 5) == "order")
                {
                    var cookie = new HttpCookie(Request.Cookies[i].Name);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }
            //delete order 0 and add new cookie
            foreach (var item in model)
            {
                if (item.Black > 0 || item.Blue > 0 || item.Brown > 0 || item.Choco > 0 || item.Gold > 0 || item.Gray > 0 || item.Green > 0 || item.Pink > 0 || item.Red > 0 || item.Silver > 0 || item.Sky > 0 || item.Violet > 0)
                {
                    addCookieStock(item, 1);
                }

            }

            return RedirectToAction("Address");
        }






        private void addCookieStock(stockBindingModel item, int state)
        {
            var Cookie = new HttpCookie("order:" + item.id);
            Cookie.Values["id"] = item.id.ToString();
            Cookie.Values["model"] = item.model;
            Cookie.Values["brand"] = item.brand;
            Cookie.Values["imageUrl"] = item.imageUrl;
            Cookie.Values["near"] = item.near;
            Cookie.Values["Black"] = item.Black.ToString();
            Cookie.Values["Blue"] = item.Blue.ToString();
            Cookie.Values["Brown"] = item.Brown.ToString();
            Cookie.Values["Choco"] = item.Choco.ToString();
            Cookie.Values["Gold"] = item.Gold.ToString();
            Cookie.Values["Gray"] = item.Gray.ToString();
            Cookie.Values["Green"] = item.Green.ToString();
            Cookie.Values["Pink"] = item.Pink.ToString();
            Cookie.Values["Red"] = item.Red.ToString();
            Cookie.Values["Silver"] = item.Silver.ToString();
            Cookie.Values["Sky"] = item.Sky.ToString();
            Cookie.Values["Violet"] = item.Violet.ToString();
            Cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(Cookie);
            Response.Cookies["confirm"].Value = state.ToString();
        }

        public ActionResult Address()
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login");
            }
            if (Request.Cookies["confirm"] == null)
            {
                return RedirectToAction("Index");
            }
            if (Request.Cookies["confirm"].Value != "1")
            {
                return RedirectToAction("Cart");
            }

            //get address form member
            int id = Convert.ToInt32(Response.Cookies["Account"].Values["id"]);
            var address = _db.bigeyedev_member_address.Where(m => m.member_id == id).ToList();
            var contract = new Contract();
            contract.addressList = address;

            return View(contract);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(Contract model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model == null)
            {
                return RedirectToAction("Address");
            }
            var Cookie = new HttpCookie("contract");
            Cookie.Values["name"] = model.address.name;
            Cookie.Values["mobile"] = model.address.mobile_shop;
            Cookie.Values["mobile2"] = model.address.mobile_shop;
            Cookie.Values["lineid"] = model.memberData.line_id;
            Cookie.Values["email"] = model.memberData.email;
            Cookie.Values["shopname"] = model.address.shop_name;
            Cookie.Values["address"] = model.address.address;
            Cookie.Values["distict"] = model.address.sub_district;
            Cookie.Values["area"] = model.address.district;
            Cookie.Values["province"] = model.address.province;
            Cookie.Values["zipcode"] = model.address.zip;
            Cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(Cookie);

            //test
            model.address.member_id = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            _db.bigeyedev_member_address.Add(model.address);
            _db.SaveChanges();
            return RedirectToAction("Review");
        }


        public ActionResult Review()
        {
            var contract = bindCookieContract();
            if (contract == null)
            {
                return RedirectToAction("Address");
            }

            var itemStock = bindCookieStock();
            if (itemStock == null)
            {
                return RedirectToAction("Indext");
            }

            return View(new Tuple<Contract, List<stockBindingModel>>(contract, itemStock));
        }



        private Contract bindCookieContract()
        {
            if (Request.Cookies["contract"] == null)
            {
                return null;
            }
            var Cookie = Request.Cookies["contract"];
            var contract = new Contract();

            contract.address.name = Cookie.Values["name"];
            contract.address.mobile_shop = Cookie.Values["mobile"];
            contract.address.mobile2_shop = Cookie.Values["mobile2"];
            contract.memberData.line_id = Cookie.Values["lineid"];
            contract.memberData.email = Cookie.Values["email"];
            contract.address.shop_name = Cookie.Values["shopname"];
            contract.address.address = Cookie.Values["address"];
            contract.address.sub_district = Cookie.Values["distict"];
            contract.address.district = Cookie.Values["area"];
            contract.address.province = Cookie.Values["province"];
            contract.address.zip = Cookie.Values["zipcode"];
            return contract;
        }





        private List<stockBindingModel> bindCookieStock()
        {
            bool found = chkCookie();
            if (!found)
            {
                return null;
            }
            var model = new List<stockBindingModel>();
            for (int i = 0; i < Request.Cookies.Count; i++)
            {
                if (Request.Cookies[i].Name.Substring(0, 5) == "order")
                {
                    model.Add(new stockBindingModel()
                    {
                        id = Convert.ToInt32(Request.Cookies[i].Values["id"]),
                        brand = Request.Cookies[i].Values["brand"],
                        model = Request.Cookies[i].Values["model"],
                        imageUrl = Request.Cookies[i].Values["imageUrl"],
                        near = Request.Cookies[i].Values["near"],
                        Black = Convert.ToInt32(Request.Cookies[i].Values["Black"]),
                        Blue = Convert.ToInt32(Request.Cookies[i].Values["Blue"]),
                        Brown = Convert.ToInt32(Request.Cookies[i].Values["Brown"]),
                        Choco = Convert.ToInt32(Request.Cookies[i].Values["Choco"]),
                        Gold = Convert.ToInt32(Request.Cookies[i].Values["Gold"]),
                        Gray = Convert.ToInt32(Request.Cookies[i].Values["Gray"]),
                        Green = Convert.ToInt32(Request.Cookies[i].Values["Green"]),
                        Pink = Convert.ToInt32(Request.Cookies[i].Values["Pink"]),
                        Red = Convert.ToInt32(Request.Cookies[i].Values["Red"]),
                        Silver = Convert.ToInt32(Request.Cookies[i].Values["Silver"]),
                        Sky = Convert.ToInt32(Request.Cookies[i].Values["Sky"]),
                        Violet = Convert.ToInt32(Request.Cookies[i].Values["Violet"])
                    });
                }
            }
            return (model);
        }


        private bool chkCookie()
        {
            bool found = false;
            for (int i = 0; i < Request.Cookies.Count; i++)
            {
                if (Request.Cookies[i].Name.Substring(0, 5) == "order")
                {
                    found = true;
                    break;
                }
            }
            return found;
        }



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Result()
        //{
        //    bool found = chkCookie();

        //    if (!found)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    if (Request.Cookies["confirm"].Value != "1")
        //    {
        //        return RedirectToAction("Cart");
        //    }
        //    if (Request.Cookies["contract"] == null)
        //    {
        //        return RedirectToAction("Address");
        //    }


        //    var contract = bindCookieContract();
        //    _db.orders.Add(contract);
        //    if (_db.SaveChanges() <= 0)
        //    {
        //        return View(0);
        //    }
        //    int orderId = contract.id;


        //    var stock = bindCookieStock();
        //    var product = new List<bigeyedev_order_fashion_item>();
        //    foreach (var item in stock)
        //    {
        //        product.Add(new bigeyedev_order_fashion_item()
        //        {
        //            order_id = orderId,
        //            product_id = item.id,
        //            brand = item.brand,
        //            model = item.model,
        //            near = item.near,
        //            black = item.Black,
        //            blue = item.Blue,
        //            brown = item.Brown,
        //            choco = item.Choco,
        //            gold = item.Gold,
        //            gray = item.Gray,
        //            green = item.Green,
        //            pink = item.Pink,
        //            red = item.Red,
        //            silver = item.Silver,
        //            sky = item.Sky,
        //            violet = item.Violet
        //        });
        //    }
        //    _db.bigeyedev_order_fashion_item.AddRange(product);
        //    if (_db.SaveChanges() <= 0)
        //    {
        //        _db.bigeyedev_order.Remove(contract);
        //        _db.SaveChanges();
        //        return View(0);
        //    }

        //    return View(orderId);
        //}






        public ActionResult Login(int? status)
        {
            var d = Request.Url.AbsoluteUri;
            //ตรวจสอบว่ามีการล็อคอินค้างอยู่หรือป่าว
            if (Request.Cookies["Account"] != null)
            {
                return RedirectToAction("Index");
            }

            //กำหนด ข้อความ อาไว้แสดงเมื่อการกระทำ
            string msg = "";
            if (status == 1)
            {
                return RedirectToAction("Index");
            }
            else if (status == 0)
            {
                msg = "Login Fail!Mobile or Year is wrong.";
            }
            else if (status == 2)
            {
                msg = "Register Fail!Already Mobile.";
            }
            else if (status == 3)
            {
                msg = "Register Success";
            }

            return View((object)msg);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string txtUser, string txtPwd, string url)
        {
            //check varible is null
            if (txtUser == null || txtPwd == null)
            {
                return RedirectToAction("Login");
            }

            //select data member from database
            var member = _db.bigeyedev_member.Where(m => m.mobile == txtUser && m.year == txtPwd).SingleOrDefault();
            if (member == null)
            {
                return RedirectToAction("Login", new { status = 0 });
            }

            //new cookie for user
            var Cookie = new HttpCookie("Account");
            Cookie.Values["memberPhone"] = txtUser;
            Cookie.Values["id"] = member.member_id.ToString();
            Cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(Cookie);


            //check back link to redirect
            if (url == "" || Request.Url.ToString().ToUpper() == url.ToUpper())
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(url);
            }

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string txtUserR, string txtPwdR, string txtRePwdR, string txtName, string txtEmail, string txtLine)
        {

            //check null var
            if (txtPwdR == null || txtUserR == null || txtRePwdR == null || txtName == null)
            {
                return RedirectToAction("Login");
            }

            //checck repass and padd is equal
            if (!txtRePwdR.Equals(txtPwdR))
            {
                return RedirectToAction("Login");
            }

            //check mobile is exist?
            if (_db.bigeyedev_member.Where(m => m.mobile == txtUserR).SingleOrDefault() != null)
            {
                return RedirectToAction("Login", new { status = 2 });
            }

            //insert data
            try
            {
                var e = _db.bigeyedev_member.Add(new bigeyedev_member()
                {
                    mobile = txtUserR,
                    year = txtPwdR,
                    name = txtName,
                    email = txtEmail,
                    line_id = txtLine
                });
                var d = _db.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Login");
            }

            return RedirectToAction("Login", new { status = 3 });
        }



        public ActionResult Singout()
        {
            //check ว่า มี cookie การล็อคอินหรือป่าว
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Index");
            }

            //ลบ cookie login
            Response.Cookies["Account"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {

            return View();
        }























    public ActionResult Contact()
    {
        ViewBag.Message = "Your contact page.";

        return View();
    }







    }
}