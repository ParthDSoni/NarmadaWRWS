using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class BlogMasterModel
    {
        public int Id { get; set; }
        public int BlogMasterId { get; set; }
        public int LanguageId { get; set; }
        public string? BlogName { get; set; }
        public string? Allimages { get; set; }
        public string? Description { get; set; }
        public string? BlogBy { get; set; }
        public DateTime? BlogDate { get; set; }
        public string BlogDateconvert { get; set; }
        public string? FileUpload { get; set; }
        public string? FilePath { get; set; }
        public string? FirstImagePath { get; set; }
        public bool IsActive { get; set; }
        public string? Location { get; set; }
        public bool IsDelete { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? DeleteBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public string? Username { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }

        public int TypeId { get; set; }
        public string? TypeName { get; set; }

    }
}
