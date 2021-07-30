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
            var p = db.Products.Where(m => m.Status == true && m.ID == pid).First();
            var op = db.ProductOptions.Where(m => m.ColorId == cId && m.SizeId == sId && m.ProductId == pid).FirstOrDefault();
            var cart = Session["Cart"];
            if (cart == null)
            {
                var item = new CartViewModel();
                item.ProductID = p.ID;
                item.Name = p.Name;
                item.Image = p.Image;
                item.Quantity = qty;
                item.Price = p.Price;
                item.OptionId = op.Id;
                var list = new List<CartViewModel>();
                list.Add(item);
                Session["Cart"] = list;
                return Json(new { result = 1, count = list.Count() });
            }
            else
            {
                var list = (List<CartViewModel>)cart;

                if (list.Exists(m => m.ProductID == pid))
                {
                    foreach (var item in list)
                    {
                        if (item.ProductID == pid)
                            item.Quantity += qty;
                        return Json(new { result = 2, count = list.Count() });
                    }
                }
                else
                {
                    var item = new CartViewModel();
                    item.ProductID = p.ID;
                    item.Name = p.Name;
                    item.Image = p.Image;
                    item.Quantity = qty;
                    item.Price = p.Price;
                    item.OptionId = op.Id;
                    list.Add(item);
                    return Json(new { result = 1, count = list.Count() });
                }
            }
            return Json(new { result = 0});
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
    }
}