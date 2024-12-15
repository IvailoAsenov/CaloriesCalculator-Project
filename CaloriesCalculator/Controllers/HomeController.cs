﻿using CaloriesCalculator.Data;
using CaloriesCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace CaloriesCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Assuming your data model classes are in a namespace like CaloriesCalculator.Data and CaloriesCalculator.Models respectively
            var categories = await _context.Categories
                            .Select(c => new CaloriesCalculator.Models.Category
                            {
                                Id = c.Id,
                                Name = c.Name,
                                Calories = c.Calories // Adjust based on your properties and class mappings
                            })
                                           .ToListAsync();
            return View(categories);
        }


        [HttpGet]
        public JsonResult GetFoodSuggestions(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<Food>()); // Връща празен списък, ако няма въведен текст
            }

            // Взимане на съвпадения от базата данни
            var suggestions = _context.Categories
                .Where(f => f.Name.Contains(query))
                .Select(f => new { f.Name, f.Calories }) // Взимаш само необходимите данни
                .ToList();

            return Json(suggestions);
        }

        [HttpPost]
        public async Task<IActionResult> CalculateCalories(IFormCollection form)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var selectedFoodItems = _context.Categories.ToList(); // Fetch the categories again if necessary
            int totalCalories = 0;

            var gramsInput = form["grams"];
            if (int.TryParse(gramsInput, out int grams) && grams > 0)
            {
                foreach (var category in selectedFoodItems)
                {
                    // Calculate calories based on grams and calories per 100 grams
                    int caloriesPer100g = category.Calories;
                    totalCalories += (caloriesPer100g * grams) / 100;
                }
            }

            ViewData["TotalCalories"] = totalCalories;
            return RedirectToAction("Progres");
        }



        public IActionResult Sport()
        {
            return View();
        }

        public IActionResult Food()
        {
            return View();
        }

        public async Task<IActionResult> Progres()
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var progressEntries = await _context.ProgressEntries
                .Where(p => p.UserId == userId)
                .OrderBy(p => p.Date)
                .ToListAsync();

            var weeklyGoal = await _context.UserSettings
                .Where(u => u.UserId == userId)
                .Select(u => u.WeeklyTargetCalories)
                .FirstOrDefaultAsync();

            if (weeklyGoal == 0)
            {
                weeklyGoal = 0;
            }

            var totalCalories = progressEntries.Sum(p => p.Calories);

            ViewBag.WeeklyGoal = weeklyGoal;
            ViewBag.TotalCalories = totalCalories;

            return View(progressEntries);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateGoal(int weeklyGoal)
        {
            if (weeklyGoal <= 0)
            {
                ModelState.AddModelError("", "Целта трябва да е положително число.");
                return RedirectToAction(nameof(Progres));
            }

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userSettings = await _context.UserSettings
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userSettings != null)
            {
                userSettings.WeeklyTargetCalories = weeklyGoal;
            }
            else
            {
                userSettings = new UserSettings
                {
                    UserId = userId,
                    WeeklyTargetCalories = weeklyGoal,
                    TargetCalories = 2000 
                };

                _context.UserSettings.Add(userSettings);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Progres));
        }

        [HttpPost]
        public async Task<IActionResult> AddCalories(DateTime date, int calories)
        {
            var userId = GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (calories <= 0)
            {
                ModelState.AddModelError("", "Калориите трябва да бъдат положителни.");
                return RedirectToAction(nameof(Progres));
            }

            var entry = await _context.ProgressEntries
                .FirstOrDefaultAsync(p => p.Date == date && p.UserId == userId);

            if (entry == null)
            {
                entry = new ProgressEntry
                {
                    UserId = userId,
                    Date = date,
                    Calories = calories
                };
                _context.ProgressEntries.Add(entry);
            }
            else
            {
                entry.Calories += calories;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Progres));
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
