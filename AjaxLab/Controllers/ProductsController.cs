using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AjaxLab.Models;
using System.Diagnostics;

namespace AjaxLab.Controllers
{
    public class ProductsController : Controller
    {
        private ProductsModel db = new ProductsModel();

        // GET: Products
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


            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ProductTable(string query, string categoryID, string supplierID)
        {
            if (!String.IsNullOrEmpty(query) && query.ToLower() == "error")
                throw new InvalidOperationException();


            Debug.WriteLine($"cat: {categoryID}, sup: {supplierID}");

            var ctx = new ProductsModel();
            var products = ctx.Products
                .Where(c => c.ProductName.Contains(query.Trim()) || String.IsNullOrEmpty(query.Trim()))
                .Where(c => String.IsNullOrEmpty(categoryID) || categoryID.Equals(""+c.CategoryID) )
                .Where(c => String.IsNullOrEmpty(supplierID) || supplierID.Equals(""+c.SupplierID) )
                .ToList();

            if(Request.IsAjaxRequest())
            {
                Debug.WriteLine("ProductTable: Ajax");
                return PartialView("_ProductTablePartial", products);
            }
            Debug.WriteLine("ProductTable: Non-Ajax");
            return View("ProductTable", products);
        }

    }
}
