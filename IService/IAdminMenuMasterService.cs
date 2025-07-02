using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.IService
{
    public interface IAdminMenuMasterService : IDisposable
    {
        AdminMenuMasterModel Get(long id);

        List<AdminMenuMasterModel> GetList();

        bool Delete(long id, string username, out string strMessage);

        bool AddOrUpdate(AdminMenuMasterModel model, out string strMessage);
    }
}
