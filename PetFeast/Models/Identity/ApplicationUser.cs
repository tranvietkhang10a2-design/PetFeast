using Microsoft.AspNetCore.Identity;

namespace PetFeast.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
