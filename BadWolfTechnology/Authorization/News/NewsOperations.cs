using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.News
{
    public class NewsOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = NewsCreateOperationName };
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = NewsReadOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = NewsUpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = NewsDeleteOperationName };

        public static string NewsCreateOperationName => "News.Create";
        public static string NewsReadOperationName => "News.Read";
        public static string NewsUpdateOperationName => "News.Update";
        public static string NewsDeleteOperationName => "News.Delete";

        public static string NewsAdministratorRole => "Administrator";
        public static string NewsManagerRole => "NewsManager";
    }
}
