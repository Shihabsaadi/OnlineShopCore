using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private ApplicationDbContext _db;
        public ProductTypeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public ProductTypes CheckValidation(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var model = _db.ProductTypes.Find(id);
            if (model == null)
            {
                return null;
            }
            return model;
        }
        public IActionResult Index()
        {
            var models = _db.ProductTypes.ToList();
            return View(models);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTypes model)
        {
            if(ModelState.IsValid)
            {
                _db.ProductTypes.Add(model);
                await _db.SaveChangesAsync();
                TempData["save"] = model.ProductType + " saved successfully!!!";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            var model = CheckValidation(id: id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTypes model)
        {
            if (ModelState.IsValid)
            {
                _db.ProductTypes.Update(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public IActionResult Details(int? id)
        {
            var model = CheckValidation(id: id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public IActionResult Delete(int? id)
        {
            var model = CheckValidation(id: id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ProductTypes model)
        {
            if (model.Id >0)
            {
                _db.ProductTypes.Remove(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}