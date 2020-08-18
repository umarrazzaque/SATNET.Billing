using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Areas.Identity.Data
{
    public class MyUserClaimsPrincipalFactory //: UserClaimsPrincipalFactory<ApplicationUser>
    {
        //public MyUserClaimsPrincipalFactory(
        //UserManager<ApplicationUser> userManager,
        //IOptions<IdentityOptions> optionsAccessor)
        //    : base(userManager, optionsAccessor)
        //{
        //}
        //protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        //{
        //    var identity = await base.GenerateClaimsAsync(user);
        //    identity.AddClaim(new Claim("CustomerId", user.CustomerId.ToString()));
        //    return identity;
        //}
    }
}
