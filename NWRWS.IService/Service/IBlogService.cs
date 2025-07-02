using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IBlogService : IDisposable
    {
        JsonResponseModel AddOrUpdate(BlogMasterModel model, List<EventAchievementsImageModel> lstData);
        JsonResponseModel Delete(long id, string username);
        BlogMasterModel Get(long id, long lgLangId);
        BlogMasterModel GetMenuRes(long id, long lgLangId);
        List<BlogMasterModel> GetList(long lgLangId = 1);
        List<EventAchievementsImageModel> GetAchievementsImages(long lgLangId, long id);

        // List<EventAchievementsImageModel> GetAllFrontImagesById(long blogMasterId, long lgLangId);

    }
}