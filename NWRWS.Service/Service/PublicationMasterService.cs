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
    public class PublicationMasterService : IPublicationServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public PublicationMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public PublicationMasterModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallPublicationmasterlanguageid", ex.ToString(), "PublicationMasterService", "Get");
                return null;
            }
        }

        public JsonResponseModel AddgetVisitorsCount(string ipaddress,int PublicationId, int isDownload)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("p_IPAddress", ipaddress);
                dictionary.Add("p_PublicationId", PublicationId);
                dictionary.Add("p_isDownload", isDownload);
                jsonResponseModel.result = dapperConnection.GetListResult<PublicationMasterModel>("viewvisitorcount", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into WebSiteVisitorsCount", ex.ToString(), "CMSMenuMasterService", "AddgetVisitorsCount");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public PublicationMasterModel GetMenuRes(long PublicationId, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.PublicationId == PublicationId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "PublicationMasterService", "Get");
                return null;
            }
        }

        public List<PublicationMasterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<PublicationMasterModel>("getallPublicationmasterlanguageid", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.PublicationId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallPublicationmasterlanguageid", ex.ToString(), "PublicationMasterService", "GetList");
                return null;
            }
        }

        public List<PublicationTypeMasterModel> GetListPublicationType()
        {
            try
            {
                return dapperConnection.GetListResult<PublicationTypeMasterModel>("getPublicationtype", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getPublicationtype", ex.ToString(), "PublicationMasterService", "GetListLanguage");
                return null;
            }
        }

        public PublicationAllDetails GetMultiListFront(long lgLangId = 1, int pageSize = 5, int skip = 0, int publicationTypeId = 2)
        {
            try
            {
                var dictionary = new DynamicParameters();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("ppageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("ppublicationtypeid", publicationTypeId);

                var data = (PublicationAllDetails)dapperConnection.GetmultiTableObjectP("getallpublicationlanguageidfront", CommandType.StoredProcedure, dictionary);
                data.publicationDetails.ForEach(x =>
                {
                    x.Id = (long)x.PublicationId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallpublicationlanguageidfront", ex.ToString(), "NewsMasterService", "GetMultiListFront");
                return null;
            }
        }

        public List<PublicationMasterModel> GetListFront(long lgLangId = 1, int pageSize = 5, int skip = 0, int publicationTypeId = 2)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("ppageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("ppublicationtypeid", publicationTypeId);


                var data = dapperConnection.GetListResult<PublicationMasterModel>("getallpublicationlanguageidfront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.PublicationId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallpublicationlanguageidfront", ex.ToString(), "PublicationMasterService", "GetListFront");
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
                dapperConnection.GetListResult<PublicationMasterModel>("removePublicationmaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into removePublicationmaster", ex.ToString(), "PublicationMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(PublicationMasterModel model, string username)
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
                if (string.IsNullOrWhiteSpace(model.CoverPhotoPath))
                {
                    model.CoverPhotoPath = "";
                }
                if (string.IsNullOrWhiteSpace(model.CoverPhotoName))
                {
                    model.CoverPhotoName = "";
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
                        if (string.IsNullOrWhiteSpace(model.CoverPhotoPath))
                        {
                            model.CoverPhotoName = dataModel.CoverPhotoName;
                            model.CoverPhotoPath = dataModel.CoverPhotoPath;
                        }

                    }
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pid", model.Id);
                dictionary.Add("planguageid", model.LanguageId);
                dictionary.Add("pPublicationId", model.PublicationId);
                dictionary.Add("pPublicationTypeId", model.PublicationTypeId);
                dictionary.Add("pPublicationTitle", model.PublicationTitle);
                dictionary.Add("pPublicationDesc", model.PublicationDesc);
                dictionary.Add("pPublicationDate", model.PublicationDate);
                dictionary.Add("pCoverPhotoName", model.CoverPhotoName);
                dictionary.Add("pCoverPhotoPath", model.CoverPhotoPath);
                dictionary.Add("pimagename", model.ImageName);
                dictionary.Add("pimagepath", model.ImagePath);
                dictionary.Add("pisactive", model.IsActive);
                dictionary.Add("pmetatitle", model.MetaTitle);
                dictionary.Add("pmetadescription", model.MetaDescription);
                dictionary.Add("pusername", username);

                var data = dapperConnection.GetListResult<long>("insertorupdatePublicationmaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into insertorupdatePublicationmaster", ex.ToString(), "PublicationMasterService", "AddOrUpdate");
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
        ~PublicationMasterService()
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
