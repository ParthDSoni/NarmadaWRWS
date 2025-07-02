using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IJsMasterService : IDisposable
    {
        JSMasterModel Get(long id, long lgLangId = 1);
        List<JSMasterModel> GetList(long lgLangId = 1);
        JsonResponseModel AddOrUpdate(JSMasterModel model, string username);
        JsonResponseModel Delete(long id, string username);
    }
}
