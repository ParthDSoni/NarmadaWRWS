using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class CategoryModel
    {
        public long Id { get; set; }
        public long LanguageId { get; set; }
        public long CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public long SubCategoryID { get; set; }
        public string? SubCategoryName { get; set; }
        public string? Title { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public bool IsActive { get; set; }
        public bool IsStoreDB { get; set; } = false;
    }
}
