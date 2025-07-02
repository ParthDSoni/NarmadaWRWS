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
    public class JsMasterService : IJsMasterService
    {
        #region Constant
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor
        public JsMasterService()
        { 
            dapperConnection = new DapperConnection();
        }
        #endregion

        #region Public Action Method
        public JSMasterModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllJsMasterLanguageId", ex.ToString(), "JsMasterService", "Get");
                return null;
            }
        }

        public List<JSMasterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                var data = dapperConnection.GetListResult<JSMasterModel>("GetAllJsMaster", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllJsMasterLanguageId", ex.ToString(), "JsMasterService", "GetList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdate(JSMasterModel model, string username)
        {
            JsonResponseModel responseModel = new JsonResponseModel();
            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("Id", model.Id);
                    dictionary.Add("Title", model.Title); 
                    dictionary.Add("Jsfile", model.Jsfile);
                    dictionary.Add("IsActive", model.IsActive);
                    dictionary.Add("Username", username);

                    var data = dapperConnection.GetListResult<long>("InsertOrUpdateJsMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                    if (model.Id == 0)
                    {
                        responseModel.strMessage = "Record inserted successfully";
                        responseModel.isError = false;
                        responseModel.type = PopupMessageType.success.ToString();
                    }
                    else
                    {
                        responseModel.strMessage = "Record updated successfully";
                        responseModel.isError = false;
                        responseModel.type = PopupMessageType.success.ToString();
                    }
                    transactionScope.Complete();
                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into InsertOrUpdateJsMaster,RemoveJsMasterDocumentsByJsId,InsertJsMasterDocuments", ex.ToString(), "JsMasterService", "AddOrUpdate");
                    responseModel.strMessage = ex.Message;
                    responseModel.isError = true;
                    responseModel.type = PopupMessageType.error.ToString();
                }
            }
            return responseModel;
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveJsMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveJsMaster", ex.ToString(), "JsMasterService", "Delete");
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
        ~JsMasterService()
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
