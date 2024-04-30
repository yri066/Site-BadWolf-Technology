using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.Comment
{
    public static class CommentOperations
    {
        public static OperationAuthorizationRequirement Create =
            new OperationAuthorizationRequirement { Name = CommentCreateOperationName };
        public static OperationAuthorizationRequirement Delete =
            new OperationAuthorizationRequirement { Name = CommentDeleteOperationName };

        public static string CommentCreateOperationName => "CommentCreate";
        public static string CommentDeleteOperationName => "CommentDelete";

        public static string CommentAdministratorRole => "Administrator";
        public static string CommentManagerRole => "CommentManager";
    }
}
