using Microsoft.AspNet.Identity.EntityFramework;

namespace InterviewAcer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string LicenseKey { get; set; }
    }
}