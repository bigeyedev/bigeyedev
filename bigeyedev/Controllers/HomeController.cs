﻿using bigeyedev.Models;
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




        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
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



        private Contract bindCookieContract()
        {
            if (Request.Cookies["contract"] == null)
            {
                return null;
            }
            var Cookie = Request.Cookies["contract"];
            var contract = new Contract();
            contract.address.id = Convert.ToInt32(Cookie.Values["id"]);
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
            contract.address.address_order = Convert.ToInt32(Cookie.Values["addressorder"]);
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




        private void deleteCookieStock()
        {
            for (int i = Request.Cookies.Count - 1; i >= 0; i--)
            {
                if (Request.Cookies[i].Name.Substring(0, 5) == "order")
                {
                    var cookie = new HttpCookie(Request.Cookies[i].Name);
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }
        }


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

            var brand = await _db.bigeyedev_brand.ToListAsync();

            if (id == 1)
            {
                var modelItem = new List<stockBindingModel>();

                brand = brand.Where(m => m.brand_group == 1).ToList();

                foreach (var itemBrand in brand)
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
            deleteCookieStock();

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




        public ActionResult Address()
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
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
            int id = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            var address = _db.bigeyedev_member_address.Where(m => m.member_id == id).ToList();
            var contract = new Contract();
            contract.addressList = address;

            return View(contract);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Address(Contract model, int? addressRdo)
        {
            if (addressRdo == null || addressRdo.Value == 0)
            {
                if (!ModelState.IsValid)
                {
                    return RedirectToAction("Address");
                }
            }
            if (model == null)
            {
                return RedirectToAction("Address");
            }

            //clear cookie การติดต่อ ที่อยู่
            if (Request.Cookies["contract"] != null)
            {
                Response.Cookies["contract"].Expires = DateTime.Now.AddDays(-1);

            }


            if (addressRdo != null && addressRdo.Value != 0)
            {
                model = new Contract();
                try
                {
                    model.address = _db.bigeyedev_member_address.Where(m => m.id == addressRdo.Value).Single();
                }
                catch
                {
                    RedirectToAction("Address");
                }

                //ดึงข้อมูล account จาก DB
                try
                {
                    model.memberData = _db.bigeyedev_member.Where(m => m.member_id == model.address.member_id).Single();
                }
                catch
                {
                    return RedirectToAction("Address");
                }
                
            }
            else
            {
                //update email and line id

            }
            var Cookie = new HttpCookie("contract");
            Cookie.Values["id"] = model.address.id.ToString();
            Cookie.Values["name"] = model.address.name;
            Cookie.Values["mobile"] = model.address.mobile_shop;
            Cookie.Values["mobile2"] = model.address.mobile2_shop;
            Cookie.Values["lineid"] = model.memberData.line_id;
            Cookie.Values["email"] = model.memberData.email;
            Cookie.Values["shopname"] = model.address.shop_name;
            Cookie.Values["address"] = model.address.address;
            Cookie.Values["distict"] = model.address.sub_district;
            Cookie.Values["area"] = model.address.district;
            Cookie.Values["province"] = model.address.province;
            Cookie.Values["zipcode"] = model.address.zip;
            if (model.address.address_order > 0)
            {
                // ตรวจสอบถ้า เป็น ที่อยู่เดิม ให้เอาเข้า cookie ไปด้วย เพื่อเอาไป บวกเพิ่ม
                Cookie.Values["addressorder"] = model.address.address_order.ToString();
            }
            Cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(Cookie);


            return RedirectToAction("Review");
        }


        public ActionResult Review()
        {
            if (Request.Cookies["confirm"] == null)
            {
                return RedirectToAction("Cart");
            }
            if (Request.Cookies["confirm"].Value != "1")
            {
                return RedirectToAction("Cart");
            }
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            var contract = bindCookieContract();
            if (contract == null)
            {
                return RedirectToAction("Address");
            }

            var itemStock = bindCookieStock();
            if (itemStock == null)
            {
                return RedirectToAction("Index");
            }

            return View(new Tuple<Contract, List<stockBindingModel>>(contract, itemStock));
        }






        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Review(int? replace, int? replace_fac, int? txtFix)
        {
            //check ว่ามี ออเดอร์ใน cookie ไหม
            bool found = chkCookie();

            if (!found)
            {
                return RedirectToAction("Index");
            }
            if (Request.Cookies["confirm"] == null)
            {
                return RedirectToAction("Cart");
            }
            if (Request.Cookies["confirm"].Value != "1")
            {
                return RedirectToAction("Cart");
            }
            if (Request.Cookies["contract"] == null)
            {
                return RedirectToAction("Address");
            }


            //add address to database
            var contract = bindCookieContract();
            if (contract.address.address_order == 0)
            {
                contract.address.member_id = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
                contract.address.address_order += 1;
                _db.bigeyedev_member_address.Add(contract.address);
            }
            else
            {
                var updateAddress = _db.bigeyedev_member_address.Find(contract.address.id);
                updateAddress.address_order += 1;
            }
            try
            {
                _db.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Result", new { id = 0 });
            }

            //ini fixValue Fashion = 0
            int fashionFixValue = 0;
            if (txtFix != null)
            {
                fashionFixValue = txtFix.Value;
            }
            //add order
            var order = new bigeyedev_order()
            {
                datetime = DateTime.Now,
                address_id = contract.address.id,
                member_id = Convert.ToInt32(Request.Cookies["Account"].Values["id"])
                //add เพิ่มเงื่อนไข replace
            };
            if (replace != null)
            {
                order.note_fashion2_replace = replace.Value;
                if (replace == 3)
                {
                    order.fix_volume_fashion = fashionFixValue;
                }
            }
            if (replace_fac != null)
            {
                order.note_fashion3_replace_fac = replace_fac.Value;
            }
            try
            {
                _db.bigeyedev_order.Add(order);
                _db.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Result", new { id = 1 });
            }


            //add fashion item
            int orderId = order.order_id;
            var stock = bindCookieStock();
            var product = new List<bigeyedev_order_fashion_item>();
            foreach (var item in stock)
            {
                product.Add(new bigeyedev_order_fashion_item()
                {
                    datetime = DateTime.Now,
                    order_id = orderId,
                    product_id = item.id,
                    brand = item.brand,
                    model = item.model,
                    near = item.near,
                    black = item.Black,
                    blue = item.Blue,
                    brown = item.Brown,
                    choco = item.Choco,
                    gold = item.Gold,
                    gray = item.Gray,
                    green = item.Green,
                    pink = item.Pink,
                    red = item.Red,
                    silver = item.Silver,
                    sky = item.Sky,
                    violet = item.Violet
                });
            }
            _db.bigeyedev_order_fashion_item.AddRange(product);
            try
            {
                _db.SaveChanges();
            }
            catch
            {
                _db.bigeyedev_order.Remove(order);
                _db.SaveChanges();
                deleteCookieStock();
                return RedirectToAction("Result", new { id = 2 });
            }

            //dalete cookie
            deleteCookieStock();
            //delete comfirm cookie
            Response.Cookies["confirm"].Expires = DateTime.Now.AddDays(-1);

            return RedirectToAction("Result", new { id = orderId });
        }





        public ActionResult Login(int? status,string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            
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
        public ActionResult Login(string txtUser, string txtPwd, string returnUrl)
        {
            //check varible is null
            if (txtUser == null || txtPwd == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            //select data member from database
            var member = _db.bigeyedev_member.Where(m => m.mobile == txtUser && m.year == txtPwd).SingleOrDefault();
            if (member == null)
            {
                return RedirectToAction("Login", new { status = 0, returnUrl =  Request.Url.AbsolutePath  });
            }

            //new cookie for user
            var Cookie = new HttpCookie("Account");
            Cookie.Values["memberPhone"] = txtUser;
            Cookie.Values["id"] = member.member_id.ToString();
            Cookie.Expires = DateTime.Now.AddDays(1);
            Response.Cookies.Add(Cookie);


            //check back link to redirect
            return RedirectToLocal(returnUrl);

        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string txtUserR, string txtPwdR, string txtRePwdR, string txtName, string txtEmail, string txtLine)
        {

            //check null var
            if (txtPwdR == null || txtUserR == null || txtRePwdR == null || txtName == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath});
            }
            
            //checck repass and padd is equal
            if (!txtRePwdR.Equals(txtPwdR))
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            //check mobile is exist?
            if (_db.bigeyedev_member.Where(m => m.mobile == txtUserR).SingleOrDefault() != null)
            {
                return RedirectToAction("Login", new { status = 2 , returnUrl =  Request.Url.AbsolutePath});
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
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            return RedirectToAction("Login", new { status = 3 ,returnUrl =  Request.Url.AbsolutePath});
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
            Response.Cookies["comfirm"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["contract"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Index");
        }



        public ActionResult Result(int? id)
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Index");
            }
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            //check error 1 , 2 ,3 is error
            int idResult = id.Value;

            return View(idResult);
        }



        [HttpGet]
        public ActionResult Payment(int? id)
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            var order = new List<bigeyedev_order>();
            int memberID = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            //statusView 0 = แจ้งโอนเงิน , 1= ดูรายละเอียดการชำระเงิน
            int statusView = 1;
            if (id == null)
            {
                statusView = 0;
                order = _db.bigeyedev_order.Where(m => m.member_id == memberID && m.pay_amount == null).ToList();
            }
            else
            {
                order = _db.bigeyedev_order.Where(m => m.order_id == id.Value && m.member_id == memberID).ToList();
            }


            if (order.Count == 0)
            {
                return RedirectToAction("Index");
            }

            return View(new Tuple<List<bigeyedev_order>, int>(order, statusView));
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Payment(bigeyedev_order model)
        {
            if (model == null)
            {
                return RedirectToAction("Payment");
            }
            try
            {
                var update = _db.bigeyedev_order.Find(model.order_id);
                update.pay_amount = model.pay_amount;
                update.pay_bank = model.pay_bank;
                update.pay_time = model.pay_time;
                update.pay_date = model.pay_date;
                _db.SaveChanges();
            }
            catch
            {
                return RedirectToAction("Payment");
            }

            return RedirectToAction("Payment");
        }




        public ActionResult Tracking()
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }
                        
            // feath data order form DB
            int memberID = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            var tracking = _db.bigeyedev_order.Where(m => m.pay_finish == 1 && m.member_id == memberID).ToList();
             
            //order data by order_ID
            return View(tracking.OrderByDescending(m=>m.order_id).ToList());
        }




        

        public ActionResult Order(int? id)
        {
            if(Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }
            //ตัวบ่งบอกว่า รับค่า id มาหรือไม่ เอาไว้สร้างเงื่อนไขในหน้า view
            int check = 0;
            //รับค่า member id จาก cookie
            int memberID = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            // ประกาศตัวรับค่าจาก DB
            var order = new List<bigeyedev_order>();

            //ตรวจสอบว่า มีการรับ id มาหรือป่าว
            if (id == null)
            {
                order = _db.bigeyedev_order.Where(m => m.member_id == memberID).ToList();
            }
            else
            {
                check = 1;
                int orderID = id.Value;
                order = _db.bigeyedev_order.Where(m => m.order_id == orderID).ToList();
            }

            return View(new Tuple<List<bigeyedev_order>,int>(order.OrderByDescending(m=>m.order_id).ToList(),check));
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
            foreach(var item in orderItem)
            {
                try
                {
                    productItem.Add(_db.product.Where(m => m.id == item.product_id).Select(u => u.image_url).Single());
                }
                catch {}
                
            }

            return PartialView(new Tuple<List<bigeyedev_order_fashion_item>,List<string>>(orderItem,productItem));
        }






        public ActionResult Account()
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            //ค้นหาที่อยู่ของ account นี้
            var memberID = Convert.ToInt16(Request.Cookies["Account"].Values["id"]);
            var ModelAddress = _db.bigeyedev_member_address.Where(m => m.member_id == memberID).ToList();
            //featch account data
            var modelAccount = _db.bigeyedev_member.Find(memberID);

            return View(new Tuple<bigeyedev_member, List<bigeyedev_member_address>>(modelAccount, ModelAddress));
        }




        public ActionResult AccountEdit()
        {
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }
            int memberID = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            var modelAccount = _db.bigeyedev_member.Find(memberID);


            return View(modelAccount);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AccountEdit(bigeyedev_member model)
        {
            if (model == null)
            {
                RedirectToAction("AccountEdit");
            }
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Login", new { returnUrl =  Request.Url.AbsolutePath });
            }

            int memberID = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);

            var memberUpdate = _db.bigeyedev_member.Find(memberID);
            memberUpdate.name = model.name; 
            memberUpdate.email = model.email;
            memberUpdate.mobile2 = model.mobile2;
            memberUpdate.line_id = model.line_id;
            try
            {
                _db.SaveChanges();
            }
            catch
            {
                return View(model);
            }


            return RedirectToAction("Account");
        }





        public ActionResult  AddressEdit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Account");
            }
            if (Request.Cookies["Account"] == null)
            {
                return RedirectToAction("Account");
            }
            int memberID = Convert.ToInt32(Request.Cookies["Account"].Values["id"]);
            int addressID = id.Value;
            var modelAddress = new bigeyedev_member_address();
            try
            {
                 modelAddress = _db.bigeyedev_member_address.Where(m => m.id == addressID && m.member_id == memberID).Single();
            }
            catch
            {
                return RedirectToAction("Account");
            }
            

            return View(modelAddress);
        }





        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddressEdit(bigeyedev_member_address model)
        {
            if (model == null)
            {
                return RedirectToAction("AddressEdit");
            }
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddressEdit");
            }

            try
            {
                var modelAddress = _db.bigeyedev_member_address.Find(model.id);
                modelAddress.name = model.name;
                modelAddress.shop_name = model.shop_name;
                modelAddress.mobile_shop = model.mobile_shop;
                modelAddress.mobile2_shop = model.mobile2_shop;
                modelAddress.address = model.address;
                modelAddress.sub_district = model.sub_district;
                modelAddress.district = model.district;
                modelAddress.province = model.province;
                modelAddress.zip = model.zip;
                _db.SaveChanges();
            }
            catch
            {
                return RedirectToAction("AddressEdit", new { id = model.id});
            }

            return RedirectToAction("Account");
        }







        public async Task<ActionResult> Search(string id)
        {
            var model = await _db.product.Where(m => m.visible == 1&& m.model.Contains(id) && (m.black > 0 || m.choco > 0 || m.gray > 0 || m.brown > 0 || m.blue > 0 || m.green > 0 || m.violet > 0 || m.pink > 0 || m.silver > 0 || m.gold > 0 || m.sky > 0 || m.red > 0)).Select(u => new stockBindingModel
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


            return View(new Tuple<List<stockBindingModel>, List<bigeyedev_brand>>(model, brand));
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