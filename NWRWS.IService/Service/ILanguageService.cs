using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface ILanguageService : IDisposable
    {
        List<LanguageMasterModel> GetList();
        JsonResponseModel Delete(long id, string username);
        LanguageMasterModel Get(long id);
        JsonResponseModel AddorUpdate(LanguageMasterModel model, string username);
        List<LanguageMasterModel> GetListById(long id);
    }
}
