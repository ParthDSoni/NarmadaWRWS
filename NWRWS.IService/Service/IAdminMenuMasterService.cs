using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IAdminMenuMasterService : IDisposable
    {
        AdminMenuMasterModel Get(long id);

        List<AdminMenuMasterModel> GetList();

        JsonResponseModel SwapSequance(long rank, string dir, string username);

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(AdminMenuMasterModel model);
    }
}
