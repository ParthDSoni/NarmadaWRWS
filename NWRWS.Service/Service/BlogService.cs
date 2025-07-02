using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.Services.Service

{
    public class BlogService : IBlogService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion
        #region Constructor
        public BlogService()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion
        #region Public Method(s)
        public BlogMasterModel Get(long id, long lgLangId)
        {
            try
            {
                return GetList(lgLangId).Where(x => x.BlogMasterId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(" Error Into GetAllBlogDetails", ex.ToString(), "GetAllBlogDetails", "Get");
                return null;
            }
        }

        public List<BlogMasterModel> GetList(long lgLangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                return dapperConnection.GetListResult<BlogMasterModel>("GetAllBlogDetailsById", CommandType.StoredProcedure, dictionary).ToList();
                //return dapperConnection.GetListResult<BlogMasterModel>("GetAllBlogDetailsById", CommandType.StoredProcedure).ToList();
            }

            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBlogDetailsById", ex.ToString(), "GetAllBlogDetailsById", "GetList");
                return null;
            }
        }

        public List<EventAchievementsImageModel> GetAchievementsImages(long lgLangId, long id)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pBlogMasterId", id);
                // dictionary.Add("pLanguageId", lgLangId);
                return dapperConnection.GetListResult<EventAchievementsImageModel>("GetAllAchievementsImages", CommandType.StoredProcedure, dictionary).ToList();
            }

            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllBlogDetailsById", ex.ToString(), "GetAllBlogDetailsById", "GetList");
                return null;
            }
        }

        //public List<EventAchievementsImageModel> GetAllFrontImagesById(long blogMasterId, long lgLangId)
        //{
        //    try
        //    {
        //        Dictionary<string, object> dictionary = new Dictionary<string, object>();
        //        dictionary.Add("pBlogMasterId", blogMasterId);
        //        dictionary.Add("pLanguageId", lgLangId);
        //        return dapperConnection.GetListResult<EventAchievementsImageModel>("GetAllFrontAchievementsImagesByID", CommandType.StoredProcedure, dictionary).ToList();
        //    }

        //    catch (Exception ex)
        //    {
        //        ErrorLogger.Error("Error Into GetAllBlogDetailsById", ex.ToString(), "GetAllBlogDetailsById", "GetList");
        //        return null;
        //    }
        //}

        public BlogMasterModel GetMenuRes(long id, long lgLangId)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.BlogMasterId == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "GetAllBlogDetails", "Get");
                return null;
            }
        }
        #endregion

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<BlogMasterModel>("RemoveBlogMaster", CommandType.StoredProcedure, dictionary).ToList();
                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into Removeblogmasterdetails", ex.ToString(), "blogmasterdetailsService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(BlogMasterModel model, List<EventAchievementsImageModel> lstGalleryImagesModels)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            {
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("pId", model.Id);
                    dictionary.Add("pLanguageId", model.LanguageId);
                    dictionary.Add("pBlogName", model.BlogName);
                    dictionary.Add("pDescription", model.Description);
                    dictionary.Add("pBlogMasterId", model.BlogMasterId);
                    dictionary.Add("pMetaTitle", model.MetaTitle);
                    dictionary.Add("pMetaDescription", model.MetaDescription);
                    dictionary.Add("pTypeId", model.TypeId);
                  //  dictionary.Add("BlogBy", model.BlogBy);
                    dictionary.Add("pBlogDate", model.BlogDate);
                    dictionary.Add("FileName", model.FileUpload);
                    dictionary.Add("FilePath", model.FilePath);
                    dictionary.Add("pIsActive", model.IsActive);
                    dictionary.Add("pLocation", model.Location);
                    dictionary.Add("pUsername", model.Username);
                    var data = dapperConnection.GetListResult<int>("InsertOrUpdateBlogMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();
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
                    model.Id = (int)data;

                    if (lstGalleryImagesModels != null)
                    {

                        Dictionary<string, object> dictionaryRemove = new Dictionary<string, object>();
                        dictionaryRemove.Add("pBlogMasterId", model.BlogMasterId);
                        dapperConnection.GetListResult<long>("RemoveAchievementsImages", CommandType.StoredProcedure, dictionaryRemove).FirstOrDefault();

                        foreach (var item in lstGalleryImagesModels.ToList())
                        {
                            Dictionary<string, object> dictionarySub = new Dictionary<string, object>();
                            dictionarySub.Add("pBlogMasterId", model.Id);
                            // dictionarySub.Add("pLanguageId", model.LanguageId);
                            dictionarySub.Add("pImageName", item.ImageName);
                            dictionarySub.Add("pImagePath", item.ImagePath);

                            dapperConnection.GetListResult<long>("InsertAchievementsImages", CommandType.StoredProcedure, dictionarySub).FirstOrDefault();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into InsertOrUpdateblogmasterdetails", ex.ToString(), "blogmasterdetailsService", "AddOrUpdate");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }
        #region Disposing Method(s)
        private bool disposed;
        ~BlogService()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                }
            }
            disposed = true;
        }
        #endregion

    }

}
