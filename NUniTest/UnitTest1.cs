using CaloriesCalculator.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace NUniTest
{
    public class Tests
    {
        private Mock<ApplicationDbContext> _mockContext;
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Structure", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "<Pending>")]
        private HomeController _controller;

        public Tests(HomeController controller)
        {
            _controller = controller;
        }

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>(MockBehavior.Strict);
            _controller = new HomeController(Mock.Of<ILogger<HomeController>>(), _mockContext.Object);
        }

        [Test]
        public async Task Index_ReturnsViewResult_WithCategoriesList()
        {
          
            var categoriesList = new List<Category>
            {
                new Category { Id = 1, Name = "Fruits", Calories = 50 },
                new Category { Id = 2, Name = "Vegetables", Calories = 30 }
            };

          
            var result = await _controller.Index();

            
            var viewResult = result as ViewResult;
            var model = viewResult?.Model as List<Category>;
            Assert.IsNotNull(viewResult);
            Assert.AreEqual(categoriesList, model);
        }

        [Test]
        public void GetFoodSuggestions_ReturnsJsonResult_WithFilteredSuggestions()
        {
            
            var categoriesList = new List<Category>
            {
                new Category { Id = 1, Name = "Apple", Calories = 52 },
                new Category { Id = 2, Name = "Banana", Calories = 89 }
            };





        }

        [Test]
        public async Task CalculateCalories_ReturnsRedirectToProgres_WithTotalCalories()
        {
            
            var form = new Mock<IFormCollection>();
            form.Setup(f => f["grams"]).Returns("100");
            var categoriesList = new List<Category>
            {
                new Category { Id = 1, Name = "Apple", Calories = 52 },
                new Category { Id = 2, Name = "Banana", Calories = 89 }
            };


            
            var result = await _controller.CalculateCalories(form.Object);

            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Progres", redirectResult.ActionName);
        }

        [Test]
        public async Task UpdateGoal_UpdatesWeeklyGoal_WhenValid()
        {
            
            var weeklyGoal = 2000;
            var userId = "user1";
            var userSettings = new UserSettings { UserId = userId, WeeklyTargetCalories = 1500 };


            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

           
            var result = await _controller.UpdateGoal(weeklyGoal);

            
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Progres", redirectResult.ActionName);
        }

        [Test]
        public async Task AddCalories_AddsCaloriesToProgressEntry_WhenValid()
        {
            
            var date = DateTime.Now;
            var calories = 250;
            var userId = "user1";

            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            
            var result = await _controller.AddCalories(date, calories);

            
            var redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Progres", redirectResult.ActionName);
        }

        [TearDown]
        public void TearDown()
        {
            _mockContext = null;
            _controller = null;
        }
    }
}
