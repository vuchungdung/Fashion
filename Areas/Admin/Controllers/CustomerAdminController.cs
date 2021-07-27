using Fashion.Library;
using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerAdminController : Controller
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
        public ActionResult Create(Customer model)
        {
            try
            {
                Customer entity = new Customer();
                db.Customers.Add(entity);
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
            var result = db.Customers.OrderByDescending(x => x.Id).ToList();
            return View(result);
        }
        [HttpGet]
        public ActionResult Update(int id)
        {
            var entity = db.Customers.Find(id);
            return View(entity);
        }
        [HttpPost]
        public ActionResult Update(Customer Customer)
        {
            try
            {
                Customer entity = db.Customers.Find(Customer.Id);
                db.SaveChanges();
                Notification.set_flash("Cập nhật danh mục thành công!", "success");
                return RedirectToAction("List");
            }
            catch
            {
                Notification.set_flash("Cập nhật danh mục thất bại!", "danger");
                throw;
            }
        }
    }
}