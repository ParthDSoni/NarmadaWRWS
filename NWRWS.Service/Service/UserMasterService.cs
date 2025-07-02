using NWRWS.Model.Service;
using NWRWS.Common;
using System.Data;
using NWRWS.IService.Service;
using NWRWS.Model.System;

namespace NWRWS.Services.Service
{
    public class UserMasterService : IUserMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public UserMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public UserMasterModel Get(long id)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllUserMaster", ex.ToString(), "UserMasterService", "Get");
                return null;
            }
        }

        public List<UserMasterModel> GetList()
        {
            try
            {
                return dapperConnection.GetListResult<UserMasterModel>("GetAllUserMaster", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllUserMaster", ex.ToString(), "UserMasterService", "GetList");
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
                dictionary.Add("ChangedBy", username);
                dapperConnection.GetListResult<UserMasterModel>("RemoveUserMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveUserMaster", ex.ToString(), "UserMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(UserMasterModel model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("RoleId", model.RoleId);
                dictionary.Add("FirstName", model.FirstName);
                dictionary.Add("LastName", model.LastName);
                dictionary.Add("PhoneNo", model.PhoneNo);
                dictionary.Add("Email", model.Email);
                dictionary.Add("Username", model.Username);
                dictionary.Add("UserPassword", model.UserPassword);
                dictionary.Add("IsActive", model.IsActive);
                dictionary.Add("pDistrict", model.District);
                dictionary.Add("ChangedBy", model.CreatedBy);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateUserMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateUserMaster", ex.ToString(), "UserMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel ChangePassword(ChangePasswordModel model)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.Id);
                dictionary.Add("pUserPassword", model.UserPassword);
                dictionary.Add("pChangedBy", model.CreateBy);

                var data = dapperConnection.GetListResult<long>("ChangePassword", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                objreturn.isError = false;
                model.Id = (int)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateUserMaster", ex.ToString(), "UserMasterService", "AddOrUpdate");
                objreturn.isError = true;
            }
            return objreturn;
        }
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~UserMasterService()
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
