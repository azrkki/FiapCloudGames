using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace FCG.Api.Attributes
{
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if user is authenticated
            if (context.HttpContext.User.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Authentication required" });
                return;
            }

            // Get user role from claims
            var userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userRole))
            {
                context.Result = new ObjectResult(new { message = "User role not found. Access denied." })
                {
                    StatusCode = 403
                };
                return;
            }

            // Check if user has required role
            if (_roles != null && _roles.Length > 0 && !_roles.Contains(userRole))
            {
                context.Result = new ObjectResult(new { message = $"User is not allowed to access this endpoint. Required role(s): {string.Join(", ", _roles)}. Current role: {userRole}" })
                {
                    StatusCode = 403
                };
                return;
            }
        }
    }

    // Convenience attributes for specific roles
    public class AuthorizeAdminAttribute : AuthorizeRoleAttribute
    {
        public AuthorizeAdminAttribute() : base("Administrator") { }
    }

    public class AuthorizeCommonAttribute : AuthorizeRoleAttribute
    {
        public AuthorizeCommonAttribute() : base("Common", "Administrator") { }
    }
}