﻿using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    public class DiscountAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Discount model)
        {
            try
            {
                Discount entity = new Discount();
                entity.Code = model.Code;
                entity.Detail = model.Detail;
                entity.Time = model.Time;
                entity.Status = model.Status;
                entity.CreatedDate = model.CreatedDate;
                db.Discounts.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm mã giảm giá thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm mã giảm giá thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Discounts.OrderByDescending(x => x.Id).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.Discounts.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(Discount Discount)
        {
            try
            {
                Discount entity = db.Discounts.Find(Discount.Id);
                entity.Code = Discount.Code;
                entity.Detail = Discount.Detail;
                entity.Time = Discount.Time;
                db.SaveChanges();
                Notification.set_flash("Cập nhật mã giảm giá thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật mã giảm giá thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var model = db.Discounts.Where(x => x.Id == id).FirstOrDefault();
                db.Discounts.Remove(model);
                db.SaveChanges();
                Notification.set_flash("Xóa thành công!", "success");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Notification.set_flash("Xóa mã giảm giá thất bại!", "danger");
                throw;
            }
        }
        public ActionResult ChangeStatus(int id, bool status)
        {
            var model = db.Discounts.Where(x => x.Id == id).FirstOrDefault();
            model.Status = status;
            db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}