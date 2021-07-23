using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    public class CategoryAdminController : Controller
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
        public ActionResult Create(Category model)
        {
            try
            {
                Category entity = new Category();
                entity.Name = model.Name;
                entity.Alias = XString.ToAscii(model.Name);
                db.Categories.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm danh mục thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Thêm danh mục thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Categories.OrderByDescending(x => x.ID).ToList();
            return View(result);
        }
        public ActionResult Item(int id)
        {
            var entity = db.Categories.Find(id);
            return View(entity);
        }
    }
}