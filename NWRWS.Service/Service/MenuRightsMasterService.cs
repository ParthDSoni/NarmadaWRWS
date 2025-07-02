using NWRWS.Model.Service;
using System.Data;
using NWRWS.Common;
using NWRWS.IService.Service;
using System.Transactions;

namespace NWRWS.Services.Service
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
            using (IRoleMasterService roleMasterService = new RoleMasterService())
            {
                try
                {
                    Dictionary<string, object> dictionaryUserRole = new Dictionary<string, object>();
                    dictionaryUserRole.Add("UserRoleId", id);

                    List<MenuRightsMasterModel> lstMenuList = new List<MenuRightsMasterModel>();
                    List<MenuRightsMasterModel> lstSubMenuList = dapperConnection.GetListResult<MenuRightsMasterModel>("GetAllMenuRightsMasterByRoleId", CommandType.StoredProcedure, dictionaryUserRole).ToList();
                    if (roleMasterService.Get(id).RoleName.ToLower() == ("admin"))
                    {
                        var adminMas = lstSubMenuList.FirstOrDefault(x => x.Name == "Admin Master");
                        adminMas.Insert = true;
                        adminMas.Update = true;
                        adminMas.Delete = true;
                        adminMas.View = true;

                        var adminMenu = lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/AdminMenu");
                        adminMenu.Insert = true;
                        adminMenu.Update = true;
                        adminMenu.Delete = true;
                        adminMenu.View = true;

                        var MenuResource = lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/MenuResource");
                        MenuResource.Insert = true;
                        MenuResource.Update = true;
                        MenuResource.Delete = true;
                        MenuResource.View = true;

                        var MenuRights = lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/MenuRights");
                        MenuRights.Insert = true;
                        MenuRights.Update = true;
                        MenuRights.Delete = true;
                        MenuRights.View = true;

                        var RoleMaster = lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/RoleMaster");
                        RoleMaster.Insert = true;
                        RoleMaster.Update = true;
                        RoleMaster.Delete = true;
                        RoleMaster.View = true;

                        var UserMaster = lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/UserMaster");
                        UserMaster.Insert = true;
                        UserMaster.Update = true;
                        UserMaster.Delete = true;
                        UserMaster.View = true;

                        lstSubMenuList.Remove(lstSubMenuList.FirstOrDefault(x => x.Name == "Admin Master"));
                        lstSubMenuList.Remove(lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/AdminMenu"));
                        lstSubMenuList.Remove(lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/MenuResource"));
                        lstSubMenuList.Remove(lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/MenuRights"));
                        lstSubMenuList.Remove(lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/RoleMaster"));
                        lstSubMenuList.Remove(lstSubMenuList.FirstOrDefault(x => x.MenuURL == "/Admin/UserMaster"));


                        lstMenuList.Add(adminMas);
                        lstMenuList.Add(adminMenu);
                        lstMenuList.Add(MenuResource);
                        lstMenuList.Add(MenuRights);
                        lstMenuList.Add(RoleMaster);
                        lstMenuList.Add(UserMaster);

                    }

                    lstSubMenuList.ForEach(x => {
                        long lgRank = lstSubMenuList.Max(y => y.MenuRank) + 1;
                        x.MenuRank = lgRank;
                        lstMenuList.Add(x);
                    });

                    return lstMenuList;

                }catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into GetAllMenuRightsMasterByRoleId", ex.ToString(), "MenuRightsMasterService", "GetListByRoleId");
                    return null;
                }
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

                strMessage = "Record removed successfully";
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
                    else
                    {
                        strMessage = "No Rights updated...";
                        return false;
                    }

                    if (model.Count() == iCount)
                    {
                        transactionScope.Complete();
                        strMessage = "All user rights Are Updated";
                        return false;
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
