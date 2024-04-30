using BadWolfTechnology.Authorization.News;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology
{
    public class NewsManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, News>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       OperationAuthorizationRequirement requirement,
                                                       News resource)
        {
            if (context.User == null || resource == null)
            {
                return Task.CompletedTask;
            }

            if (requirement.Name == NewsOperations.NewsDeleteOperationName)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(NewsOperations.NewsManagerRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
