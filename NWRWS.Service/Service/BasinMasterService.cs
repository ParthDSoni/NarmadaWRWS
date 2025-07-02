using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;

namespace NWRWS.Services.Service
{
    public class BasinMasterService : IBasinMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor
        public BasinMasterService()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion

        #region Public Method(s)

        public BasinModel Get(long id, int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pId", id);
                dict.Add("pLangId", languageId);

                return dapperConnection.GetListResult<BasinModel>("GetBasinById", CommandType.StoredProcedure, dict).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetBasinById", ex.ToString(), "BasinMasterService", "Get");
                return null;
            }
        }

        public List<BasinModel> GetList(int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                var data = dapperConnection.GetListResult<BasinModel>("GetAllBasinData", CommandType.StoredProcedure, dict).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBasinData", ex.ToString(), "BasinMasterService", "GetList");
                return null;
            }
        }

        public List<BasinModel> GetListF(int languageId = 1, long regionId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLangId", languageId);
                dictionary.Add("pRegionId", regionId);
                var data = dapperConnection.GetListResult<BasinModel>("GetAllBasinDataF", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBannerMasterLanguageId", ex.ToString(), "BannerService", "GetList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdate(BasinModel model, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.BasinId);
                dictionary.Add("pRegionId", model.RegionId);
                dictionary.Add("pLangId", model.LanguageId);
                dictionary.Add("pName", model.Name);
                dictionary.Add("pDesc", model.Description);
                dictionary.Add("pUploadDocumentPath", model.UploadDocumentPath);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUserId", userId);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateBasinMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateBasinMaster", ex.ToString(), "BasinMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel Delete(long id, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUserId", userId);
                dapperConnection.GetListResult<long>("RemoveBasinMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveBasinMaster", ex.ToString(), "BasinMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~BasinMasterService()
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
