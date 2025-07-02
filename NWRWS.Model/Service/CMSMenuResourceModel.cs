using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class CMSMenuResourceModel
    {
        public long Id { get; set; }
        public long? CMSMenuResId { get; set; }
        public long? LanguageId { get; set; }
        public string MenuName { get; set; }
        public string MenuURL { get; set; }
        public string? Tooltip { get; set; }
        public string? PageDescription { get; set; }
        public string? ResourceType { get; set; }
        public string? TemplateId { get; set; }
        public bool IsActive { get; set; }
        public bool IsRedirect { get; set; }
        public bool IsFullScreen { get; set; }
        public string? col_menu_type { get; set; }
        public long col_parent_id { get; set; }
        public long MenuRank { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ParentName { get; set; }
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? DeletedDate { get; set; }
    }
}

