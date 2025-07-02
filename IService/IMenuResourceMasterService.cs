using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.IService
{
    public interface IMenuResourceMasterService :IDisposable
    {
        MenuResourceMasterModel Get(long id);

        List<MenuResourceMasterModel> GetList();

        bool Delete(long id, string username, out string strMessage);

        bool AddOrUpdate(MenuResourceMasterModel model, out string strMessage);

    }
}
