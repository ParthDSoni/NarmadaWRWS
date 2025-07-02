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

namespace NWRWS.Services.Service
{
    public class PopupMasterService : IPopupServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public PopupMasterService()
        {
            dapperConnection = new DapperConnection();
        }


        #endregion
        public JsonResponseModel AddOrUpdate(PopupModel model, string username)
         {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {                 
                if (model.Id != 0)
                {
                    var dataModel = Get(model.Id);                     
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.Id);
                dictionary.Add("pLanguageId", model.LanguageId);
                dictionary.Add("ppopupDescription", model.popupDescription);     
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("Username", username);
                var data = dapperConnection.GetListResult<long>("InsertOrUpdatePopupMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdatePopupMaster", ex.ToString(), "PopupMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemovePopupMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemovePopupMaster", ex.ToString(), "PopupMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

         
        public PopupModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPopupMasterLanguageId", ex.ToString(), "PopupMasterService", "Get");
                return null;
            }
        }

        public List<PopupModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<PopupModel>("GetAllPopupMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPopupMasterLanguageId", ex.ToString(), "PopupMasterService", "GetList");
                return null;
            }
        }

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~PopupMasterService()
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
