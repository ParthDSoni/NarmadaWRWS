using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace NWRWS.Model.Service
{
	public class BasinFormModel
	{
		public long Id { get; set; }
		public long BasinId { get; set; }

		[Required(ErrorMessage = "Region is required.")]
		public long RegionId { get; set; }

		public int LanguageId { get; set; }

		
		public string Name { get; set; }

		[Required(ErrorMessage = "Image upload is required.")]
		public IFormFile? UploadDocument { get; set; }

		public string? UploadDocumentPath { get; set; }

		[Required(ErrorMessage = "Basin details are required.")]
		public string? Description { get; set; }

		public bool IsActive { get; set; }
	}
	public class BasinModel
    {
        public long Id { get; set; }
        public long BasinId { get; set; }
        public long RegionId { get; set; }
        public int LanguageId { get; set; }
        public string Name { get; set; }
        public string UploadDocumentPath { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
