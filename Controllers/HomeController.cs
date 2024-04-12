using IntroToIdentity.Data;
using IntroToIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IntroToIdentity.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<ApplicationUser> um,
            ApplicationDbContext context,
            RoleManager<IdentityRole> rm
            )
        {
            _logger = logger;
            _userManager = um;
            _context = context;
            _roleManager = rm;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ClaimAdminRole()
        {
            if (!await _roleManager.RoleExistsAsync(Constants.AdminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(Constants.AdminRole));
            }

            ApplicationUser? user = GetUser();
            if (user == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await _userManager.AddToRoleAsync(user, Constants.AdminRole);

            return RedirectToAction(nameof(Index));
        }

        private ApplicationUser? GetUser()
        {
            string? userId = _userManager.GetUserId(User);
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }
    }
}
