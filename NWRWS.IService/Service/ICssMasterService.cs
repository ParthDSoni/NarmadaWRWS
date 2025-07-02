using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface ICssMasterService : IDisposable
    {
        JsonResponseModel AddOrUpdate(CssMasterModel model, string username);
        List<CssMasterModel> CSSMasterSiteData();
        CssMasterModel GetFileByName(string strFileName);
        List<CssMasterModel> GetList(long lgLangId = 1);
        JsonResponseModel Delete(long id, string username);
        CssMasterModel Get(long id, long lgLangId = 1);
    }
}
