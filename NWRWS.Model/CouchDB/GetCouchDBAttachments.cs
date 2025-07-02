using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.CouchDB
{
    public class GetCouchDBAttachments
    {
        [JsonProperty("Docs")]
        public List<AttachmentInfo> Docs { get; set; }
    }
    public class AttachmentInfo
    {
        [JsonProperty("_id")]
        public string Id { get; set; }
        [JsonProperty("_rev")]
        public string Rev { get; set; }
        public string FileExtension { get; set; }
        public string FileName { get; set; }
    }
}
