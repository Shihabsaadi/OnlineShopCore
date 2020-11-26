using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Customer.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public Products CheckValidation(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var model = _db.Products.Find(id);
            if (model == null)
            {
                return null;
            }
            return model;
        }
        public IActionResult Index()
        {
            var models = _db.Products.Include(x => x.ProductTypes).ToList();
            return View(models);
        }
        public IActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products model,IFormFile image)
        {
            if(ModelState.IsValid)
            {
                if(image!=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name,FileMode.Create));
                    model.Image = "images/" + image.FileName;
                }
                else
                {
                    model.Image = "images/NoImage.jpg";
                }
                _db.Products.Add(model);
                await _db.SaveChangesAsync();
                TempData["save"] = model.Product + " saved successfully!!!";
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
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products model, IFormFile image)
        {
            if (ModelState.IsValid)
            {

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    model.Image = "images/" + image.FileName;
                }
                else
                {
                    model.Image = "images/NoImage.jpg";
                }
                _db.Products.Update(model);
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
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
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
        public async Task<IActionResult> Delete(Products model)
        {
            if (model.Id >0)
            {
                _db.Products.Remove(model);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}