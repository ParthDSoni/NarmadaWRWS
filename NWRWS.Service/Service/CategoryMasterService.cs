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

namespace NWRWS.Services.Service
{
    public class CategoryMasterService : ICategoryServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public CategoryMasterService() 
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region PR Master
        public CategoryModel GetPRMaster(long id, long lgLangId = 1)
        {
            try
            {
                return GetPRMasterList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPRMasterLanguageId", ex.ToString(), "CategoryServiceService", "GetPRMaster");
                return null;
            }
        }
        public List<CategoryModel> GetPRMasterList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<CategoryModel>("GetAllPRMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllPRMasterLanguageId", ex.ToString(), "CategoryService", "GetPRMasterList");
                return null;
            }
        }
        public JsonResponseModel AddOrUpdatePRMaster(CategoryModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                if (string.IsNullOrWhiteSpace(model.FileName))
                {
                    model.FileName = "";
                }
                if (string.IsNullOrWhiteSpace(model.FilePath))
                {
                    model.FilePath = "";
                }
                if (model.Id != 0)
                {
                    var dataModel = Get(model.Id);
                    if (dataModel != null)
                    {
                        if (string.IsNullOrWhiteSpace(model.FilePath))
                        {
                            model.FileName = dataModel.FileName;
                        }
                    }
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("P_Id", model.Id);
                dictionary.Add("P_LanguageId", model.LanguageId);
                dictionary.Add("P_CategoryId", model.CategoryID);
                dictionary.Add("P_SubCategoryId", model.SubCategoryID);
                dictionary.Add("P_Title", model.Title);
                dictionary.Add("P_FileName", model.FileName);
                dictionary.Add("P_FilePath", model.FilePath);
                dictionary.Add("IsActive", model.IsActive);
                dictionary.Add("Username", username);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdatePRMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (model.Id == 0)
                {
                    jsonResponseModel.strMessage = "Record inserted success";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
                }
                else
                {
                    jsonResponseModel.strMessage = "Record updated success";
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
                dictionary.Add("P_Id", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<CategoryModel>("RemovePRMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record Removed success";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemovePRMaster", ex.ToString(), "CategoryService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
        #endregion

        #region Category 
        public CategoryModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCategoryMasterLanguageId", ex.ToString(), "PRMasterService", "Get");
                return null;
            }
        } 

        public List<CategoryModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<CategoryModel>("GetAllCategoryMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                 
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCategoryMasterLanguageId", ex.ToString(), "CategoryService", "GetList");
                return null;
            }
        }
        #endregion

        #region SubCategory

        public CategoryModel GetSubCategory(long id, long lgLangId = 1)
        {
            try
            {
                return GetList(lgLangId).Where(x => x.CategoryID == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllSubCategoryLanguageId", ex.ToString(), "PRMasterService", "Get");
                return null;
            }
        }
        public List<CategoryModel> GetSubList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<CategoryModel>("GetAllSubCategoryLanguageId", CommandType.StoredProcedure, dictionary).ToList();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllSubCategoryLanguageId", ex.ToString(), "SubCategoryService", "GetList");
                return null;
            }
        }
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~CategoryMasterService()
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
