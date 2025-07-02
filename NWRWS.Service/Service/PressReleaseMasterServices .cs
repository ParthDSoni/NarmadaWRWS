using Dapper;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;
using System.Drawing.Printing;

namespace NWRWS.Services.Service
{
    public class PressReleaseMasterService : IPressMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public PressReleaseMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public PressReleaseMasterModel Get(long lgid, long lgLangId = 1)
        {
            try
            {

                return GetList(lgLangId).Where(x => x.Id == lgid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallpressreleaselanguageid", ex.ToString(), "PressReleaseMasterService", "Get");
                return null;
            }
        }
        public List<PressReleaseMasterModel> GetListById(long lgid, long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLangId", lgLangId);
                dictionary.Add("pid", lgid);
                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getallpressreleasebyid", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallpressreleasebyid", ex.ToString(), "PressReleaseMasterService", "Get");
                return null;
            }
        }
        public JsonResponseModel AddgetVisitorsCount(string ipaddress, long LanguageId, long PressId)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("p_IPAddress", ipaddress);
                dictionary.Add("p_LanguageId", LanguageId);
                dictionary.Add("p_PressId", PressId);
                jsonResponseModel.result = dapperConnection.GetListResult<PressVisitModel>("insertpressreleasecount", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into WebSiteVisitorsCount", ex.ToString(), "CMSMenuMasterService", "insertpressreleasecount");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
        public PressReleaseMasterModel GetById(long lgLangId = 1, long id = 1, long? DepId = null, long? DistrictId = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLangId", lgLangId);
                dictionary.Add("pid", id);
                dictionary.Add("pdepid", DepId);
                dictionary.Add("pdistid", DistrictId);

                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getpressbyid", CommandType.StoredProcedure, dictionary).FirstOrDefault();
                
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallpressreleaselanguageid", ex.ToString(), "PressReleaseMasterService", "Get");
                return null;
            }
        }

        public PressAllDetails GetListM(long lgLangId = 1, int pageSize = 10, int skip = 0, string? searchValue = null, string? createdby = null)
        {
            try
            {
                var dictionary = new DynamicParameters();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("ppageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("search_term", searchValue);
                dictionary.Add("pcreatedby", createdby);
                var data = (PressAllDetails)dapperConnection.GetmultiTableObjectPress("getallpressreleaselanguageid", CommandType.StoredProcedure, dictionary);
                data.pressDetails.ForEach(x =>
                {
                    x.Id = (long)x.PressId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "TenderMasterService", "GetList");
                return null;
            }
        }

        public List<PressReleaseMasterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getallpressreleaselanguageid", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.PressId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "PressReleaseMasterService", "GetList");
                return null;
            }
        }

        public List<PressReleaseMasterModel> GetListFront(long lgLangId = 1, int? DepartmentName = 1, int? DistrictName = 1, DateTime? date = null, DateTime? date1 = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("pdepid", DepartmentName);
                dictionary.Add("pdisid", DistrictName);
                dictionary.Add("pstartdate", date);
                dictionary.Add("penddate", date1);

                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getallpressreleaselanguageidfront", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.Id = (long)x.PressId;
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<PressReleaseMasterModel> GetListReport()
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
             

                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("GetPressReleaseDataSearchReport", CommandType.StoredProcedure, dictionary).ToList();
               
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<PressReleaseMasterModel> GetListCMFront(long lgLangId = 1, int? DepartmentName=null , int? DistrictName= null , DateTime? date = null, DateTime? date1 = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("pdepid", DepartmentName);
                dictionary.Add("pdisid", DistrictName);
                dictionary.Add("pstartdate", date);
                dictionary.Add("penddate", date1);

                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getallpressreleaselanguageidfront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.PressId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<PressReleaseMasterModel> GetListDepartmentReleaseFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);

                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getalldepartmentreleasefront", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.Id = (long)x.PressId;
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }

        public List<PressReleaseMasterModel> GetListDistrictReleaseFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);

                var data = dapperConnection.GetListResult<PressReleaseMasterModel>("getalldistrictreleasefront", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.Id = (long)x.PressId;
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }

        public List<DistrictMaster> GetDistrictList()
        {
            try
            {
                var data = dapperConnection.GetListResult<DistrictMaster>("getalldistrictmaster", CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldistrictmaster", ex.ToString(), "PressReleaseMasterService", "GetList");
                return null;
            }
        }

        public List<DepartmentMaster> GetDepartmentList()
        {
            try
            {
                var data = dapperConnection.GetListResult<DepartmentMaster>("getalldepartmentmaster", CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldepartmentmaster", ex.ToString(), "PressReleaseMasterService", "GetList");
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
                dapperConnection.GetListResult<PressReleaseMasterModel>("Removepressreleasemaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record Removed success";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into Removepressreleasemaster", ex.ToString(), "removeecitizenmaster", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddOrUpdate(PressReleaseMasterModel model, string username)
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
                dictionary.Add("planguageid", model.LanguageId);
                dictionary.Add("pPressId", model.PressId);
                dictionary.Add("pPresstitle", model.PressTitle);
                dictionary.Add("pPressDesc", model.PressDesc);
                dictionary.Add("pPressSubTitle", model.PressSubTitle);
                dictionary.Add("pPressMultiTitle", model.PressMultiTitle);
                dictionary.Add("pPressReleaseDate", model.PressReleaseDate);
                dictionary.Add("pimagename", model.ImageName);
                dictionary.Add("pimagepath", model.ImagePath);
                dictionary.Add("pisactive", model.IsActive);
                dictionary.Add("pmetatitle", model.MetaTitle);
                dictionary.Add("pmetadescription", model.MetaDescription);
                dictionary.Add("pdepartmentids", model.SelectedDepartement);
                dictionary.Add("pdistrictids", model.SelectedDistricts);
                dictionary.Add("pCMRelease", model.CMRelease);
                dictionary.Add("pusername", username);
                dictionary.Add("pPresstime",model.Presstime);
                dictionary.Add("pHighPress", model.HighPress);
                dictionary.Add("pEndDate", model.EndDate);
                dictionary.Add("pStartDate", model.StartDate);
                dictionary.Add("pCKImageName", model.CKImageName);
                dictionary.Add("pCKImagePath", model.CKImagePath);
                dictionary.Add("pViewImagePath", model.ViewImagePath);


                var data = dapperConnection.GetListResult<long>("insertorupdatepressreleasemasterjson", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into insertorupdatepressreleasemasterjson", ex.ToString(), "PressReleaseMasterService", "AddOrUpdate");
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
        ~PressReleaseMasterService()
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
