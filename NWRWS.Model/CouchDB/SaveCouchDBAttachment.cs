using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NWRWS.Model.CouchDB
{
    public class SaveCouchDBAttachment
    {
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string Rev { get; set; }
        [JsonIgnore]
        public IFormFile File { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        [JsonIgnore]
        public byte[] AttachmentData { get; set; }
    }
}
