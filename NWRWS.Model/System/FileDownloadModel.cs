using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.System
{
    public class FileDownloadModel
    {
        public string Filename { get; set; }
        public string FileExtension { get; set; }
        public byte[] DataBytes { get; set; }
    }
}
