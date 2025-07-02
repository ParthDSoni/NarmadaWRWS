using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class AnnouncementModel
    {
        public long Id { get; set; }
        public long AnnouncementId { get; set; }
        public long LanguageId { get; set; }
        public string? AnnouncementTitle { get; set; }
        public string? ShortDescription { get; set; }
        public string AnnouncementDesc { get; set; }
        public DateTime? AnnouncementStartDate { get; set; }
        public DateTime? AnnouncementEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? AnnouncementDate { get; set; }
        public string? MonthName { get; set; }
        public string? OnlyDate { get; set; }
        public string? blogdateconvert { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public bool IsLink { get; set; }
    }
}

