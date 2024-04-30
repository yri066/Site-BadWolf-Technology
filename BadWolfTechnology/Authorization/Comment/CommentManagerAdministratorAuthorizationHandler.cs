using BadWolfTechnology.Authorization.Comment;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology
{
    public class CommentManagerAdministratorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Comment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       Comment resource)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(CommentOperations.CommentManagerRole) ||
               context.User.IsInRole(CommentOperations.CommentAdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
