using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace NWRWS.Services.Service
{
    public class GalleryService : IGalleryService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public GalleryService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public GalleryModel Get(long id)
        {
            try
            {
                var dataModel = GetList().Where(x => x.Id == id).FirstOrDefault();

                if (dataModel != null)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("pGalleryMasterId", id);
                    var datalst = dapperConnection.GetListResult<GalleryImagesModel>("GetAllGalleryImages", CommandType.StoredProcedure, dictionary).ToList();
                    if(datalst!= null)
                    {
                        dataModel.lstGalleryImagesModels = datalst;
                    }
                }
                //throw new NotImplementedException();
                return dataModel;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllGalleryMasterLanguageId", ex.ToString(), "GalleryService", "Get");
                return null;
            }
        }

        public List<GalleryModel> GetList()
        {
            try
            {
                //Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<GalleryModel>("GetAllGalleryMaster", CommandType.StoredProcedure).ToList();
                
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllGalleryMasterLanguageId", ex.ToString(), "GalleryService", "GetList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdate(GalleryModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("pId", model.Id);
                    dictionary.Add("pPlaceName", model.PlaceName);
                    dictionary.Add("pThumbImageName", model.ThumbImageName);
                    dictionary.Add("pThumbImagePath", model.ThumbImagePath);
                    dictionary.Add("pIsVideo", model.IsVideo);
                    dictionary.Add("pIsActive", model.IsActive);
                    dictionary.Add("pGalleryDate", model.GalleryDate);
                    dictionary.Add("pUsername", username);

                    var data = dapperConnection.GetListResult<long>("InsertOrUpdateGalleryMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                    if (model.Id == 0)
                    {
                        jsonResponseModel.strMessage = "Record inserted successfully";
                        jsonResponseModel.isError = false;
                        jsonResponseModel.type = PopupMessageType.success.ToString();
                    }
                    else
                    {
                        jsonResponseModel.strMessage = "Record updated successfully";
                        jsonResponseModel.isError = false;
                        jsonResponseModel.type = PopupMessageType.success.ToString();
                    }
                    model.Id = (long)data;

                    if (model.lstGalleryImagesModels != null)
                    {

                        Dictionary<string, object> dictionaryRemove = new Dictionary<string, object>();
                        dictionaryRemove.Add("pGalleryMasterId", model.Id);
                        dapperConnection.GetListResult<long>("RemoveGalleryImages", CommandType.StoredProcedure, dictionaryRemove).FirstOrDefault();

                        foreach (var item in model.lstGalleryImagesModels.ToList())
                        {
                            Dictionary<string, object> dictionarySub = new Dictionary<string, object>();
                            dictionarySub.Add("pGalleryMasterId", model.Id);
                            dictionarySub.Add("pImageName", item.ImageName);
                            dictionarySub.Add("pImagePath", item.ImagePath);

                            dapperConnection.GetListResult<long>("InsertGalleryImages", CommandType.StoredProcedure, dictionarySub).FirstOrDefault();
                        }
                    }

                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into InsertOrUpdateGalleryMaster", ex.ToString(), "GalleryService", "AddOrUpdate");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveGalleryMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveGalleryMaster", ex.ToString(), "GalleryService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }


        public List<AlbumModel> GetAlbum()
        {
            try
            {

                List<AlbumModel> dataMain = new List<AlbumModel>();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //int pageSize = 10, int skip = 0,
                //dictionary.Add("ppagesize", pageSize);
                //dictionary.Add("pskip", skip);
                var data = dapperConnection.GetListResult<AlbumModel>("GetAllAlbumFirstData", CommandType.StoredProcedure, dictionary).ToList();

                //var dataTypeList = GetEventTypeList().Select(x => new AlbumModel
                //{
                //    Id = x.Id,
                //}).ToList();

                //dataTypeList.ForEach(x => {
                //    var da = data.Where(y => x.Id == y.Id).FirstOrDefault();
                //    if (da != null)
                //    {
                //        x.FirstImagePath = da.FirstImagePath;
                //    }
                //});

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllYogaEventMasterFilter", ex.ToString(), "GetAllYogaEventMaster", "GetList");
                return null;
            }
        }
        public List<AlbumModel> GetCMAlbum()
        {
            try
            {

                List<AlbumModel> dataMain = new List<AlbumModel>();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                var data = dapperConnection.GetListResult<AlbumModel>("getallalbumCMfirstdata", CommandType.StoredProcedure, dictionary).ToList();

                //var dataTypeList = GetEventTypeList().Select(x => new AlbumModel
                //{
                //    Id = x.Id,
                //}).ToList();

                //dataTypeList.ForEach(x => {
                //    var da = data.Where(y => x.Id == y.Id).FirstOrDefault();
                //    if (da != null)
                //    {
                //        x.FirstImagePath = da.FirstImagePath;
                //    }
                //});

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllYogaEventMasterFilter", ex.ToString(), "GetAllYogaEventMaster", "GetList");
                return null;
            }
        }
        public List<EventTypeModel> GetEventTypeList()
        {
            try
            {
                var data = dapperConnection.GetListResult<EventTypeModel>("GetAllEventTypeMaster", CommandType.StoredProcedure).ToList();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllYogaEventTypeMaster", ex.ToString(), "GetEventType", "GetList");
                return null;
            }
        }
        public List<GalleryImagesModel> GetAlbumImages(long id)
        {
            try
            {
                List<GalleryImagesModel> dataMain = new List<GalleryImagesModel>();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("p_id", id);
                var data = dapperConnection.GetListResult<GalleryImagesModel>("GetAllAlbumImages", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllYogaEventMasterFilter", ex.ToString(), "GetAllYogaEventMaster", "GetList");
                return null;
            }
        }

        public List<VideoAlbumModel> GetAllvideoalbum()
        {
            try
            {

                List<VideoAlbumModel> dataMain = new List<VideoAlbumModel>();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                var data = dapperConnection.GetListResult<VideoAlbumModel>("GetAllvideoAlbum", CommandType.StoredProcedure, dictionary).ToList();               

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllYogaEventMasterFilter", ex.ToString(), "GetAllYogaEventMaster", "GetList");
                return null;
            }
        }
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~GalleryService()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The dispose method that implements IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The virtual dispose method that allows
        /// classes inherithed from this one to dispose their resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }

                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        #endregion
    }
}
