using IntroToIdentity.Data;
using IntroToIdentity.Models;
using IntroToIdentity.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IntroToIdentity.Controllers
{
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

        public IActionResult Index()
        {
            var books = _context.Books
                .Include(b => b.Author)
                .ToList();

            ApplicationUser? user = GetUser();

            BooksIndexViewModel vm = new()
            {
                Books = books,
                UserId = user?.Id ?? string.Empty
            };

            return View(vm);
        }

        private ApplicationUser? GetUser()
        {
            string? userId = _userManager.GetUserId(User);
            return _context.Users.FirstOrDefault(u => u.Id == userId);
        }

        public IActionResult Details(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            return View(book);
        }

        public IActionResult CheckoutBook(int id)
        {
            ApplicationUser? user = GetUser();
            Book? book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (user == null)
                return Forbid();

            if (book == null)
                return NotFound();

            if (book.CurrentApplicationUserId != null)
                return Forbid();

            book.CurrentApplicationUserId = user.Id;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ReturnBook(int id)
        {
            ApplicationUser? user = GetUser();
            Book? book = _context.Books.FirstOrDefault(b => b.Id == id);

            if (user == null)
                return Forbid();

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
