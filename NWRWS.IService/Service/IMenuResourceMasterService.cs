using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IMenuResourceMasterService :IDisposable
    {
        MenuResourceMasterModel Get(long id);

        List<MenuResourceMasterModel> GetList();

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(MenuResourceMasterModel model);

    }
}
