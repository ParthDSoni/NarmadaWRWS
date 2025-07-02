using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IFeedbackServices : IDisposable
    {
        JsonResponseModel AddFeedback(Feedback model);

        List<Feedback> GetList();
    }
}
