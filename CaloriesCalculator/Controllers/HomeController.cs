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

        public IActionResult Index()
        {
            return View();
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
                weeklyGoal = 14000;
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
                    TargetCalories = 2000 // Default value for daily target
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



//using CaloriesCalculator.Data;
//using CaloriesCalculator.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CaloriesCalculator.Controllers
//{
//    public class FoodController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        // Списък с храните, който ще използваме
//        private readonly List<Food> foodList = new List<Food>
//        {
//            new Food { Name = "Ябълка", CaloriesPer100g = 52 },
//            new Food { Name = "Банан", CaloriesPer100g = 89 },
//            new Food { Name = "Пица", CaloriesPer100g = 266 },
//            new Food { Name = "Пържени картофи", CaloriesPer100g = 312 },
//            new Food { Name = "Авокадо", CaloriesPer100g = 160 },
//            new Food { Name = "Ананас", CaloriesPer100g = 50 },
//            new Food { Name = "Артишок", CaloriesPer100g = 47 }
//        };

//        public FoodController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        // Метод за показване на списък с храни по запитване
//        public IActionResult ShowSuggestions(string query)
//        {
//            var filteredFoods = foodList.Where(f => f.Name.ToLower().Contains(query.ToLower())).ToList();
//            return Json(filteredFoods);
//        }

//        // Метод за добавяне на храна
//        [HttpPost]
//        public async Task<IActionResult> AddFood(string foodName, double quantity)
//        {
//            var food = foodList.FirstOrDefault(f => f.Name == foodName);
//            if (food == null)
//            {
//                return NotFound();
//            }

//            var foodItem = new SelectedFood
//            {
//                Name = food.Name,
//                CaloriesPer100g = food.CaloriesPer100g,
//                Quantity = quantity,
//                TotalCalories = (food.CaloriesPer100g * quantity) / 100
//            };

//            _context.SelectedFoods.Add(foodItem);
//            await _context.SaveChangesAsync();

//            return Ok(foodItem);
//        }

//        // Метод за изчисляване на общите калории
//        public IActionResult CalculateCalories()
//        {
//            var totalCalories = _context.SelectedFoods.Sum(f => f.TotalCalories);
//            return Json(new { totalCalories = totalCalories });
//        }

//        // Метод за започване на ново изчисление (изчистване на текущия списък с храни)
//        public async Task<IActionResult> StartNewCalculation()
//        {
//            _context.SelectedFoods.RemoveRange(_context.SelectedFoods);
//            await _context.SaveChangesAsync();
//            return Ok(new { message = "New calculation started" });
//        }
//    }


//}