using Microsoft.AspNetCore.Identity;

namespace BadWolfTechnology.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; } = string.Empty;

        [PersonalData]
        public string LastName { get; set; } = string.Empty;

        [PersonalData]
        public DateTime BirthDate { get; set; }
    }
}
