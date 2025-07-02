using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.System
{
    public class PageRightsModel
    {
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }
    }
}
