using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;
using System.Security.Claims;

namespace SisOdonto.App.Extensions
{
    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        #region Constructors

        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequirementClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }

        #endregion Constructors
    }

    public class CustomAuthorization
    {
        #region Methods

        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity is {IsAuthenticated: true} && (context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue)) || context.User.Claims.Any(c => c.Type == "Admin"));
        }

        #endregion Methods
    }
    public class RequirementClaimFilter : IAuthorizationFilter
    {
        #region Fields

        private readonly Claim _claim;

        #endregion Fields

        #region Constructors

        public RequirementClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        #endregion Constructors

        #region Methods

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.Identity != null && context.HttpContext.User.Identity.IsAuthenticated is false)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Identity", page = "/Account/Login", ReturnUrl = context.HttpContext.Request.Path.ToString() }));
                return;
            }

            if (CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, _claim.Type, _claim.Value) is false)
            {
                context.Result = new StatusCodeResult(403);
            }
        }

        #endregion Methods
    }
}