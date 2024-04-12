namespace IntroToIdentity.Models
{
    public class Author
    {
        // self
        public int Id { get; set; }
        public string Name { get; set; }

        // fk

        // nav
        public virtual ICollection<Book> Books { get; set; }
    }
}
