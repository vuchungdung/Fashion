using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    public class ProductOptionAdminController : Controller
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
        public ActionResult Create(ProductOption model)
        {
            try
            {
                ProductOption entity = new ProductOption();
                entity.ColorId = model.ColorId;
                entity.SizeId = model.SizeId;
                entity.Count = model.Count;
                entity.ProductId = model.ProductId;
                entity.CreatedDate = DateTime.Now;
                db.ProductOptions.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm thông tin sản phẩm thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm thông tin sản phẩm thất bại!", "danger");
                throw;
            }
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.ProductOptions.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(ProductOption ProductOption)
        {
            try
            {
                ProductOption entity = db.ProductOptions.Find(ProductOption.Id);
                entity.ColorId = ProductOption.ColorId;
                entity.SizeId = ProductOption.SizeId;
                entity.ProductId = ProductOption.ProductId;
                entity.Count = ProductOption.Count;
                db.SaveChanges();
                Notification.set_flash("Cập nhật thông tin sản phẩm thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật thông tin sản phẩm thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var model = db.ProductOptions.Where(x => x.Id == id).FirstOrDefault();
                db.ProductOptions.Remove(model);
                db.SaveChanges();
                Notification.set_flash("Xóa thành công!", "success");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Notification.set_flash("Xóa sản phẩm thất bại!", "danger");
                throw;
            }
        }
    }
}