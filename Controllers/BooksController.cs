using IntroToIdentity.Data;
using IntroToIdentity.Models;
using IntroToIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IntroToIdentity.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> um)
        {
            _context = context;
            _userManager = um;
        }

        public async Task<IActionResult> Index()
        {
            var books = _context.Books.ToList();

            ApplicationUser user = await GetUser();

            BooksIndexViewModel vm = new()
            {
                Books = books,
                UserId = user.Id
            };

            return View(vm);
        }

        private async Task<ApplicationUser> GetUser()
        {
            //string? userId = _userManager.GetUserId(User);
            //return _context.Users.FirstOrDefault(u => u.Id == userId);
            
            return await _userManager
                .FindByNameAsync(User.Identity?.Name ?? "")
                ?? throw new Exception("This shouldn't happen");
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            return View(book);
        }

        public async Task<IActionResult> CheckoutBook(int id)
        {
            ApplicationUser user = await GetUser();
            Book? book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            if (book.CurrentApplicationUserId != null)
                return Forbid();

            book.CurrentApplicationUserId = user.Id;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ReturnBook(int id)
        {
            ApplicationUser user = await GetUser();
            Book? book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (book == null)
                return NotFound();

            if (book.CurrentApplicationUserId != user.Id)
                return Forbid();

            book.CurrentApplicationUserId = null;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
