using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class DistrictMasterModel
    {
        public long DistId { get; set; }
        public string DistName { get; set; }
        public long Id { get; set; }
        public long LanguageId { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}
