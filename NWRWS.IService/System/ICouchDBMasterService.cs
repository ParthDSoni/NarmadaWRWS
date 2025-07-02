using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.System
{
    public interface ICouchDBMasterService : IDisposable
    {
        CouchDBModel Get();

        bool InsertOrUpdate(CouchDBModel objSMTPModel, out string strErrorMessage);
    }
}
