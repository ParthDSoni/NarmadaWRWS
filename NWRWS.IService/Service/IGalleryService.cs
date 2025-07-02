using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.IService.Service
{
    public interface IGalleryService : IDisposable
    {
        GalleryModel Get(long id);

        List<GalleryModel> GetList();

        JsonResponseModel AddOrUpdate(GalleryModel model, string username);

        JsonResponseModel Delete(long id, string username);

        List<AlbumModel> GetAlbum();
        List<AlbumModel> GetCMAlbum();

        List<GalleryImagesModel> GetAlbumImages(long id);

        List<VideoAlbumModel> GetAllvideoalbum();
    }
}
