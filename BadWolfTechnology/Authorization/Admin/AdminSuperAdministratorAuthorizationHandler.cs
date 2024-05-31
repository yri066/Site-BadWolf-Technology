using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Authorization.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BadWolfTechnology
{
    public class AdminSuperAdministratorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ApplicationUser>
    {
        UserManager<ApplicationUser> _userManager;

        public AdminSuperAdministratorAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       ApplicationUser resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            // Не может забанить сам себя.
            if (requirement.Name == AdminOperations.AdminBanOperationName &&
                resource.Id == _userManager.GetUserId(context.User))
            {
                return Task.CompletedTask;
            }

            // Не может забанить супер администратора
            if (requirement.Name == AdminOperations.AdminBanOperationName &&
                context.User.IsInRole(AdminOperations.AdminSuperAdministratorRole))
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(AdminOperations.AdminSuperAdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
