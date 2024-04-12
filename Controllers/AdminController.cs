using IntroToIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntroToIdentity.Controllers
{
    [Authorize(Roles = Constants.AdminRole)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AdminController(UserManager<ApplicationUser> um)
        {
            _userManager = um;
        }
    
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddAuthor()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddAuthor(string name)
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddUser(string userName, string pw)
        {
            ApplicationUser newUser = new() { UserName = userName };
            var result = await _userManager.CreateAsync(newUser, pw);

            if (!result.Succeeded)
            {
                // get angry
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        // Extend SeedData to also create the Librarian role
        // Do not need to add a user to this role. 
        // Add a new action to AdminController
        // This action should list all of the users in the system
        // Should include their roles in the listing
        // and permit the admin to add/remove any user from any role
    }
}
