using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.IService
{
    public interface IAccountService : IDisposable
    {
        bool LogInValidation(string strUserName, string strPassword, string userLogDetails, out SessionUserModel sessionUserModel, out string strErrorMessage);
    }
}
