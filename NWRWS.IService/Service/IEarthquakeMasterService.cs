using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IEarthquakeMasterService : IDisposable
    {
        JsonResponseModel AddOrUpdate(EarthquakeMasterModel model, string username);
        JsonResponseModel Delete(long id, string username);
        EarthquakeMasterModel Get(long lgid, long lgLangId = 1);
        List<EarthquakeMasterModel> GetList(long lgLangId = 1);
        List<EarthquakeMasterModel> GetListFront(long lgLangId = 1);
    }
}
