using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface ICMSMenuResourceMasterService : IDisposable
    {
        CMSMenuResourceModel GetMenuRes(long menuResid, long lgLangId = 1);

        CMSMenuResourceModel Get(long id, long lgLangId = 1);

        List<CMSMenuResourceModel> GetList(long lgLangId = 1);
        List<CMSMenuResourceModel> GetListFront(long lgLangId = 1);
        List<CMSMenuResourceModel> GetListMaster();
        List<CMSMenuResourceModel> GetParentList(long? lgLangId = 1);
        //JsonResponseModel SwapSequance(long rank, string dir, string username);
        JsonResponseModel SwapSequance(long rank, string dir, string username, string type, long parentid);

        List<LanguageMasterModel> GetListLanguage();
        

        // List<ExamTypeMasterModel> GetExamType();

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(CMSMenuResourceModel model, string username);
        JsonResponseModel UpdateSwap(CMSMenuResourceModel model, string username);
        CMSMenuResourceModel PageDeails(long Id);

        CMSMenuResourceModel UpdateSiteDate();
    }
}