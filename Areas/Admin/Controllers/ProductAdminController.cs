using Fashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.ListCategory = new SelectList(db.Categories.ToList(), "ID", "Name", 0);
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
    }
}