using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IDocumentServices : IDisposable
    {
        DocumentModel Get(long Id, long lgLangId = 1);

        List<DocumentModel> GetList(long lgLangId = 1);

        JsonResponseModel Delete(long Doc_Id, string username);

        JsonResponseModel AddOrUpdate(DocumentModel model, string username);
    }
}
