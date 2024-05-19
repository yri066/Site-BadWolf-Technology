using BadWolfTechnology.Authorization.Product;
using BadWolfTechnology.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology
{
    public class ProductAdministratorAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Product resource)
        {
            if(context.User == null)
            {
                return Task.CompletedTask;
            }

            if(context.User.IsInRole(ProductOperations.ProductAdministratorRole))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
