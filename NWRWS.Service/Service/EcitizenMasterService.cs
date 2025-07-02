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
    public class EcitizenMasterService : IEcitizenMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public EcitizenMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public EcitizenModel Get(long id, long lgLangId = 1)
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
        public List<branch> GetListGRBranchFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", lgLangId);
                return dapperConnection.GetListResult<branch>("GetBranch", CommandType.StoredProcedure, dictionary).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getnewtype", ex.ToString(), "CMSMenuResourceMasterService", "GetListLanguage");
                return null;
            }
        }
        public EcitizenModel GetMenuRes(long NewsId, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.EcitizenId == NewsId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "CMSMenuResourceMasterService", "Get");
                return null;
            }
        }

        public List<EcitizenModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<EcitizenModel>("getallecitizenmasterlanguageid", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.EcitizenId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageId", ex.ToString(), "NewsMasterService", "GetList");
                return null;
            }
        }

        public List<EcitizenModel> GetListFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<EcitizenModel>("getallecitizenlanguageidforfront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.EcitizenId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<EcitizenModel> GetLatestUpdatesListFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<EcitizenModel>("GetAllLatestUpdatesLanguageIdForFront", CommandType.StoredProcedure, dictionary).ToList();
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

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<EcitizenModel>("removeecitizenmaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into removeecitizenmaster", ex.ToString(), "removeecitizenmaster", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(EcitizenModel model, string username)
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
                dictionary.Add("pecitizenid", model.EcitizenId);
                dictionary.Add("pecitizentypeid", model.EcitizenTypeId);
                dictionary.Add("padveid", model.AdvertisementId);
                dictionary.Add("pecitizentitle", model.EcitizenTitle);
                dictionary.Add("pShortDescription", model.ShortDescription);
                dictionary.Add("pecitizendesc", model.EcitizenDesc);
                dictionary.Add("pgrnumber", model.GRNumber);
                dictionary.Add("pbranch", model.Branch);
                dictionary.Add("pecitizenstartdate", model.EcitizenStartDate);
                dictionary.Add("pecitizenenddate", model.EcitizenEndDate);
                dictionary.Add("pimagename", model.ImageName);
                dictionary.Add("pimagepath", model.ImagePath);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pMetaTitle", model.MetaTitle);
                dictionary.Add("pMetaDescription", model.MetaDescription);
                dictionary.Add("pUsername", username);

                var data = dapperConnection.GetListResult<long>("insertorupdateecitizenmaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
        ~EcitizenMasterService()
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
