using CommonCMS.Webs.Common;
using CommonCMS.Webs.Models;
using System.Data;
using CommonCMS.Webs.IService;
using System.Transactions;

namespace CommonCMS.Webs.Service
{
    public class MenuRightsMasterService : IMenuRightsMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public MenuRightsMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public List<MenuRightsMasterModel> GetList()
        {
            try
            {
                return dapperConnection.GetListResult<MenuRightsMasterModel>("GetAllMenuRightsMaster", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllMenuRightsMaster", ex.ToString(), "MenuRightsMasterService", "GetList");
                return null;
            }
        }

        public List<MenuRightsMasterModel> GetListByRoleId(long id)
        {
            try
            {
                Dictionary<string, object> dictionaryUserRole = new Dictionary<string, object>();
                dictionaryUserRole.Add("UserRoleId", id);
                return dapperConnection.GetListResult<MenuRightsMasterModel>("GetAllMenuRightsMasterByRoleId", CommandType.StoredProcedure, dictionaryUserRole).ToList();
            }catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllMenuRightsMasterByRoleId", ex.ToString(), "MenuRightsMasterService", "GetListByRoleId");
                return null;
            }
        }

        public bool DeleteByRole(long id, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
            try
            {
                Dictionary<string, object> dictionaryUserRole = new Dictionary<string, object>();
                dictionaryUserRole.Add("UserRoleId", id);
                dapperConnection.GetListResult<MenuResourceMasterModel>("RemoveRoleMenuRightsByRoleId", CommandType.StoredProcedure, dictionaryUserRole).ToList();

                strMessage = "Record Removed success";
                isError = false;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveRoleMenuRightsByRoleId", ex.ToString(), "MenuRightsMasterService", "DeleteByRole");
                strMessage = ex.Message;
                isError = true;
            }
            return isError;
        }

        public bool Insert(List<MenuRightsMasterModel> model, long lgRoleId, string strUsername, out string strMessage)
        {
            bool isError = true;
            strMessage = "";
            int iCount = 0;
            try
            {
                using (var transactionScope = new TransactionScope())
                {

                    Dictionary<string, object> dictionaryUserRole = new Dictionary<string, object>();
                    dictionaryUserRole.Add("UserRoleId", lgRoleId);
                    dapperConnection.GetListResult<MenuResourceMasterModel>("RemoveRoleMenuRightsByRoleId", CommandType.StoredProcedure, dictionaryUserRole).ToList();

                    if (model.Count() > 0)
                    {
                        foreach (var item in model)
                        {
                            Dictionary<string, object> dictionary = new Dictionary<string, object>();
                            dictionary.Add("pRoleId", lgRoleId);
                            dictionary.Add("pMenuId", item.MenuResourceId);
                            dictionary.Add("pInsert", item.Insert);
                            dictionary.Add("pUpdate", item.Update);
                            dictionary.Add("pDelete", item.Delete);
                            dictionary.Add("pView", item.View);
                            dictionary.Add("pLastUpdateBy", strUsername);

                            var data = dapperConnection.ExecuteWithoutResult("InsertMenuRightsMaster", CommandType.StoredProcedure, dictionary);

                            if (data)
                            {
                                iCount++;
                            }
                        }
                    }

                    if (model.Count() == iCount)
                    {
                        transactionScope.Complete();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveRoleMenuRightsByRoleId , InsertMenuRightsMaster ", ex.ToString(), "MenuRightsMasterService", "Insert");
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
        ~MenuRightsMasterService()
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
        /// classes inherited from this one to dispose their resources.
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
