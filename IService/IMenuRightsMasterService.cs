using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.IService
{
    public interface IMenuRightsMasterService : IDisposable
    {
        List<MenuRightsMasterModel> GetList();

        List<MenuRightsMasterModel> GetListByRoleId(long id);

        bool DeleteByRole(long id, out string strMessage);

        bool Insert(List<MenuRightsMasterModel> model, long lgRoleId, string strUsername, out string strMessage);
    }
}
