using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;

namespace NWRWS.Services.Service
{
    public class RegionMasterService : IRegionMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor
        public RegionMasterService()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion

        #region Public Method(s)

        public RegionModel Get(long id, int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pId", id);
                dict.Add("pLangId", languageId);

                return dapperConnection.GetListResult<RegionModel>("GetRegionById", CommandType.StoredProcedure, dict).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetRegionById", ex.ToString(), "RegionMasterService", "Get");
                return null;
            }
        }

        public List<RegionModel> GetList(int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                var data = dapperConnection.GetListResult<RegionModel>("GetAllRegionData", CommandType.StoredProcedure, dict).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallregiondata", ex.ToString(), "RegionMasterService", "GetList");
                return null;
            }
        }

        public List<RegionModel> GetListF(int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLangId", languageId);
                var data = dapperConnection.GetListResult<RegionModel>("GetAllRegionDataF", CommandType.StoredProcedure, dictionary).ToList();
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

        public JsonResponseModel AddOrUpdate(RegionModel model, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.RegionId);
                dictionary.Add("pLangId", model.LanguageId);
                dictionary.Add("pName", model.Name);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUserId", userId);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateRegionMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateRegionMaster", ex.ToString(), "RegionMasterService", "AddOrUpdate");
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
                dapperConnection.GetListResult<long>("RemoveRegionMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveRegionMaster", ex.ToString(), "RegionMasterService", "Delete");
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
        ~RegionMasterService()
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
