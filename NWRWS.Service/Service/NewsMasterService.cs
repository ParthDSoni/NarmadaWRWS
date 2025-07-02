using Dapper;
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
    public class NewsMasterService : INewsMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public NewsMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public NewsModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageId", ex.ToString(), "NewsMasterService", "Get");
                return null;
            }
        }

        public NewsModel GetMenuRes(long NewsId, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.NewsId == NewsId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "CMSMenuResourceMasterService", "Get");
                return null;
            }
        }

        public List<NewsModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<NewsModel>("GetAllNewsMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.NewsId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageId", ex.ToString(), "NewsMasterService", "GetList");
                return null;
            }
        }

        public List<NewTypeMasterModel> GetListNewType()
        {
            try
            {
                return dapperConnection.GetListResult<NewTypeMasterModel>("getnewtype", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getnewtype", ex.ToString(), "CMSMenuResourceMasterService", "GetListLanguage");
                return null;
            }
        }
        public List<NewsModel> GetListFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<NewsModel>("GetAllNewsMasterLanguageIdForFront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.NewsId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<NewsModel> GetNewsCircularResolution(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<NewsModel>("GetNewsCircularResolution", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.NewsId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetNewsCircularResolution", ex.ToString(), "NewsMasterService", "GetListNewsSpotlightRequirenments");
                return null;
            }
        }
        public List<NewsModel> GetLatestUpdatesListFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<NewsModel>("GetAllLatestUpdatesLanguageIdForFront", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.Id = (long)x.NewsId;
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllLatestUpdatesLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<NewsModel> GetResolutionFront(long lgLangId = 1, int pageSize = 10, int skip = 0, string? searchValue = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
                dictionary.Add("pageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("search_term", searchValue);
                List<NewsModel> data = dapperConnection.GetListResult<NewsModel>("getallResolution", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.NewsId = (long)x.NewsId;
                    //x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "NewMasterService", "GetList");
                return null;
            }
        }

        public List<NewsModel> GetCircularFront(long lgLangId = 1, int pageSize = 10, int skip = 0, string? searchValue = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
                dictionary.Add("pageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("search_term", searchValue);
                List<NewsModel> data = dapperConnection.GetListResult<NewsModel>("getallCircular", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.NewsId = (long)x.NewsId;
                    //x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "NewMasterService", "GetList");
                return null;
            }
        }
        public List<NewsModel> GetNewsFront(long lgLangId = 1, int pageSize = 10, int skip = 0, string? searchValue = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
                dictionary.Add("pageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("search_term", searchValue);
                List<NewsModel> data = dapperConnection.GetListResult<NewsModel>("getallNews", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.NewsId = (long)x.NewsId;
                    //x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "NewMasterService", "GetNewsFront");
                return null;
            }
        }
        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<NewsModel>("RemoveNewsMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveNewsMaster", ex.ToString(), "NewsMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(NewsModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(model.ImageName))
                {
                    model.ImageName = "";
                }
                if (string.IsNullOrWhiteSpace(model.ImagePath))
                {
                    model.ImagePath = "";
                }
                if (model.Id != 0)
                {
                    var dataModel = Get(model.Id);
                    if (dataModel != null)
                    {
                        if (string.IsNullOrWhiteSpace(model.ImagePath))
                        {
                            model.ImageName = dataModel.ImageName;
                            model.ImagePath = dataModel.ImagePath;
                        }
                    }
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", model.Id);
                dictionary.Add("pLanguageId", model.LanguageId);
                dictionary.Add("pNewsId", model.NewsId);
                dictionary.Add("pNewsTypeId", model.NewsTypeId);
                dictionary.Add("pNewsTitle", model.NewsTitle);
                dictionary.Add("pShortDescription", model.ShortDescription);
                dictionary.Add("pNewsDesc", model.NewsDesc);
                dictionary.Add("pDepartment", model.Department);
                dictionary.Add("pPost", model.Post);
                dictionary.Add("pNewsBy", model.NewsBy);
                dictionary.Add("pNewsStartDate", model.NewsStartDate);
                dictionary.Add("pNewsEndDate", model.NewsEndDate);
                dictionary.Add("pImageName", model.ImageName);
                dictionary.Add("pImagePath", model.ImagePath);
                dictionary.Add("pLocation", model.Location);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pMetaTitle", model.MetaTitle);
                dictionary.Add("pMetaDescription", model.MetaDescription);
                dictionary.Add("pUsername", username);
                dictionary.Add("pIsLink", model.IsLink);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateNewsMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateNewsMaster", ex.ToString(), "NewsMasterService", "AddOrUpdate");
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
        ~NewsMasterService()
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
