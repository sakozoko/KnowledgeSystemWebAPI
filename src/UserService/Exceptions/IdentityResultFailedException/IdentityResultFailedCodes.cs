using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Exceptions.IdentityResultFailedException
{
    public static class IdentityResultFailedCodes
    {
        public const string UserNotFound = "UserNotFound";
        public const string RoleNotFound = "RoleNotFound";
        public const string BadUserModel = "BadUserModel";
        public const string AccessDenied = "AccessDenied";

    }
}