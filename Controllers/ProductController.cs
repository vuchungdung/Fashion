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
            ViewBag.ListColors = db.Colors.ToList();
            ViewBag.ListSizes = db.Sizes.ToList();
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
    }
}