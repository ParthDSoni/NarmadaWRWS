using NWRWS.Model.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IGlobleSerchService:IDisposable
    {
        List<GlobleSerchModel> GetList(string SearchText, long lgLangId);
    }
}
