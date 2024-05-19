using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.Post
{
    public class PostOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = PostCreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = PostReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = PostUpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = PostDeleteOperationName };

        public static string PostCreateOperationName => "Product.Create";
        public static string PostReadOperationName => "Product.Read";
        public static string PostUpdateOperationName => "Product.Update";
        public static string PostDeleteOperationName => "Product.Delete";

        public static string PostAdministratorRole => "Administrator";
    }
}
