using IntroToIdentity.Models;
using IntroToIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntroToIdentity.Controllers
{
    [Authorize(Roles = Constants.AdminRole)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(
            UserManager<ApplicationUser> um,
            RoleManager<IdentityRole> rm)
        {
            _userManager = um;
            _roleManager = rm;
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

        [HttpGet]
        public async Task<IActionResult> ReviewUsers()
        {
            ReviewUsersViewModel vm = new()
            {
                Roles = _roleManager.Roles.Select(r => r.Name).ToList()!
            };

            foreach (var user in _userManager.Users)
            {
                vm.Members.Add(
                    new ReviewUsersViewModel.Member()
                    {
                        Name = user.UserName ?? "no user name",
                        Roles = (await _userManager.GetRolesAsync(user)).ToHashSet()
                    }
                    );
            }

            return View(vm);
        }

        // Extend SeedData to also create the Librarian role
        // Do not need to add a user to this role. 
        // Add a new action to AdminController
        // This action should list all of the users in the system
        // Should include their roles in the listing
        // and permit the admin to add/remove any user from any role
    }
}
