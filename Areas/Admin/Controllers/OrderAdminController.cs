﻿using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    [Authorize]
    public class OrderAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index(int? status)
        {
            var result = db.Orders.ToList();
            return View(result);
        }
        public ActionResult ChangeStatus(int Id, int status)
        {
            var entity = db.Orders.Where(x => x.ID == Id).FirstOrDefault();
            entity.Status = status;
            db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Detail(int Id)
        {
            var query = from o in db.Orders
                        join od in db.OrderDetails on o.ID equals od.OrderId
                        join p in db.Products on od.ProductId equals p.ID
                        join s in db.Sizes on od.SizeId equals s.ID
                        join c in db.Colors on od.ColorId equals c.ID
                        select new { };
            var result = "";
            return View(result);
        }
    }
}