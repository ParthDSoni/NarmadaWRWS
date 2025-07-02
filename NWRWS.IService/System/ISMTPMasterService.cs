using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.System
{
    public interface ISMTPMasterService
    {
        SMTPModel Get();
        bool InsertOrUpdate(SMTPModel objSMTPModel, out string strErrorMessage);
        bool UpdateSMTPEnvironment(string strSMTPIsTest, out string strErrorMessage);
    }
}
