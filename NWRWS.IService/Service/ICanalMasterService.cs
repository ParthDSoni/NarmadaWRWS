using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface ICanalMasterService
    {
        JsonResponseModel AddOrUpdateCanal(CanalModel model, long userId);
        JsonResponseModel DeleteCanal(long id, long userId);
        CanalModel GetCanal(long id, int languageId = 1);
        List<CanalModel> GetList(int languageId = 1, long regionId = 0);
        List<CanalModel> GetCanalListF(int languageId = 1, long regionId = 0, long canalId = 0);
    }
}