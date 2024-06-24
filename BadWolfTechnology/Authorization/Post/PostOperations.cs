using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.Post
{
    public class PostOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = PostCreateOperationName };
        public static OperationAuthorizationRequirement ViewHidden =
            new OperationAuthorizationRequirement { Name = PostViewHiddenOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = PostUpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = PostDeleteOperationName };

        public static string PostCreateOperationName => "Product.Create";
        public static string PostViewHiddenOperationName => "Product.ViewHidden";
        public static string PostUpdateOperationName => "Product.Update";
        public static string PostDeleteOperationName => "Product.Delete";

        public static string PostAdministratorRole => "Administrator";
    }
}
