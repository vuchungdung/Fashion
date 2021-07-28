using Fashion.Models;
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
            product.ProductOptions = db.ProductOptions.Where(x => x.ProductId == id).ToList();
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
    }
}