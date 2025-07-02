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
    public class PayScalServices : IPayScalServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion
        #region Constructor
        public PayScalServices()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion
        #region Public Method(s)
        public PayScalMasterModel Get(long id)
        {
            try
            {
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(" Error Into GetAllPayscal", ex.ToString(), "GetAllPayscal", "Get");
                return null;
            }
        }
        #endregion
        public List<PayScalMasterModel> GetList()
        {
            try
            {
                return dapperConnection.GetListResult<PayScalMasterModel>("GetAllPayscalById", CommandType.StoredProcedure).ToList();
            }

            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPayscalById", ex.ToString(), "GetAllPayscalById", "GetList");
                return null;
            }
        }
        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<PayScalMasterModel>("RemovePayscaldetails", CommandType.StoredProcedure, dictionary).ToList();
                jsonResponseModel.strMessage = "Record Removed success";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemovePayscaldetails", ex.ToString(), "PayscaldetailsService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(PayScalMasterModel model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            {
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("Id", model.Id);
                    dictionary.Add("PayScalLevel", model.PayScalLevel);
                    dictionary.Add("IsActive", model.IsActive);
                   

                    var data = dapperConnection.GetListResult<long>("InsertOrUpdatePayscaldetails", CommandType.StoredProcedure, dictionary).FirstOrDefault();
                    if (model.Id == 0)
                    {
                        jsonResponseModel.strMessage = "Record inserted success";
                        jsonResponseModel.isError = false;
                        jsonResponseModel.type = PopupMessageType.success.ToString();
                    }
                    else
                    {
                        jsonResponseModel.strMessage = "Record updated success";
                        jsonResponseModel.isError = false;
                        jsonResponseModel.type = PopupMessageType.success.ToString();
                    }
                    model.Id = (int)data;
                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into InsertOrUpdatePayscaldetails", ex.ToString(), "Payscal", "AddOrUpdate");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }
        #region Disposing Method(s)
        private bool disposed;
        ~PayScalServices()
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
