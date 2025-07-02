using CommonCMS.Webs.Common;
using CommonCMS.Webs.Models;
using System.Data;
using CommonCMS.Webs.IService;

namespace CommonCMS.Webs.Service
{
    public class MenuResourceMasterService : IMenuResourceMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public MenuResourceMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public MenuResourceMasterModel Get(long id)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }catch (Exception ex)
            {

                ErrorLogger.Error("Error Into GetAllMenuResourceMaster", ex.ToString(), "MenuResourceMasterService", "Get");
                return null;
            }
        }

        public List<MenuResourceMasterModel> GetList()
        {
            try
            {
                return dapperConnection.GetListResult<MenuResourceMasterModel>("GetAllMenuResourceMaster", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {

                ErrorLogger.Error("Error Into GetAllMenuResourceMaster", ex.ToString(), "MenuResourceMasterService", "GetList");
                return null;
            }
        }

        public bool Delete(long id, string username, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", id);
                dictionary.Add("ChangedBy", username);
                dapperConnection.GetListResult<MenuResourceMasterModel>("RemoveMenuResourceMaster", CommandType.StoredProcedure, dictionary).ToList();

                strMessage = "Record Removed success";
                isError = false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveMenuResourceMaster", ex.ToString(), "MenuResourceMasterService", "Delete");
                strMessage = ex.Message;
                isError = true;
            }
            return isError;
        }

        public bool AddOrUpdate(MenuResourceMasterModel model, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("MenuName", model.MenuName);
                dictionary.Add("MenuURL", model.MenuURL);
                dictionary.Add("ChangedBy", model.CreatedBy);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateMenuResourceMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateMenuResourceMaster", ex.ToString(), "MenuResourceMasterService", "AddOrUpdate");
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
        ~MenuResourceMasterService()
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
