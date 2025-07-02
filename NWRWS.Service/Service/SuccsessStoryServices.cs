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
    public class SuccsessStoryServices : ISuccsessStoryServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion
        #region Constructor
        public SuccsessStoryServices()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion
        #region Public Method(s)
        public SuccsessStoryMasterModel Get(long id, long lgLangId)
        {
            try
            {
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(" Error Into GetAllBlogDetails", ex.ToString(), "GetAllBlogDetails", "Get");
                return null;
            }
        }
        public SuccsessStoryMasterModel GetMenuRes(long id, long lgLangId)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "GetAllBlogDetails", "Get");
                return null;
            }
        }
        #endregion
        public List<SuccsessStoryMasterModel> GetList(long lgLangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                return dapperConnection.GetListResult<SuccsessStoryMasterModel>("GetAllsuccsessstorydetailsById", CommandType.StoredProcedure, dictionary).ToList();
                //return dapperConnection.GetListResult<BlogMasterModel>("GetAllBlogDetailsById", CommandType.StoredProcedure).ToList();
            }

            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllsuccsessstorydetailsById", ex.ToString(), "GetAllsuccsessstorydetailsById", "GetList");
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
                dapperConnection.GetListResult<SuccsessStoryMasterModel>("Removesuccsessstory", CommandType.StoredProcedure, dictionary).ToList();
                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into Removesuccsessstory", ex.ToString(), "Removesuccsessstory", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(SuccsessStoryMasterModel model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            {
                try
                {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("Id", model.Id);
                    dictionary.Add("LanguageId", model.LanguageId);
                    dictionary.Add("Title", model.Title);
                    dictionary.Add("Date", model.Date);
                    dictionary.Add("Details", model.Details);
                    dictionary.Add("IsActive", model.IsActive);
                 
                    dictionary.Add("Username", model.Username);
                    var data = dapperConnection.GetListResult<int>("InsertOrUpdatesuccsessstorydetails", CommandType.StoredProcedure, dictionary).FirstOrDefault();
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
                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into succsessstorydetails", ex.ToString(), "InsertOrUpdatesuccsessstorydetails", "AddOrUpdate");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }
        #region Disposing Method(s)
        private bool disposed;
        ~SuccsessStoryServices()
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
