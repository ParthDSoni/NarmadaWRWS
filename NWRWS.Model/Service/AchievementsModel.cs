using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.Service
{
    //public class AchievementsModel
    //{
    //    public long Id { get; set; }
    //    public string? ThumbImageName { get; set; }
    //    public string? ThumbImagePath { get; set; }
    //    public bool IsActive { get; set; }       

    //    public List<AchievementsImagesModel> lstAchievementsImagesModels { get; set; }
    //}
    //public class AchievementsImagesModel
    //{
    //    public long Id { get; set; }
    //    public long BlogMasterId { get; set; }
    //    public string ImageName { get; set; }
    //    public string ImagePath { get; set; }
    //    public int? LanguageId { get; set; }
    //}
    //public class EventTypeModel
    //{
    //    public long Id { get; set; }
    //    public string PlaceName { get; set; }
    //}

    //public class AlbumModel
    //{
    //    public long Id { get; set; }
    //    public string PlaceName { get; set; }
    //    public string FirstImagePath { get; set; }

    //}

    //public class VideoAlbumModel
    //{
    //    public long Id { get; set; }
    //    public string PlaceName { get; set; }
    //    public string? ThumbImageName { get; set; }
    //    public string? ThumbImagePath { get; set; }
    //    public string FirstImagePath { get; set; }
    //    public bool IsVideo { get; set; }

    //}


    public class EventAchievementsImageModel
    {
        public long RowIndex { get; set; }
        public string Command { get; set; }
        public List<IFormFile>? Image { get; set; }
        public string? ImageName { get; set; }
        public string? ImagePath { get; set; }

        public string? LanguageId { get; set; }

    }
}

