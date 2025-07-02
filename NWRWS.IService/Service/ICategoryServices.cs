using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public  interface ICategoryServices :IDisposable
    {
        CategoryModel Get(long id, long lgLangId = 1);
        List<CategoryModel> GetList(long lgLangId = 1); 

        CategoryModel GetSubCategory(long id, long lgLangId = 1);
        List<CategoryModel> GetSubList(long lgLangId = 1);


        CategoryModel GetPRMaster(long id, long lgLangId = 1);
        List<CategoryModel> GetPRMasterList(long lgLangId = 1);
        JsonResponseModel AddOrUpdatePRMaster(CategoryModel model, string username);
        JsonResponseModel Delete(long id, string username);
    }
}
