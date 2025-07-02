using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class BlogListMasterModel
    {
        public List<BlogMasterModel> BlogListMasterModels { get; set; } 
        public int PageCount { get; set; }
        public int CurrentPageIASList { get; set; }


    }
}
