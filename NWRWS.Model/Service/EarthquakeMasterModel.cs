using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class EarthquakeMasterModel
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }
        public long EarthquakeId { get; set; }
        public float Magnitude { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Depth { get; set; }
        public string Link { get; set; }
        public string Date { get; set; }
        public string? blogdateconvert { get; set; }
        //public DateTime Time { get; set; }
        public string Location { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
