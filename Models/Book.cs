namespace IntroToIdentity.Models
{
    public class Book
    {
        // Self props
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        // FK props
        public string? CurrentApplicationUserId { get; set; }
        public int AuthorId { get; set; }

        // Nav props
        public virtual ApplicationUser? CurrentApplicationUser { get; set; }
        public virtual Author Author { get; set; }
    }
}
