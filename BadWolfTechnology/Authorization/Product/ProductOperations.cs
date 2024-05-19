using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.Product
{
    public class ProductOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = ProductCreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = ProductReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = ProductUpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = ProductDeleteOperationName };

        public static string ProductCreateOperationName => "Product.Create";
        public static string ProductReadOperationName => "Product.Read";
        public static string ProductUpdateOperationName => "Product.Update";
        public static string ProductDeleteOperationName => "Product.Delete";

        public static string ProductAdministratorRole => "Administrator";
        public static string ProductManagerRole => "ProductManager";
    }
}
