using System;
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
    public class PublicationTypeService : IPublicationTypeMaster
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public PublicationTypeService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public PublicationTypeMasterModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPublicationTypeMasterLanguageId", ex.ToString(), "PublicationTypeService", "Get");
                return null;
            }
        }

        public List<PublicationTypeMasterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLangId", lgLangId);
                var data = dapperConnection.GetListResult<PublicationTypeMasterModel>("GetAllPublicationTypeMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.PublicationTypeId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPublicationTypeMasterLanguageId", ex.ToString(), "PublicationTypeService", "GetList");
                return null;
            }
        }

        public List<PublicationTypeMasterModel> GetListF()
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
               
                var data = dapperConnection.GetListResult<PublicationTypeMasterModel>("GetAllBannerMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.PublicationTypeId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPublicationTypeMasterLanguageId", ex.ToString(), "PublicationTypeService", "GetList");
                return null;
            }
        }

     

        public JsonResponseModel AddOrUpdate(PublicationTypeMasterModel model, string username)
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
                dictionary.Add("pPublicationTypeName", model.PublicationTypeName);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUsername", username);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdatePublicationtypemaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdatePublicationtypemaster", ex.ToString(), "PublicationTypeService", "AddOrUpdate");
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
                dictionary.Add("pId", id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemovePublicationTypeMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemovePublicationTypeMaster", ex.ToString(), "PublicationTypeService", "Delete");
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
        ~PublicationTypeService()
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
