using BadWolfTechnology.Authorization.Product;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology
{
    public class ProductManagerAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Product resource)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }

            if(requirement == ProductOperations.Delete)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(ProductOperations.ProductManagerRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
