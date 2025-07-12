using LMSWebAppClean.Domain.Base;

namespace LMSWebAppClean.Application.Interface
{
    public interface IPermissionChecker
    {
        // Check methods that throw UnauthorizedAccessException
        void Check(int requestUserId, int targetUserId, string selfPermission, string processPermission);
        void Check(BaseUser requestUser, BaseUser targetUser, string selfPermission, string processPermission);
        
        // Simple permission check methods for single permissions
        void Check(int userId, string permission, string errorMessage);
        void Check(BaseUser user, string permission, string errorMessage);
        
        // CheckIf methods that return boolean
        bool CheckIf(int requestUserId, int targetUserId, string selfPermission, string processPermission);
        bool CheckIf(BaseUser requestUser, BaseUser targetUser, string selfPermission, string processPermission);
        
        // Simple CheckIf methods that return boolean  
        bool CheckIf(int userId, string permission);
        bool CheckIf(BaseUser user, string permission);
    }
}
