using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IAnnouncementMasterService : IDisposable
    {
        JsonResponseModel AddOrUpdate(AnnouncementModel model, string username);
        JsonResponseModel Delete(long id, string username);
        AnnouncementModel Get(long id, long lgLangId = 1);
        List<AnnouncementModel> GetList(long lgLangId = 1);
        List<AnnouncementModel> GetListFront(long lgLangId = 1);
        AnnouncementModel GetMenuRes(long AnnouncementId, long lgLangId = 1);
    }
}