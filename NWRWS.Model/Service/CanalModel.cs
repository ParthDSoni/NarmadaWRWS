using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NWRWS.Model.Service
{
   public class CanalFormModel
    {
        public long Id { get; set; }
        public long CanalId { get; set; }
        public long BasinId { get; set; }
        public long RegionId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public IFormFile? UploadDocument { get; set; }
        public string? UploadDocumentPath { get; set; }
        public string? ImageNamehash { get; set; }

        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
    public class CanalModel
    {
        public long Id { get; set; }
        public long CanalId { get; set; }
        public long BasinId { get; set; }
        public long RegionId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string UploadDocumentPath { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
