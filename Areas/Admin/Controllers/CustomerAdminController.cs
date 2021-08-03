using Fashion.Models;
using Fashion.ViewModel;
using System.Linq;
using System.Web.Mvc;

namespace Fashion.Areas.Admin.Controllers
{
    [Authorize]
    public class CustomerAdminController : Controller
    {
        private FSDbContext db = new FSDbContext();
        public ActionResult Index()
        {
            var query = from od in db.Orders
                        join c in db.Customers
                        on od.CustomerId equals c.Id
                        where od.Status == 4
                        group od by new { c.Id, c.Name, c.Email, c.Phone, c.Username, c.Address } into groupc
                        orderby groupc.Key.Id descending
                        select new CustomerViewModel()
                        {
                            Id = groupc.Key.Id,
                            Name = groupc.Key.Name,
                            Email = groupc.Key.Email,
                            Phone = groupc.Key.Phone,
                            Username = groupc.Key.Username,
                            Count = groupc.Sum(c => c.Total)
                        };
            var result = query.ToList();
            return View(result);
        }
        public ActionResult List()
        {
            var result = db.Customers.ToList();
            return View(result);
        }
    }
}