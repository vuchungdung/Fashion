﻿using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class CartController : Controller
    {
        public FSDbContext db = new FSDbContext();

        [HttpPost]
        public ActionResult AddCart(int pid, int qty, int cId, int sId)
        {
            if ((Customer)Session["CUS"] == null)
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
                    return Json(new { result = 1 }, JsonRequestBehavior.AllowGet);
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
            return RedirectToAction("ViewCart");
        }
        public ActionResult Checkout()
        {
            if (Session["CUS"] != null && Session["Cart"] != null)
            {
                var user = (Customer)Session["CUS"];
                ViewBag.Info = user;
            }
            else
                return RedirectToAction("Index", "Cart");
            return View();
        }
        [HttpPost]
        public JsonResult Payment()
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
        [HttpGet]
        public ActionResult Update(int pId,int sId, int cId,int count)
        {
            var cart = (List<CartViewModel>)Session["Cart"];
            var item = cart.Where(x => x.ProductID == pId && x.SizeId == sId && x.ColorId == cId).FirstOrDefault();
            item.Quantity = count;
            return Json(true,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListCart()
        {
            var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
            var result = "";
            if (cart != null)
            {
                float? total = 0;
                foreach (var item in cart)
                {
                    total = total + item.Price * item.Quantity;
                    var html = "<div class=\"cart-row\"> <a href=\"/product/index/" + item.ProductID + "\" class=\"img\"><img src=\"" + item.Image + "\" alt=\"image\" class=\"img-responsive\"></a> <div class=\"mt-h\"> <span class=\"mt-h-title\"><a href=\"/product/index/" + item.ProductID + "\">" + item.Name + "</a></span> <span class=\"price\"> " + item.Price + " VNĐ</span> <span class=\"mt-h-title\">Số lượng: " + item.Quantity + "</span><span class=\"mt-h-title\">Màu sắc: " + item.ColorName + "</span> <span class=\"mt-h-title\">Kích cỡ: " + item.SizeName + "</span> </div> <a href=\"#\" class=\"close fa fa-times\"></a></div>";
                    result = result + html;
                }
                var end = "<div class=\"cart-row-total\"> <span class=\"mt-total\">Tổng tiền</span> <span class=\"mt-total-txt\">" + total + " VNĐ</span></div><div class=\"cart-btn-row\"> <a href=\"/cart/viewcart\" class=\"btn-type2\">Xem</a> <a href=\"/cart/viewcart\" class=\"btn-type3\">Thanh toán</a></div>";
                result = result + end;
                return Json(new { cart = cart, result = result }, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ViewCart(string code = "")
        {
            if (Session["CUS"] != null)
            {
                var carttotal = new CartTotal();
                float? _price = 0;
                var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
                foreach(var item in cart)
                {
                    _price = _price + item.Price * item.Quantity;
                }
                carttotal.Total = _price;
                if (String.IsNullOrEmpty(code))
                {
                    carttotal.Payment = _price;
                }
                else
                {
                    var entity = db.Discounts.Where(x => x.Code == code && x.Status == true).FirstOrDefault();
                    if(entity != null)
                    {
                        carttotal.Code = entity.Value.ToString();
                        carttotal.Payment = _price - ((_price * entity.Value) / 100);
                    }
                }
                ViewBag.Carttotal = carttotal;
                return View(cart);
            }
            else
                return RedirectToAction("Index", "Home");
            
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult RemoveItem(int pId,int sId,int cId)
        {
            var cart = (List<Fashion.ViewModel.CartViewModel>)Session["Cart"];
            var item = cart.Where(x => x.ProductID == pId && x.SizeId == sId && x.ColorId == cId).FirstOrDefault();
            cart.Remove(item);
            return RedirectToAction("ViewCart");
        }
    }
}