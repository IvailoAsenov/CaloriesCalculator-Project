using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CaloriesCalculator.Data;
using System.Linq;
using System.Threading.Tasks;
using Project.Models;

namespace Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]  // Restrict access to Admins only
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Index
        public IActionResult Index()
        {
            // Fetch all categories from the database
            var categories = _context.Categories.ToList();
            return View(categories);
        }

        // GET: Admin/CreateCategory
        public IActionResult CreateCategory()
        {
            // Pass an empty Category object to avoid null reference in the view
            var category = new Category();
            return View(category);
        }

        // POST: Admin/CreateCategory
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                // Add the category to the database
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // Redirect to the categories list page after successfully creating a category
                return RedirectToAction(nameof(Index));
            }

            // If model is not valid, return to the same view with the model to show errors
            return View(category);
        }
    }
}
