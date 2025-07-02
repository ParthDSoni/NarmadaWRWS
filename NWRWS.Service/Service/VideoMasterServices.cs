using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.Services.Service

{
    public class VideoMasterServices : IVideoMasterServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion
        #region Constructor
        public VideoMasterServices()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion
        #region Public Method(s)
        public VideoMasterModel Get(long id)
        {
            try
            {
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(" Error Into GetAllVideodetails", ex.ToString(), "GetAllVideodetails", "Get");
                return null;
            }
        }
        #endregion
        public List<VideoMasterModel> GetList()
        {
            try
            { 
                return dapperConnection.GetListResult<VideoMasterModel>("GetAllVideodetailsById", CommandType.StoredProcedure).ToList();
            }

            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllVideodetails", ex.ToString(), "GetAllVideodetailsById", "GetList");
                return null;
            }
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<VideoMasterModel>("RemoveVideodetails", CommandType.StoredProcedure, dictionary).ToList();
                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveVideodetails", ex.ToString(), "RemoveVideodetails", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
      
        public JsonResponseModel AddOrUpdate(VideoMasterModel model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            {
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("Id", model.Id);
                    dictionary.Add("VideoTitle", model.VideoTitle);
                    dictionary.Add("ThumbImage", model.ThumbImage);
                    dictionary.Add("VideoUrl", model.VideoUrl);
                 
                    dictionary.Add("pVideoDate", model.VideoDate);
                  
                    dictionary.Add("IsActive", model.IsActive);

                    dictionary.Add("Username", model.Username);

                    var data = dapperConnection.GetListResult<long>("InsertOrUpdateVideodetails", CommandType.StoredProcedure, dictionary).FirstOrDefault();
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
                    model.Id = (int)data;
                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into InsertOrUpdateVideodetails", ex.ToString(), "InsertOrUpdateVideodetails", "AddOrUpdate");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }
        #region Disposing Method(s)
        private bool disposed;
        ~VideoMasterServices()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }
            disposed = true;
        }
        #endregion

    }

}
