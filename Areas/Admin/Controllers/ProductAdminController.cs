using Fashion.Library;
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
            try
            {
                Product entity = new Product();
                entity.Name = product.Name;
                entity.Alias = XString.ToAscii(product.Name);
                entity.CategoryId = product.CategoryId;
                entity.Image = product.Image;
                entity.OriginalPrice = product.OriginalPrice;
                entity.Price = product.Price;
                entity.PromotionPrice = product.PromotionPrice;
                entity.Description = product.Description;
                entity.Content = product.Content;
                entity.HomeFlag = product.HomeFlag;
                entity.HotFlag = product.HotFlag;
                entity.ViewCount = 0;
                entity.Quantity = product.Quantity;
                entity.QrCode = product.QrCode;
                entity.Status = product.Status;
                db.Products.Add(entity);
                db.SaveChanges();
                Notification.set_flash("Thêm mới sản phẩm thành công!", "success");
                return View();
            }
            catch
            {
                Notification.set_flash("Thêm mới sản phẩm thất bại!", "danger");
                throw;
            }
        }
        public ActionResult List()
        {
            var result = db.Products.OrderByDescending(x => x.ID).ToList();
            return View(result);
        }
    }
}