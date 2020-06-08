using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Areas.Identity.Data
{
	public class AdditionalUserClaimsPrincipalFactory
		: UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
	{
		public AdditionalUserClaimsPrincipalFactory(
			UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager,
			IOptions<IdentityOptions> optionsAccessor)
			: base(userManager, roleManager, optionsAccessor)
		{ }

		public async override Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
		{
			var principal = await base.CreateAsync(user);
			//var identity = (ClaimsIdentity)principal.Identity;

			//var claims = new List<Claim>();
			//if (user.IsAdmin)
			//{
			//	claims.Add(new Claim(JwtClaimTypes.Role, "admin"));
			//}
			//else
			//{
			//	claims.Add(new Claim(JwtClaimTypes.Role, "user"));
			//}

			//identity.AddClaims(claims);
			return principal;
		}
	}
}
