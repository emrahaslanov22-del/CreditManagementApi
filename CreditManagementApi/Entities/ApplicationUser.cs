using Microsoft.AspNetCore.Identity;

namespace CreditManagementApi.Entities
{
    public class ApplicationUser:IdentityUser  
    {
        public  string Fullname { get; set; }

    }
}
