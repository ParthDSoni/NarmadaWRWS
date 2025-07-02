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
    public class MinisterMasterService : IMinisterServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public MinisterMasterService()
        {
            dapperConnection = new DapperConnection();
        }


        #endregion
        public JsonResponseModel ElectionModeOn()
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            var data = dapperConnection.GetListResult<long>("GetAllElectionModeON", CommandType.StoredProcedure).FirstOrDefault();
            return jsonResponseModel;
        }
        public JsonResponseModel ElectionModeOFF()
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            var data = dapperConnection.GetListResult<long>("GetAllElectionModeOFF", CommandType.StoredProcedure).FirstOrDefault();
            return jsonResponseModel;
        }
        public List<MinisterModel> GetElectionData()
        {
            try
            {
                var data = dapperConnection.GetListResult<MinisterModel>("CheckElectionModeData", CommandType.StoredProcedure).ToList();
                //data.ForEach(x => {
                //    x.Id = (long)x.MinisterID;
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetElectionModeData", ex.ToString(), "MinisterMasterService", "GetList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdate(MinisterModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                //if (string.IsNullOrWhiteSpace(model.ImageName))
                //{
                //    model.ImageName = "";
                //}
                //if (string.IsNullOrWhiteSpace(model.ImagePath))
                //{
                //    model.ImagePath = "";
                //}
                //if (model.Id != 0)
                //{
                //    var dataModel = Get(model.Id);
                //    if (dataModel != null)
                //    {
                //        if (model.ImagePath != null)
                //        {
                //            model.ImageName = dataModel.ImageName;
                //            model.ImagePath = dataModel.ImagePath;
                //        }
                //    }
                //}
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.Id);
                dictionary.Add("pLanguageId", model.LanguageId);
                dictionary.Add("pMinisterName", model.MinisterName);
                dictionary.Add("pMinisterDescription", model.MinisterDescription);
                dictionary.Add("ImageName", model.ImageName);
                dictionary.Add("ImagePath", model.ImagePath);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("Username", username);
                dictionary.Add("pMinisterRank", model.MinisterRank);
                dictionary.Add("pElectionMode", model.ElectionMode);


                var data = dapperConnection.GetListResult<long>("InsertOrUpdateMinisterMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateMinisterMaster", ex.ToString(), "MinisterMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveMinisterMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveMinisterMaster", ex.ToString(), "MinisterMasterService", "Delete");
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

                    MinisterModel masterModel = getDetails.Where(x => x.MinisterRank == rank).FirstOrDefault();

                    var cuurentLevelList = getDetails.OrderBy(x => x.MinisterRank).ToList();

                    long minValue = cuurentLevelList.Min(x => x.MinisterRank);
                    long maxValue = cuurentLevelList.Max(x => x.MinisterRank);

                    long updateRank = 0;

                    if (dir == "up" && (rank - 1) < minValue)
                    {
                        jsonResponseModel.strMessage = "This  already have min rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else if (dir == "down" && (rank + 1) > maxValue)
                    {
                        jsonResponseModel.strMessage = "This  already have max rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else
                    {
                        var indexList = cuurentLevelList.Select((x, i) => new
                        {
                            item = x,
                            index = i
                        }).ToList();

                        foreach (var cuurentLevel in indexList)
                        {
                            if (dir == "up" && cuurentLevel.item.MinisterRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index - 1)).FirstOrDefault().item.MinisterRank;
                                break;
                            }
                            else if (dir == "down" && cuurentLevel.item.MinisterRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index + 1)).FirstOrDefault().item.MinisterRank;
                                break;
                            }
                        }


                        MinisterModel masterupdateRankModel = getDetails.Where(x => x.MinisterRank == updateRank).FirstOrDefault();

                        if (masterModel != null && masterupdateRankModel != null)
                        {
                            masterModel.MinisterRank = updateRank;
                            masterupdateRankModel.MinisterRank = rank;
                            jsonResponseModel = AddOrUpdate(masterModel, masterModel.CreatedBy);
                            jsonResponseModel = AddOrUpdate(masterupdateRankModel, masterModel.CreatedBy);

                            jsonResponseModel.strMessage = "Data Swap Successfully";

                            transactionScope.Complete();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into SwapSequance", ex.ToString(), "CMSMenuMasterService", "SwapSequance");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }
        public MinisterModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllMinisterMasterLanguageId", ex.ToString(), "MinisterMasterService", "Get");
                return null;
            }
        }

        public List<MinisterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<MinisterModel>("GetAllMinisterMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = (long)x.MinisterID;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllMinisterMasterLanguageId", ex.ToString(), "MinisterMasterService", "GetList");
                return null;
            }
        }
        public List<MinisterModel> GetListFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<MinisterModel>("GetAllMinisterMasterLanguageIdFront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = (long)x.MinisterID;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllMinisterMasterLanguageIdFront", ex.ToString(), "MinisterMasterService", "GetList");
                return null;
            }
        }
        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~MinisterMasterService()
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
