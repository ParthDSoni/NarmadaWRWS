using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;

namespace NWRWS.Services.Service
{
    public class RiverDamMasterService : IRiverDamMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor
        public RiverDamMasterService()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion

        #region Public Method(s)

        public RiverDamModel GetRiver(long id, int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pId", id);
                dict.Add("pLangId", languageId);

                return dapperConnection.GetListResult<RiverDamModel>("GetRiverById", CommandType.StoredProcedure, dict).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetRiverById", ex.ToString(), "RiverMasterService", "GetRiver");
                return null;
            }       
        }
		public List<RiverDamModel> GetCanalListF(int languageId = 1, long regionId = 0, long canalId = 0)
		{
			try
			{
				Dictionary<string, object> dict = new Dictionary<string, object>();
				dict.Add("pLangId", languageId);
				dict.Add("pRegionId", regionId);
				var data = dapperConnection.GetListResult<RiverDamModel>("GetAllCanalData", CommandType.StoredProcedure, dict).ToList();
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
		public List<RiverDamModel> GetRiverList(int languageId = 1, long regionId = 0,long basinId = 0)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                dict.Add("pRegionId", regionId);
                dict.Add("pBasinId", basinId);
                var data = dapperConnection.GetListResult<RiverDamModel>("GetAllRiverData", CommandType.StoredProcedure, dict).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllRiverData", ex.ToString(), "RiverMasterService", "GetRiverList");
                return null;
            }
        }

        public List<RiverDamModel> GetRiverListF(int languageId = 1, long regionId = 0, long basinId = 0)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                dict.Add("pRegionId", regionId);
                dict.Add("pBasinId", basinId);
                var data = dapperConnection.GetListResult<RiverDamModel>("GetAllRiverData", CommandType.StoredProcedure, dict).ToList();
                //data.ForEach(x =>
                //{
                //    x.Id = (long)x.RegionId;
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllRiverData", ex.ToString(), "RiverMasterService", "GetRiverList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdateRiver(RiverDamModel model, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.Id);
                dictionary.Add("pBasinId", model.BasinId);
                dictionary.Add("pRegionId", model.RegionId);
                dictionary.Add("pLangId", model.LanguageId);
                dictionary.Add("pName", model.Name);
                dictionary.Add("pUploadDocumentPath", model.UploadDocumentPath);
                dictionary.Add("pDesc", model.Description);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUserId", userId);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateRiverMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateRiverMaster", ex.ToString(), "RiverMasterService", "AddOrUpdateRiver");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel DeleteRiver(long id, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUserId", userId);
                dapperConnection.GetListResult<long>("RemoveRiverMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveRiverMaster", ex.ToString(), "RiverMasterService", "DeleteRiver");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public RiverDamModel GetDam(long id, int languageId = 1)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pId", id);
                dict.Add("pLangId", languageId);

                return dapperConnection.GetListResult<RiverDamModel>("GetDamById", CommandType.StoredProcedure, dict).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetDamById", ex.ToString(), "RiverDamMasterService", "GetDam");
                return null;
            }
        }

        public List<RiverDamModel> GetDamList(int languageId = 1, long regionId = 0, long basinId = 0)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                dict.Add("pRegionId", regionId);
                dict.Add("pBasinId", basinId);
                var data = dapperConnection.GetListResult<RiverDamModel>("GetAllDamData", CommandType.StoredProcedure, dict).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllDamData", ex.ToString(), "RiverDamMasterService", "GetdamList");
                return null;
            }
        }

        public List<RiverDamModel> GetDamListF(int languageId = 1, long regionId = 0, long basinId = 0)
        {
            try
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                dict.Add("pLangId", languageId);
                dict.Add("pRegionId", regionId);
                dict.Add("pBasinId", basinId);
                var data = dapperConnection.GetListResult<RiverDamModel>("GetAllDamDataF", CommandType.StoredProcedure, dict).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.RegionId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllDamData", ex.ToString(), "RiverDamMasterService", "GetdamList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdateDam(RiverDamModel model, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.Id);
                dictionary.Add("pBasinId", model.BasinId);
                dictionary.Add("pRegionId", model.RegionId);
                dictionary.Add("pLangId", model.LanguageId);
                dictionary.Add("pName", model.Name);
                dictionary.Add("pUploadDocumentPath", model.UploadDocumentPath);
                dictionary.Add("pDesc", model.Description);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUserId", userId);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateDamMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateDamMaster", ex.ToString(), "RiverDamMasterService", "AddOrUpdateDam");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel DeleteDam(long id, long userId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUserId", userId);
                dapperConnection.GetListResult<long>("RemoveDamMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveDamMaster", ex.ToString(), "RiverDamMasterService", "DeleteDam");
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
        ~RiverDamMasterService()
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
