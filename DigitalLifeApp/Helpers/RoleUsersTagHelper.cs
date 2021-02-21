using DigitalLifeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigitalLifeApp.Helpers
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper: TagHelper
    {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManger;

        public RoleUsersTagHelper(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManger)
        {
            userManager = _userManager;
            roleManger = _roleManger;
        }

        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();

            var role = await roleManger.FindByIdAsync(Role);

            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }

            output.Content.SetContent(names.Count == 0 ? "No User" :
                string.Join(", ", names));
        }
    }
}
