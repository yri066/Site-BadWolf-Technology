using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace BadWolfTechnology.Authorization.Admin
{
    public static class AdminOperations
    {
        public static OperationAuthorizationRequirement Read =
            new OperationAuthorizationRequirement { Name = AdminReadOperationName};
        public static OperationAuthorizationRequirement UpdateRole =
            new OperationAuthorizationRequirement { Name = AdminUpdateRoleOperationName };
        public static OperationAuthorizationRequirement Ban =
            new OperationAuthorizationRequirement { Name = AdminBanOperationName };
        public static string AdminReadOperationName => "Admin.Read";
        public static string AdminUpdateRoleOperationName => "Admin.UpdateRole";
        public static string AdminBanOperationName => "Admin.Ban";

        public static string AdminSuperAdministratorRole => "SuperAdministrator";
        public static string AdminAdministratorRole => "Administrator";
    }
}
