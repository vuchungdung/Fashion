using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    public class UserAdminController : Controller
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
        public ActionResult Create(User model)
        {
            try
            {
                User entity = new User();
                entity.Name = model.Name;
                entity.Username = model.Username;
                entity.Password = XString.ToMD5(model.Password);
                entity.Email = model.Email;
                entity.Phone = model.Phone;
                entity.Status = model.Status;
                entity.Image = model.Image;
                entity.CreatedDate = DateTime.Now;
                db.Users.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm người dùng thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm người dùng thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Users.OrderByDescending(x => x.Id).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.Users.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(User model)
        {
            try
            {
                User entity = db.Users.Find(model.Id);
                entity.Name = model.Name;
                entity.Username = model.Username;
                entity.Password = XString.ToMD5(model.Password);
                entity.Email = model.Email;
                entity.Phone = model.Phone;
                entity.Status = model.Status;
                entity.Image = model.Image;
                entity.CreatedDate = DateTime.Now;
                db.SaveChanges();
                Notification.set_flash("Cập nhật người dùng thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật người dùng thất bại!", "danger");
                throw;
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                var model = db.Users.Where(x => x.Id == id).FirstOrDefault();
                db.Users.Remove(model);
                db.SaveChanges();
                Notification.set_flash("Xóa thành công!", "success");
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                Notification.set_flash("Xóa người dùng thất bại!", "danger");
                throw;
            }
        }
        public JsonResult ChangeShow(int id, bool status)
        {
            var model = db.Users.Where(x => x.Id == id).FirstOrDefault();
            model.Status = status;
            db.SaveChanges();
            Notification.set_flash("Cập nhật thành công!", "success");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}