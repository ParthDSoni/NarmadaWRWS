using NWRWS.Model.Service;
using System.Data;
using NWRWS.IService.Service;
using NWRWS.Common;
using NWRWS.Model.System;

namespace NWRWS.Services.Service
{
    public class RoleMasterService : IRoleMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public RoleMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public RoleMasterModel Get(long id)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllRoleMaster", ex.ToString(), "RoleMasterService", "Get");
                return null;
            }
        }

        public List<RoleMasterModel> GetList()
        {
            try
            {
                return dapperConnection.GetListResult<RoleMasterModel>("GetAllRoleMaster", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllRoleMaster", ex.ToString(), "RoleMasterService", "GetList");
                return null;
            }
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (id > 0)
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("Id", id);
                    dictionary.Add("ChangedBy", username);
                    dapperConnection.GetListResult<RoleMasterModel>("RemoveRoleMaster", CommandType.StoredProcedure, dictionary).ToList();

                    objreturn.strMessage = "Record deleted successfully";
                    objreturn.isError = false;
                    objreturn.type = PopupMessageType.success.ToString();
                }
                else
                {
                    objreturn.strMessage = "Record not deleted, Try again";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveRoleMaster", ex.ToString(), "RoleMasterService", "Delete");
                objreturn.strMessage = "Record not deleted, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return objreturn;
        }

        public JsonResponseModel AddOrUpdate(RoleMasterModel model)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("Id", model.Id);
                    dictionary.Add("RoleName", model.RoleName);
                    dictionary.Add("IsActive", model.IsActive);
                    dictionary.Add("ChangedBy", model.CreatedBy);

                    var data = dapperConnection.GetListResult<long>("InsertOrUpdateRoleMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                    if (model.Id == 0)
                    {
                        objreturn.strMessage = "Record inserted successfully";
                        objreturn.isError = false;
                        objreturn.type = PopupMessageType.success.ToString();
                    }
                    else
                    {
                        objreturn.strMessage = "Record updated successfully";
                        objreturn.isError = false;
                        objreturn.type = PopupMessageType.success.ToString();
                    }
                }
                else
                {
                    objreturn.strMessage = "Enter Role Name";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.warning.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateRoleMaster", ex.ToString(), "RoleMasterService", "AddOrUpdate");
                objreturn.strMessage = "Record not saved, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return objreturn;
        }

        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~RoleMasterService()
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
