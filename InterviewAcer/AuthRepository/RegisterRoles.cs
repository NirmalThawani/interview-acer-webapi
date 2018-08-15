using InterviewAcer.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace InterviewAcer.AuthRepository
{
    public static class RegisterRoles
    {
        private static UserManager<ApplicationUser> _userManager;
        private static RoleManager<IdentityRole> _roleManager;
        private static AuthContext.AuthContext _ctx;
        static RegisterRoles()
        {
            _ctx = new AuthContext.AuthContext();
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        public static void SeedRoles()
        {
            if (!_roleManager.RoleExists("Administrator"))
            {
                var roleresult = _roleManager.Create(new IdentityRole("Administrator"));
            }
            if (!_roleManager.RoleExists("Candidate"))
            {
                var roleresult = _roleManager.Create(new IdentityRole("Candidate"));
            }
            if (!_roleManager.RoleExists("UniversityAdministrator"))
            {
                var roleresult = _roleManager.Create(new IdentityRole("UniversityAdministrator"));
            }
            var adminUser = _userManager.FindByName("tejashri.godse@omni-bridge.net");
            if(adminUser == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "tejashri.godse@omni-bridge.net",
                    Email = "tejashri.godse@omni-bridge.net",
                    EmailConfirmed = true,
                    Name = "Tejashri Godse"
                };

                _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };

                IdentityResult userResult = _userManager.Create(user, "SuperPass");
                if (userResult.Succeeded)
                {
                    var result = _userManager.AddToRole(user.Id, "Administrator");
                }
            }
            var uniAdminUser = _userManager.FindByName("uniadmin@omni-bridge.net");
            if (uniAdminUser == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "uniadmin@omni-bridge.net",
                    Email = "uniadmin@omni-bridge.net",
                    EmailConfirmed = true,
                    Name = "Tejashri Godse"
                };

                _userManager.UserValidator = new UserValidator<ApplicationUser>(_userManager)
                {
                    AllowOnlyAlphanumericUserNames = false,
                    RequireUniqueEmail = true
                };

                IdentityResult userResult = _userManager.Create(user, "SuperPass");
                if (userResult.Succeeded)
                {
                    var result = _userManager.AddToRole(user.Id, "UniversityAdministrator");
                }
            }
        }
    }
}