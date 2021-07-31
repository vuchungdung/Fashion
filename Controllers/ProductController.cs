using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class ProductController : Controller
    {
        public FSDbContext db = new FSDbContext();
        public ActionResult Index(int id)
        {
            var product = db.Products.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.ListColors = db.Colors.ToList();
            ViewBag.ListSizes = db.Sizes.ToList();
            var query = from c in db.Customers
                        join cm in db.Comments
                        on c.Id equals cm.CustomerId
                        where cm.ProductId == id
                        select new CommentViewModel()
                        {
                            Name = c.Name,
                            CreatedDate = cm.CreatedDate,
                            Content = cm.Content
                        };
            ViewBag.ListComments = query.ToList().OrderByDescending(x => x.CreatedDate).ToList();
            ViewBag.ListProducts = db.Products.Where(x => x.ID != product.ID && x.CategoryId == product.CategoryId).Take(5).ToList();
            return View(product);
        }
        public ActionResult List(int? id)
        {
            var list = db.Products.ToList();
            if(id != null)
            {
                list = list.Where(x => x.CategoryId == id).ToList();
            }
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.Products = list.Where(x => x.ActivePromotion == true).ToList();
            return View(list);
        }
        public ActionResult Count(int pId,int? sId,int? cId)
        {
            var list = db.ProductOptions.Where(x => x.ProductId == pId).ToList();
            if(cId != 0)
            {
                list = list.Where(x => x.ColorId == cId).ToList();
            }
            if(sId != 0)
            {
                list = list.Where(x => x.SizeId == sId).ToList();
            }
            int total = 0;
            foreach(var item in list)
            {
                total = total + item.Count;
            }
            return Json(total, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddCart(int pid, int qty, int cId, int sId)
        {
            if((Customer)Session["CUS"] == null)
            {
                return Json(new { result = 4 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var query = from p in db.Products
                            join op in db.ProductOptions on p.ID equals op.ProductId
                            join c in db.Colors on op.ColorId equals c.ID
                            join s in db.Sizes on op.SizeId equals s.ID
                            where p.ID == pid && c.ID == cId && s.ID == sId
                            select new CartViewModel()
                            {
                                ProductID = pid,
                                Name = p.Name,
                                Quantity = qty,
                                Price = (p.ActivePromotion == true ? p.Price : p.PromotionPrice),
                                Image = p.Image,
                                SizeId = sId,
                                ColorId = cId,
                                ColorName = c.Name,
                                SizeName = s.Name
                            };
                var _p = query.FirstOrDefault();
                var cart = Session["Cart"];
                if (cart == null)
                {
                    var list = new List<CartViewModel>();
                    list.Add(_p);
                    Session["Cart"] = list;
                    return Json(new { result = 1 },JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var list = (List<CartViewModel>)cart;

                    if (list.Exists(m => m.ProductID == pid && m.SizeId == sId && m.ColorId == cId))
                    {
                        foreach (var item in list)
                        {
                            if (item.ProductID == pid)
                                item.Quantity += qty;
                            return Json(new { result = 2 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        list.Add(_p);
                        return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { result = 0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult RemoveAll()
        {
            Session.Remove("Cart");
            Notification.set_flash("Đã xóa toàn bộ sản phẩm trong giỏ hàng!", "success");
            return Redirect("~/gio-hang");
        }
        public ActionResult Checkout()
        {
            if (Session["User_Name"] != null && Session["Cart"] != null)
            {
                int user_id = Convert.ToInt32(Session["User_ID"]);
                ViewBag.Info = db.Users.Where(m => m.Id == user_id).First();
            }
            else
                return RedirectToAction("Index", "Cart");
            return View();
        }
        [HttpPost]
        public JsonResult Payment(String Address, String FullName, String Phone, String Email)
        {
            var order = new Order();
            int user_id = Convert.ToInt32(Session["User_ID"]);
            order.Code = DateTime.Now.ToString("yyyyMMddhhMMss"); // yyyy-MM-dd hh:MM:ss
            order.Status = 1;
            db.Orders.Add(order);
            db.SaveChanges();

            var OrderID = order.ID;

            foreach (var c in (List<CartViewModel>)Session["Cart"])
            {
                var orderdetails = new OrderDetail();
            }
            db.SaveChanges();

            Session.Remove("Cart");
            Notification.set_flash("Bạn đã đặt hàng thành công!", "success");
            return Json(true);

        }
        public JsonResult Update(int pid, String option)
        {
            var sCart = (List<CartViewModel>)Session["Cart"];
            CartViewModel c = sCart.First(m => m.ProductID == pid);
            if (c != null)
            {
                switch (option)
                {
                    case "add":
                        c.Quantity++;
                        return Json(1);
                    case "minus":
                        c.Quantity--;
                        return Json(2);
                    case "remove":
                        sCart.Remove(c);
                        if (sCart.Count() == 0)
                            Session.Remove("Cart");
                        return Json(3);
                    default:
                        break;
                }
            }
            return Json(null);
        }
        [HttpGet]
        public JsonResult ListCart()
        {
            var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
            var result = "";
            if (cart != null)
            {
                float? total = 0;
                foreach(var item in cart)
                {
                    total = total + item.Price * item.Quantity;
                    var html = "<div class=\"cart-row\"> <a href=\"/product/index/"+item.ProductID+"\" class=\"img\"><img src=\""+item.Image+"\" alt=\"image\" class=\"img-responsive\"></a> <div class=\"mt-h\"> <span class=\"mt-h-title\"><a href=\"/product/index/"+item.ProductID+"\">"+item.Name+"</a></span> <span class=\"price\"> "+item.Price+" VNĐ</span> <span class=\"mt-h-title\">Số lượng: "+item.Quantity+ "</span><span class=\"mt-h-title\">Màu sắc: " + item.ColorName + "</span> <span class=\"mt-h-title\">Kích cỡ: " + item.SizeName + "</span> </div> <a href=\"#\" class=\"close fa fa-times\"></a></div>";
                    result = result + html;
                }
                var end = "<div class=\"cart-row-total\"> <span class=\"mt-total\">Tổng tiền</span> <span class=\"mt-total-txt\">"+total+" VNĐ</span></div><div class=\"cart-btn-row\"> <a href=\"/product/\" class=\"btn-type2\">Xem</a> <a href=\"\" class=\"btn-type3\">Thanh toán</a></div>";
                result = result + end;
                return Json(new {cart = cart, result = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
    }
}