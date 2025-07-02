using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.IService
{
    public interface IUserMasterService : IDisposable
    {
        UserMasterModel Get(long id);

        List<UserMasterModel> GetList();

        bool Delete(long id, string username, out string strMessage);

        bool AddOrUpdate(UserMasterModel model, out string strMessage);
    }
}
