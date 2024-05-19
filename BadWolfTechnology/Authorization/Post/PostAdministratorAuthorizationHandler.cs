using BadWolfTechnology.Authorization.Post;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology
{
    public class PostAdministratorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Post>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Post resource)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(PostOperations.PostAdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
