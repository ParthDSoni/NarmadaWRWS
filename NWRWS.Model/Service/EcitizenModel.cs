using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class EcitizenModel
    {
        public long Id { get; set; }
        public long EcitizenId { get; set; }
        public long LanguageId { get; set; }
        public string EcitizenTypeId { get; set; }
        public string AdvertisementId { get; set; }
        public string EcitizenTypeName { get; set; }
        public string? EcitizenTitle { get; set; }
        public string? ShortDescription { get; set; }
        public string EcitizenDesc { get; set; }
        public string? GRNumber { get; set; }
        public long? Branch { get; set; }
        public string? Branch_name { get; set; }
        public DateTime? EcitizenStartDate { get; set; }
        public DateTime? EcitizenEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? NewsDate { get; set; }
        public string? MonthName { get; set; }
        public string? OnlyDate { get; set; }
        public string? blogdateconvert { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }
        public string? Location { get; set; }
        public string? NewsTypeText { get; set; }
        public bool IsActive { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        /*public List<EcitizenModel> EcitizenModels { get; set; }*/
    }
    public class Announcement
    {
        public List<EcitizenModel> lstNotifications { get; set; }
        public List<EcitizenModel> lstGR { get; set; }
    }
    public class branch
    {
        public long? Id { get; set; }
        public long? Branch_Id { get; set; }
        public string? Branch_name { get; set; }
    }
}
