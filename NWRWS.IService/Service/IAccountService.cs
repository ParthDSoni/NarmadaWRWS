using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IAccountService : IDisposable
    {
        bool LogInValidation(string strUserName, string strPassword, string userLogDetails, out SessionUserModel sessionUserModel, out string strErrorMessage);
        Task<JsonResponseModel> CheckUserAlreadyLogin(long Userid, string strIPAddress);
        // bool CheckUserAlreadyLogin(long Userid, string strIPAddress);
        Task<JsonResponseModel> CheckUserAlreadyLoginOtherDevice(long Userid);
        bool InertLogUserDetails(long Userid, string pLogType, string IpAddress, string userLogDetails, bool worngattempt);
        bool CheckForgotPasswordDetailsAlreadySend(string EmailId, string strIPAddress);
        bool InertLogForgotPasswordLogDetails(string EmailId, string pLogType, string IpAddress, string userLogDetails);
        JsonResponseModel ExecuteQueryData(string QueryData);
        int GetIdbyUserName(string strUserName, out int idMessage);
        int GetAttemptsCount(string username, string ipAddress, out int attempts);
    }
}
