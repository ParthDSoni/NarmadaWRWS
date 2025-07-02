using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.CouchDB
{

    public class CouchResponseModel
    {
        public bool ok { get; set; }
        public string id { get; set; }
        public string rev { get; set; }
    }
    public class CoutchDBFileManagementResponse
    {
        public string Id { get; set; }
        public string RevId { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsCouchDBSave { get; set; }
        public string Filename { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public dynamic Result { get; set; }
    }
}
