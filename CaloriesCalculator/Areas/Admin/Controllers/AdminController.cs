﻿using CaloriesCalculator.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        

        public IActionResult CreateCategory()
        {
           
            var categoryViewModel = new CaloriesCalculator.Models.Category();
            return View(categoryViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CaloriesCalculator.Models.Category categoryViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    
                    var category = new CaloriesCalculator.Data.Category
                    {
                        Name = categoryViewModel.Name,
                        Calories = categoryViewModel.Calories
                    };

                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home", new { area = "" });

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"An error occurred while saving: {ex.Message}");
                    return RedirectToAction("Index", "Home", new { area = "" });

                }
            }

            return RedirectToAction("Index", "Home", new { area = "" });

        }

    }
}
