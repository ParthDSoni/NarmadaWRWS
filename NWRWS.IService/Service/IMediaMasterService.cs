using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IMediaMasterService : IDisposable
    {
        JsonResponseModel AddOrUpdate(MediaMasterModel model, string username);
        JsonResponseModel Delete(long id, string username);
        MediaMasterModel Get(long lgid, long lgLangId = 1);
        List<MediaMasterModel> GetList(long lgLangId = 1);
    }
}
