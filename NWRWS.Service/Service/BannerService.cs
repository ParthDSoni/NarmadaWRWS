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
    public class BannerService : IBannerService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public BannerService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public BannerModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBannerMasterLanguageId", ex.ToString(), "BannerService", "Get");
                return null;
            }
        }

        public List<BannerModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<BannerModel>("GetAllBannerMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.BannerId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBannerMasterLanguageId", ex.ToString(), "BannerService", "GetList");
                return null;
            }
        }

        public List<BannerModel> GetListF(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<BannerModel>("GetAllBannerMasterLanguageIdF", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.BannerId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBannerMasterLanguageId", ex.ToString(), "BannerService", "GetList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdate(BannerModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(model.ImageName))
                {
                    model.ImageName = "";
                }
                if (model.Id != 0)
                {
                    var dataModel = Get(model.Id);
                    if (dataModel != null)
                    {
                        if (string.IsNullOrWhiteSpace(model.ImageName))
                        {
                            model.ImageName = dataModel.ImageName;
                        }
                    }
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("LanguageId", model.LanguageId);
                dictionary.Add("Title", model.Title);
                dictionary.Add("pURL", model.URL);
                dictionary.Add("ImageName", model.ImageName);
                dictionary.Add("ImagePath", model.ImagePath);
                dictionary.Add("Description", model.Description);
                dictionary.Add("BannerRank", model.BannerRank);
                dictionary.Add("IsActive", model.IsActive);
                dictionary.Add("Username", username);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateBannerMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateBannerMaster", ex.ToString(), "BannerService", "AddOrUpdate");
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
                dictionary.Add("P_Id", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveBannerMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveBannerMaster", ex.ToString(), "BannerService", "Delete");
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

                    BannerModel masterModel = getDetails.Where(x => x.BannerRank == rank).FirstOrDefault();

                    var cuurentLevelList = getDetails.OrderBy(x => x.BannerRank).ToList();

                    long minValue = cuurentLevelList.Min(x => x.BannerRank);
                    long maxValue = cuurentLevelList.Max(x => x.BannerRank);

                    long updateRank = 0;

                    if (dir == "up" && (rank - 1) < minValue)
                    {
                        jsonResponseModel.strMessage = "This Menu already have min rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else if (dir == "down" && (rank + 1) > maxValue)
                    {
                        jsonResponseModel.strMessage = "This Menu already have max rank!";
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
                            if (dir == "up" && cuurentLevel.item.BannerRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index - 1)).FirstOrDefault().item.BannerRank;
                                break;
                            }
                            else if (dir == "down" && cuurentLevel.item.BannerRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index + 1)).FirstOrDefault().item.BannerRank;
                                break;
                            }
                        }


                        BannerModel masterupdateRankModel = getDetails.Where(x => x.BannerRank == updateRank).FirstOrDefault();

                        if (masterModel != null && masterupdateRankModel != null)
                        {
                            masterModel.BannerRank = updateRank;
                            masterupdateRankModel.BannerRank = rank;
                            jsonResponseModel = AddOrUpdate(masterModel,masterModel.CreatedBy);
                            jsonResponseModel = AddOrUpdate(masterupdateRankModel,masterModel.CreatedBy);

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
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~BannerService()
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
