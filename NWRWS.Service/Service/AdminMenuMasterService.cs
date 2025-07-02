
using NWRWS.Common;
using NWRWS.Model.Service;
using System.Data;
using NWRWS.IService.Service;
using NWRWS.Model.System;
using System.Transactions;

namespace NWRWS.Services.Service
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
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "GetList");
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
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveAdminMenuMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel SwapSequance(long rank, string dir, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var getDetails = GetList();

                    AdminMenuMasterModel masterModel = getDetails.Where(x => x.MenuRank == rank).FirstOrDefault();

                    var cuurentLevelList = getDetails.Where(x => x.ParentId == masterModel.ParentId).OrderBy(x => x.MenuRank).ToList();

                    long minValue= cuurentLevelList.Min(x=> x.MenuRank);
                    long maxValue= cuurentLevelList.Max(x=> x.MenuRank);

                    long updateRank = 0;

                    if (dir == "up" && (rank - 1) < minValue)
                    {
                        jsonResponseModel.strMessage = "This Menu already have min rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else if (dir == "down" && (rank+1) > maxValue)
                    {
                        jsonResponseModel.strMessage = "This Menu already have max rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else
                    {
                        var indexList=cuurentLevelList.Select((x, i) => new
                        {
                            item = x,
                            index = i
                        }).ToList();

                        foreach (var cuurentLevel in indexList)
                        {
                            if (dir == "up" && cuurentLevel.item.MenuRank== rank)
                            {
                                updateRank = indexList.Where(x=> x.index== (cuurentLevel.index-1)).FirstOrDefault().item.MenuRank;
                                break;
                            }
                            else if (dir == "down" && cuurentLevel.item.MenuRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index + 1)).FirstOrDefault().item.MenuRank;
                                break;
                            }
                        }


                        AdminMenuMasterModel masterupdateRankModel = getDetails.Where(x => x.MenuRank == updateRank).FirstOrDefault();

                        if (masterModel != null && masterupdateRankModel != null)
                        {
                            masterModel.MenuRank=updateRank;
                            masterupdateRankModel.MenuRank=rank;
                            jsonResponseModel = AddOrUpdate(masterModel);
                            jsonResponseModel = AddOrUpdate(masterupdateRankModel);

                            jsonResponseModel.strMessage = "Data Swap Successfully";

                            transactionScope.Complete();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into SwapSequance", ex.ToString(), "AdminMenuMasterService", "SwapSequance");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(AdminMenuMasterModel model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("MenuId", model.MenuId);
                dictionary.Add("Name", model.Name);
                dictionary.Add("MenuType", model.MenuType);
                dictionary.Add("MenuRank", model.MenuRank);
                dictionary.Add("ParentId", model.ParentId);
                dictionary.Add("IsActive", model.IsActive);
                dictionary.Add("ChangedBy", model.CreatedBy);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateAdminMenuMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateAdminMenuMaster", ex.ToString(), "AdminMenuMasterService", "AddOrUpdate");
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
