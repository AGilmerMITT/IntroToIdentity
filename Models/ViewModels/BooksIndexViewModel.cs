namespace IntroToIdentity.Models.ViewModels
{
    public class BooksIndexViewModel
    {
        public string UserId { get; init; }
        public IEnumerable<Book> Books { get; init; }
    }
}
