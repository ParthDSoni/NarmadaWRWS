using Dapper;
using NWRWS.Model.Service;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.System;
using System.Data;
using System.Drawing.Printing;

namespace NWRWS.Services.Service
{
    public class ResearchMasterService : IResearchMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public ResearchMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)
        public ResearchMasterModel Get(long lgid, long lgLangId = 1)
        {
            try
            {
                List<ResearchMasterModel> data = new List<ResearchMasterModel>();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("pid", lgid);
                data = dapperConnection.GetListResult<ResearchMasterModel>("getresearchbyid", CommandType.StoredProcedure, dictionary).ToList();
                return data[0];
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getresearchbyid", ex.ToString(), "PressReleaseMasterService", "Get");
                return null;
            }
        }
		public List<SchmeTypeMaster> GetSchmeTypeData()
		{
			try
			{
				List<SchmeTypeMaster> data = new List<SchmeTypeMaster>();
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				
				data = dapperConnection.GetListResult<SchmeTypeMaster>("GetSchmeTypeData", CommandType.StoredProcedure, dictionary).ToList();
				return data;
			}
			catch (Exception ex)
			{
				ErrorLogger.Error("Error Into getresearchbyid", ex.ToString(), "PressReleaseMasterService", "Get");
				return null;
			}
		}
		public List<ResearchMasterModel> GetList(long lgLangId ,long SchmeTypeMasterId )
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("plangId", lgLangId);
				dictionary.Add("pSchmeTypeMasterId", SchmeTypeMasterId);
				var data = dapperConnection.GetListResult<ResearchMasterModel>("GetAllResearch", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.ResearchId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "ResearchMasterService", "GetList");
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
                dapperConnection.GetListResult<ResearchMasterModel>("removeresearchmaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into removeecitizenmaster", ex.ToString(), "removeecitizenmaster", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(ResearchMasterModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(model.ImageName))
                {
                    model.ImageName = "";
                }
                if (string.IsNullOrWhiteSpace(model.ImagePath))
                {
                    model.ImagePath = "";
                }
                if (model.Id != 0)
                {
                    var dataModel = Get(model.Id);
                    if (dataModel != null)
                    {
                        if (string.IsNullOrWhiteSpace(model.ImagePath))
                        {
                            model.ImageName = dataModel.ImageName;
                            model.ImagePath = dataModel.ImagePath;
                        }
                    }
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pid", model.Id);
                dictionary.Add("planguageid", model.LanguageId);
                dictionary.Add("pSchmeTypeMasterId", model.SchmeTypeMasterId);
				dictionary.Add("presearchid", model.ResearchId);
                dictionary.Add("presearchtitle", model.ResearchTitle);
                dictionary.Add("presearchdesc", model.ResearchDesc);
                dictionary.Add("pimagename", model.ImageName);
                dictionary.Add("pimagepath", model.ImagePath);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUsername", username);

                var data = dapperConnection.GetListResult<long>("insertorupdateresearchmaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into insertorupdatetendermaster", ex.ToString(), "ResearchMasterService", "AddOrUpdate");
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
        ~ResearchMasterService()
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
