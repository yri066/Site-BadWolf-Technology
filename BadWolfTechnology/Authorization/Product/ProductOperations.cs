using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.Product
{
    public class ProductOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = ProductCreateOperationName };
        public static OperationAuthorizationRequirement ViewHidden =
            new OperationAuthorizationRequirement { Name = ProductViewHiddenOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = ProductUpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = ProductDeleteOperationName };

        public static string ProductCreateOperationName => "Product.Create";
        public static string ProductViewHiddenOperationName => "Product.ViewHidden";
        public static string ProductUpdateOperationName => "Product.Update";
        public static string ProductDeleteOperationName => "Product.Delete";

        public static string ProductAdministratorRole => "Administrator";
        public static string ProductManagerRole => "ProductManager";
    }
}
