using NWRWS.Common;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.IService.Service;
using System.Data;

namespace NWRWS.Services.Service
{
    public class CanalMasterService : ICanalMasterService
    {


        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor
        public CanalMasterService()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion

        #region Public Method(s)
        public CanalModel GetCanal(long id, int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pId", id);
                dict.Add("pLangId", languageId);

                return dapperConnection.GetListResult<CanalModel>("GetCanalById", CommandType.StoredProcedure, dict).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetCanalById", ex.ToString(), "CanalMasterService", "GetCanal");
                return null;
            }
        }

        public List<CanalModel> GetList(int languageId = 1, long regionId = 0)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                dict.Add("pRegionId", regionId);

                var data = dapperConnection.GetListResult<CanalModel>("GetAllCanalData", CommandType.StoredProcedure, dict).ToList();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCanalData", ex.ToString(), "CanalMasterService", "GetList");
                return new List<CanalModel>();
            }
        }
             
        public List<CanalModel> GetCanalListF(int languageId = 1, long regionId = 0, long canalId = 0)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                dict.Add("pRegionId", regionId);
                var data = dapperConnection.GetListResult<CanalModel>("GetAllCanalData", CommandType.StoredProcedure, dict).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCanalDataFront", ex.ToString(), "CanalMasterService", "GetCanalListF");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdateCanal(CanalModel model, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.CanalId);
                dictionary.Add("pRegionId", model.RegionId);
                dictionary.Add("pLangId", model.LanguageId);
                dictionary.Add("pName", model.Name);
                dictionary.Add("pUploadDocumentPath", model.UploadDocumentPath ?? "");
                dictionary.Add("pDesc", model.Description ?? "");
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUserId", userId);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateCanalMaster",
                              CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = model.Id == 0
                    ? "Record inserted successfully"
                    : "Record updated successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
                model.Id = (long)data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateCanalMaster", ex.ToString(),
                                 "CanalMasterService", "AddOrUpdateCanal");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
       
        public JsonResponseModel DeleteCanal(long id, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUserId", userId);
                dapperConnection.GetListResult<long>("RemoveCanalMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveCanalMaster", ex.ToString(), "CanalMasterService", "DeleteCanal");
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
        ~CanalMasterService()
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
