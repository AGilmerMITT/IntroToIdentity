using Microsoft.AspNetCore.Identity;

namespace IntroToIdentity.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Book> CheckedOutBooks { get; set; }
    }
}
