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

            // //boytimw 2 ทำตรงไหนอยู่ ก็มาใส่ไว้ตรงนี้ boy
            return RedirectToAction("Fashion_Select");
        }

        public void dd()
        {
            //statment
        }

        public async Task<ActionResult> Fashion_Select()
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


            var model = await _db.product.Where(m => m.visible == 1 && (m.black > 0 || m.choco > 0 || m.gray > 0 || m.brown > 0 || m.blue > 0 || m.green > 0 || m.violet > 0 || m.pink > 0 || m.silver > 0 || m.gold > 0 || m.sky > 0 || m.red > 0)).Select(u => new stockBindingModel
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


            return View(model);
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
                int black = 0; int blue = 0; int brown = 0; int choco = 0; int gold = 0; int gray = 0; int green = 0;
                int pink = 0; int red = 0; int silver = 0; int sky = 0; int violet = 0;
                // if cookie order:id is exist
                if (Request.Cookies["order:" + item.id] != null)
                {
                    black = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Black"]) + item.Black;
                    blue = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Blue"]) + item.Blue;
                    brown = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Brown"]) + item.Brown;
                    choco = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Choco"]) + item.Choco;
                    gold = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Gold"]) + item.Gold;
                    gray = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Gray"]) + item.Gray;
                    green = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Green"]) + item.Green;
                    pink = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Pink"]) + item.Pink;
                    red = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Red"]) + item.Red;
                    silver = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Silver"]) + item.Silver;
                    sky = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Sky"]) + item.Sky;
                    violet = Convert.ToInt32(Request.Cookies["order:" + item.id].Values["Violet"]) + item.Violet;
                }
                else
                {
                    black = item.Black;
                    blue = item.Blue;
                    brown = item.Brown;
                    choco = item.Choco;
                    gold = item.Gold;
                    gray = item.Gray;
                    green = item.Green;
                    pink = item.Pink;
                    red = item.Red;
                    silver = item.Silver;
                    sky = item.Sky;
                    violet = item.Violet;
                }
                //addookie
                var Cookie = new HttpCookie("order:" + item.id);
                Cookie.Values["id"] = item.id.ToString();
                Cookie.Values["model"] = item.model;
                Cookie.Values["brand"] = item.brand;
                Cookie.Values["imageUrl"] = item.imageUrl;
                Cookie.Values["near"] = item.near;
                Cookie.Values["Black"] = black.ToString();
                Cookie.Values["Blue"] = blue.ToString();
                Cookie.Values["Brown"] = brown.ToString();
                Cookie.Values["Choco"] = choco.ToString();
                Cookie.Values["Gold"] = gold.ToString();
                Cookie.Values["Gray"] = gray.ToString();
                Cookie.Values["Green"] = green.ToString();
                Cookie.Values["Pink"] = pink.ToString();
                Cookie.Values["Red"] = red.ToString();
                Cookie.Values["Silver"] = silver.ToString();
                Cookie.Values["Sky"] = sky.ToString();
                Cookie.Values["Violet"] = violet.ToString();
                Cookie.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(Cookie);
                Response.Cookies["confirm"].Value = "0";
            }

            return RedirectToAction("Cart");
        }



        public async Task<ActionResult> Cart()
        {
            var model = bindCookieStock();

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
                    Response.Cookies["confirm"].Value = "1";
                }

            }

            return RedirectToAction("Address");
        }


        public ActionResult Address()
        {

            if (Request.Cookies["confirm"] == null)
            {
                return RedirectToAction("Index");
            }
            if (Request.Cookies["confirm"].Value != "1")
            {
                return RedirectToAction("Cart");
            }
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(bigeyedev_order model)
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
            //Cookie.Values["name"] = model.name;
            //Cookie.Values["mobile"] = model.mobile;
            //Cookie.Values["mobile2"] = model.mobile2;
            //Cookie.Values["lineid"] = model.line_id;
            //Cookie.Values["email"] = model.email;
            //Cookie.Values["shopname"] = model.shop_name;
            //Cookie.Values["address"] = model.address;
            //Cookie.Values["distict"] = model.district;
            //Cookie.Values["area"] = model.area;
            //Cookie.Values["province"] = model.province;
            //Cookie.Values["zipcode"] = model.zip;
            //Cookie.Values["note"] = model.note;
            Cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(Cookie);

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

            return View(new Tuple<bigeyedev_order, List<stockBindingModel>>(contract, itemStock));
        }



        private bigeyedev_order bindCookieContract()
        {
            if (Request.Cookies["contract"] == null)
            {
                return null;
            }
            var Cookie = Request.Cookies["contract"];
            var contract = new bigeyedev_order()
            {
                //name = Cookie.Values["name"],
                //mobile = Cookie.Values["mobile"],
                //mobile2 = Cookie.Values["mobile2"],
                //line_id = Cookie.Values["lineid"],
                //email = Cookie.Values["email"],
                //shop_name = Cookie.Values["shopname"],
                //address = Cookie.Values["address"],
                //district = Cookie.Values["distict"],
                //area = Cookie.Values["area"],
                //province = Cookie.Values["province"],
                //zip = Cookie.Values["zipcode"],
                //note = Cookie.Values["note"]
            };
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


            //order item



            //var product = new List<bigeyedev_order>();
            //foreach (var item in stock)
            



                //order item




                //product.Add(new bigeyedev_order()
                //{
                    //order_id = orderId,
                    //product_id = item.id,
                    //brand = item.brand,
                    //model = item.model,
                    //near = item.near,
                    //black = item.Black,
                    //blue = item.Blue,
                    //brown = item.Brown,
                    //choco = item.Choco,
                    //gold = item.Gold,
                    //gray = item.Gray,
                    //green = item.Green,
                    //pink = item.Pink,
                    //red = item.Red,
                    //silver = item.Silver,
                    //sky = item.Sky,
                    //violet = item.Violet
            //    });
            //}
            //_db.order_item.AddRange(product);
            //if (_db.SaveChanges() <= 0)
            //{
            //    _db.orders.Remove(contract);
            //    _db.SaveChanges();
            //    return View(0);
            //}

            //return View(orderId);
        //}








        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}