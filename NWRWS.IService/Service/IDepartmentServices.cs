using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IDepartmentServices : IDisposable
    {
        DepartmentMasterModel Get(long id, long lgLangId = 1);

        List<DepartmentMasterModel> GetList(long lgLangId = 1);

        List<DepartmentMasterModel> GetListF();

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(DepartmentMasterModel model, string username);

    }
}
