using CommonCMS.Webs.Common;
using CommonCMS.Webs.Models;
using System.Data;
using CommonCMS.Webs.IService;

namespace CommonCMS.Webs.Service
{
    public class AdminMenuMasterService : IAdminMenuMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public AdminMenuMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public AdminMenuMasterModel Get(long id)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "Get");
                return null;
            }
        }

        public List<AdminMenuMasterModel> GetList()
        {
            try
            {
                return dapperConnection.GetListResult<AdminMenuMasterModel>("GetAllAdminMenuMaster", CommandType.StoredProcedure).ToList();
            }catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "GetList");
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
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveAdminMenuMaster", CommandType.StoredProcedure, dictionary).ToList();

                strMessage = "Record Removed success";
                isError = false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "Delete");
                strMessage = ex.Message;
                isError = true;
            }
            return isError;
        }

        public bool AddOrUpdate(AdminMenuMasterModel model, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("Name", model.Name);
                dictionary.Add("MenuType", model.MenuType);
                dictionary.Add("MenuRank", model.MenuRank);
                dictionary.Add("ParentId", model.ParentId);
                dictionary.Add("IsActive", model.IsActive);
                dictionary.Add("ChangedBy", model.CreatedBy);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateAdminMenuMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "AddOrUpdate");
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
        ~AdminMenuMasterService()
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
