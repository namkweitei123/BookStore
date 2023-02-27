
using Microsoft.AspNetCore.Identity;

namespace BookStore.Models
{
    public class AppUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Address { get; set; }
        public byte[]? ProfilePicture { get; set; }

     
    }
}
