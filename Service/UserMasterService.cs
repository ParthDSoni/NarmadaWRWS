using CommonCMS.Webs.Common;
using CommonCMS.Webs.Models;
using System.Data;
using CommonCMS.Webs.IService;

namespace CommonCMS.Webs.Service
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

        public bool Delete(long id,string username, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", id);
                dictionary.Add("ChangedBy", username);
                dapperConnection.GetListResult<UserMasterModel>("RemoveUserMaster", CommandType.StoredProcedure, dictionary).ToList();

                strMessage = "Record Removed success";
                isError = false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveUserMaster", ex.ToString(), "UserMasterService", "Delete");
                strMessage = ex.Message;
                isError = true;
            }
            return isError;
        }

        public bool AddOrUpdate(UserMasterModel model, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
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
                dictionary.Add("ChangedBy", model.CreatedBy);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateUserMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                    if (model.Id == 0)
                    {
                        strMessage = "Record inserted success";
                        isError = false;
                    }
                    else
                    {
                        strMessage = "Record updated success";
                        isError = false;
                    }
                    model.Id = (long)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateUserMaster", ex.ToString(), "UserMasterService", "AddOrUpdate");
                strMessage = ex.Message;
                isError = true;
            }
            return isError;
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
