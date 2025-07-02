using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IDistrictMaster : IDisposable
    {
        DistrictMasterModel Get(long id, long lgLangId = 1);

        List<DistrictMasterModel> GetList(long lgLangId = 1);

        List<DistrictMasterModel> GetListF();

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(DistrictMasterModel model, string username);

    }
}
