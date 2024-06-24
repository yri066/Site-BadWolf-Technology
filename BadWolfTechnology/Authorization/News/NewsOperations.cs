using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.News
{
    public class NewsOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = NewsCreateOperationName };
        public static OperationAuthorizationRequirement ViewHidden =
            new OperationAuthorizationRequirement { Name = NewsViewHiddenOperationName };
        public static OperationAuthorizationRequirement Update =
            new OperationAuthorizationRequirement { Name = NewsUpdateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = NewsDeleteOperationName };

        public static string NewsCreateOperationName => "News.Create";
        public static string NewsViewHiddenOperationName => "News.ViewHidden";
        public static string NewsUpdateOperationName => "News.Update";
        public static string NewsDeleteOperationName => "News.Delete";

        public static string NewsAdministratorRole => "Administrator";
        public static string NewsManagerRole => "NewsManager";
    }
}
