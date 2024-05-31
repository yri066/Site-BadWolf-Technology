using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Authorization.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BadWolfTechnology
{
    public class AdminAdministratorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, ApplicationUser>
    {
        UserManager<ApplicationUser> _userManager;

        public AdminAdministratorAuthorizationHandler(UserManager<ApplicationUser> userManager)
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

            // Не может забанить администратора.
            if (requirement.Name == AdminOperations.AdminBanOperationName &&
                _userManager.IsInRoleAsync(resource, AdminOperations.AdminAdministratorRole).Result)
            {
                return Task.CompletedTask;
            }

            // Не может взаимодействовать с супер администратором.
            if (_userManager.IsInRoleAsync(resource, AdminOperations.AdminSuperAdministratorRole).Result)
            {
                return Task.CompletedTask;
            }

            // Не может изменять роли.
            if (requirement.Name == AdminOperations.AdminUpdateRoleOperationName)
            {
                return Task.CompletedTask;
            }

            if (context.User.IsInRole(AdminOperations.AdminAdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
