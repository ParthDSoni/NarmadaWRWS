using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IEcitizenMasterService : IDisposable
    {
        JsonResponseModel AddOrUpdate(EcitizenModel model, string username);
        JsonResponseModel Delete(long id, string username);
        EcitizenModel Get(long id, long lgLangId = 1);

        EcitizenModel GetMenuRes(long NewsId, long lgLangId = 1);
        List<EcitizenModel> GetList(long lgLangId = 1);
        List<branch> GetListGRBranchFront(long lgLangId = 1);
        List<EcitizenModel> GetListFront(long lgLangId = 1);
        List<EcitizenModel> GetLatestUpdatesListFront(long lgLangId = 1);
    }
}