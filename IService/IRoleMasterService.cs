using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.IService
{
    public interface IRoleMasterService : IDisposable
    {
        RoleMasterModel Get(long id);

        List<RoleMasterModel> GetList();

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(RoleMasterModel model);
    }
}
