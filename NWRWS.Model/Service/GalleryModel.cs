using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    public class GalleryImagesModel
    {
        public long Id { get; set; }
        public long gallerymasterId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }

        public string PlaceName { get; set; }
    }
    public class GalleryModel
    {
        public long Id { get; set; }

        public string PlaceName { get; set; }

        public string? ThumbImageName { get; set; }
        public string? ThumbImagePath { get; set; }
        public bool  CMSection { get; set; }
        public DateTime? GalleryDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsVideo { get; set; }
        public long? DepartmentId { get; set; }

        public List<GalleryImagesModel> lstGalleryImagesModels { get; set; }
    }
    public class EventTypeModel
    {
        public long Id { get; set; }
        public string PlaceName { get; set; }       
    }

    public class AlbumModel
    {
        public long Id { get; set; }
        public string PlaceName { get; set; }
        public string FirstImagePath { get; set; }
        public DateTime? GalleryDate { get; set; }
        public long? DepartmentId { get; set; }


    }

    public class VideoAlbumModel
    {
        public long Id { get; set; }
        public string PlaceName { get; set; }
        public string? ThumbImageName { get; set; }
        public string? ThumbImagePath { get; set; }
        public string FirstImagePath { get; set; }
        public bool IsVideo { get; set; }

    }

    public class GalleryAllDetails
    {
        public List<GalleryModel> galleryDetails = new List<GalleryModel>();
        public int recordsTotal { get; set; }
        public int CurrentPage { get; set; }
        public double PageCount { get; set; }
    }
}
