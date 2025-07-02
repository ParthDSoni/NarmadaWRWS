using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IBasinMasterService
    {
        List<BasinModel> GetList(int languageId = 1);
        List<BasinModel> GetListF(int languageId = 1, long regionId = 1);
        BasinModel Get(long id, int languageId);
        JsonResponseModel AddOrUpdate(BasinModel objModel, long userId);
        JsonResponseModel Delete(long id, long userId);
    }
}
