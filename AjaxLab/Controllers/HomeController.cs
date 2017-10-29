using AjaxLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AjaxLab.Controllers
{
    public class HomeController : Controller
    {
        private ProductsModel db = new ProductsModel();

        public ActionResult Index()
        {
            ViewBag.suppliers = new SelectList(db.Suppliers.Select(supplier => new SelectListItem
            {
                Value = supplier.SupplierID.ToString(),
                Text = supplier.CompanyName.ToString()
            }).ToList(), "Value", "Text");

            ViewBag.categories = new SelectList(db.Categories.Select(category => new SelectListItem
            {
                Value = category.CategoryID.ToString(),
                Text = category.CategoryName.ToString()
            }).ToList(), "Value", "Text");


            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}