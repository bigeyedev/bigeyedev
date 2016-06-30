using bigeyedev.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace bigeyedev.Controllers
{
    public class HomeController : Controller
    {

        private sugareyeEntities _db = new sugareyeEntities();


        public ActionResult Index()
        {
            ViewBag.Shop = "BigeyeInw";
            ViewBag.Title = "Fashion Select";

            return RedirectToAction("Fashion_Select");
        }

        private async Task<List<stockBindingModel>> bindStock()
        {
            var model = await _db.products.Where(m => m.visible == 1 && (m.black > 0 || m.choco > 0 || m.gray > 0 || m.brown > 0 || m.blue > 0 || m.green > 0 || m.violet > 0 || m.pink > 0 || m.silver > 0 || m.gold > 0 || m.sky > 0 || m.red > 0)).Select(u => new stockBindingModel
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

            return (model);
        }

        public async Task<ActionResult> Fashion_Select()
        {
            ViewBag.Shop = "BigeyeInw";
            ViewBag.Title = "Fashion Select";

            


            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Fashion_Select(List<stockBindingModel> model)
        {
            if (model == null)
            {
                return RedirectToAction("Fashion_Selec");
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
            }

            return RedirectToAction("Cart");
        }
        public ActionResult Cart()
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
            if (!found)
            {
                return RedirectToAction("Index");
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
            return View(model);
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
            for (int i = Request.Cookies.Count-1; i>=0; i--)
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
                }
            }
            return RedirectToAction("Address");
        }



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