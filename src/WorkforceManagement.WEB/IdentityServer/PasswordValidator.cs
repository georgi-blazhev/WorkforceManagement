using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkforceManagement.BLL.IServices;
using WorkforceManagement.DAL.Entities;

namespace WorkforceManagement.WEB
{
    [ExcludeFromCodeCoverage]
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserManager _userManager;

        public PasswordValidator(IUserManager userManager)
        {
            _userManager = userManager;
        }

        //This method validates the user credentials and if successful teh IdentiryServer will build a token from the context.Result object
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            User user = await _userManager.FindByNameAsync(context.UserName);

            if (user != null)
            {
                bool authResult = await _userManager.ValidateUserCredentialsAsync(context.UserName, context.Password);
                if (authResult)
                {
                    List<string> roles = await _userManager.GetUserRolesAsync(user);

                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName));

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    context.Result = new GrantValidationResult(subject: user.Id, authenticationMethod: "password", claims: claims);
                }
                else
                {
                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
                }

                return;
            }
            context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
        }
    }
}
