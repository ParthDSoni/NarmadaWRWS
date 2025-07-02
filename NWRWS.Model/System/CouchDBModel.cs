using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.System
{
    public class CouchDBModel
    {
        public string CouchDBURL { get; set; }
        public string CouchDBDbName { get; set; }
        public string CouchDBUser { get; set; }
        public string AllowCouchDBStore { get; set; }
    }
}
