using BadWolfTechnology.Areas.Identity.Data;
using BadWolfTechnology.Authorization.Comment;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace BadWolfTechnology
{
    public class CommentUserAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Comment>
    {
        UserManager<ApplicationUser> _userManager;

        public CommentUserAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Comment resource)
        {
            if (context.User == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name == CommentOperations.CommentCreateOperationName)
            {
                context.Succeed(requirement);
            }

            if(resource.User.Id == _userManager.GetUserId(context.User))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
