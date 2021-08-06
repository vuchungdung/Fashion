using Fashion.Library;
using Fashion.Models;
using Fashion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fashion.Controllers
{
    public class CustomerController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            var cus = (Customer)Session["CUS"];
            if(cus != null)
            {
                var entity = db.Customers.Where(x => x.Id == cus.Id).FirstOrDefault();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            model.Password = XString.ToMD5(model.Password);
            var cus = db.Customers.Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();
            if(cus != null)
            {
                Session["CUS"] = cus;
                Session["Cart"] = null;
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Register(Customer model)
        {
            Customer entity = new Customer();
            entity.Username = model.Username;
            entity.Password = XString.ToMD5(model.Password);
            entity.Address = model.Address;
            entity.Phone = model.Phone;
            entity.CreatedDate = DateTime.Now;
            entity.Name = model.Name;
            entity.Email = model.Email;
            db.Customers.Add(entity);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Update(Customer model)
        {
            var entity = db.Customers.Find(model.Id);
            entity.Username = model.Username;
            if(entity.Password != model.Password)
            {
                entity.Password = XString.ToMD5(model.Password);
            }
            entity.Address = model.Address;
            entity.Phone = model.Phone;
            entity.Name = model.Name;
            entity.Email = model.Email;
            db.SaveChanges();
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult ListOrder()
        {
            var cus = (Customer)Session["CUS"];
            if(cus != null)
            {
                var entity = db.Orders.Where(x => x.CustomerId == cus.Id).ToList();
                return View(entity);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Cancel(int Id)
        {
            var result = db.Orders.Where(x => x.ID == Id).FirstOrDefault();
            result.Status = 5;
            var list = db.OrderDetails.Where(x => x.OrderId == result.ID).ToList();
            foreach(var item in list)
            {
                var product = db.ProductOptions.Where(x => x.ProductId == item.ProductId && x.ColorId == item.ColorId && x.SizeId == item.SizeId).FirstOrDefault();
                product.Count = product.Count + item.Quantity;
                db.SaveChanges();
            }
            db.SaveChanges();
            return RedirectToAction("ListOrder");
        }
    }
}