using Dapper;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;
using System.Drawing.Printing;

namespace NWRWS.Services.Service
{
    public class TenderMasterService : ITenderMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public TenderMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        //public TenderMasterModel Get(long id, long lgLangId = 1)
        //{
        //    try
        //    {
        //        return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.Error("Error Into getalltendermasterlanguageid", ex.ToString(), "TenderMasterService", "Get");
        //        return null;
        //    }
        //}
        public TenderMasterModelNew Get(long lgid, long lgLangId = 1)
        {
            try
            {
                List<TenderMasterModelNew> data = new List<TenderMasterModelNew>();
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
                dictionary.Add("pid", lgid);
                data = dapperConnection.GetListResult<TenderMasterModelNew>("NWRWS_gettenderbyid", CommandType.StoredProcedure, dictionary).ToList();
                return data[0];
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into gettenderbyid", ex.ToString(), "PressReleaseMasterService", "Get");
                return null;
            }
        }
        public List<TenderMasterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<TenderMasterModel>("getalltendermasterlanguageid", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.TenderId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "TenderMasterService", "GetList");
                return null;
            }
        }
        public TenderPageAllDetail GetListM(long lgLangId = 1, int pageSize = 10, int skip = 0, string? searchValue = null)
        {
            try
            {
                var dictionary = new DynamicParameters();
                //dictionary.Add("LangId", lgLangId);
                dictionary.Add("ppageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("search_term", searchValue);
                var data = (TenderPageAllDetail)dapperConnection.GetmultiTableObjectT("NWRWS_getalltendermasterlanguageid", CommandType.StoredProcedure, dictionary);
                data.tenderdetails.ForEach(x =>
                {
                    x.TenderId = (long)x.TenderId;
                    //x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), "TenderMasterService", "GetList");
                return null;
            }
        }

        public List<dynamic> GetTenderList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("plangId", lgLangId);
                var data = dapperConnection.GetListResult<dynamic>("GetAllTenders", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllTenders", ex.ToString(), "TenderMasterService", "GetTenderList");
                return null;
            }
        }

        public List<DistrictMaster> GetDistrictList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("plangId", lgLangId);
                var data = dapperConnection.GetListResult<DistrictMaster>("getalldistrictmaster", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldistrictmaster", ex.ToString(), "TenderMasterService", "GetList");
                return null;
            }
        }

        public List<DepartmentMaster> GetDepartmentList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("plangId", lgLangId);
                var data = dapperConnection.GetListResult<DepartmentMaster>("getalldepartmentmaster", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldepartmentmaster", ex.ToString(), "TenderMasterService", "GetList");
                return null;
            }
        }

        public TenderPageAllDetail GetListFront(long lgLangId = 1, int pageSize = 10, int skip = 0, int? DepartmentName = 1, int? DistrictName = 1, DateTime? date = null, DateTime? date1 = null)
        {
            try
            {
                var dictionary = new DynamicParameters();
                dictionary.Add("LangId", lgLangId);
                dictionary.Add("ppageSize", pageSize);
                dictionary.Add("pskip", skip);
                dictionary.Add("pdepid", DepartmentName);
                dictionary.Add("pdisid", DistrictName);
                dictionary.Add("pstartdate", date);
                dictionary.Add("penddate", date1);

                var data = (TenderPageAllDetail)dapperConnection.GetmultiTableObject("getalltenderlanguageidfront", CommandType.StoredProcedure, dictionary);
                data.tenderdetails.ForEach(x =>
                {
                    x.TenderId = (long)x.TenderId;
                    //x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageIdForFront", ex.ToString(), "NewsMasterService", "GetListFront");
                return null;
            }
        }
        public List<TenderCount> GetAllCount(int? DepartmentName = 1, int? DistrictName = 1, DateTime? date = null, DateTime? date1 = null)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pdepid", DepartmentName);
                dictionary.Add("pdisid", DistrictName);
                dictionary.Add("pstartdate", date);
                dictionary.Add("penddate", date1);
                var data = dapperConnection.GetListResult<TenderCount>("getalltendercount", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalltendercount", ex.ToString(), "NewsMasterService", "GetListFront");
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
                dapperConnection.GetListResult<TenderMasterModel>("NWRWS_removetendermaster", CommandType.StoredProcedure, dictionary).ToList();

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

        public JsonResponseModel AddOrUpdate(TenderMasterModel model, string username)
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
                    //if (dataModel != null)
                    //{
                    //    if (string.IsNullOrWhiteSpace(model.ImagePath))
                    //    {
                    //        model.ImageName = dataModel.ImageName;
                    //        model.ImagePath = dataModel.ImagePath;
                    //    }
                    //}
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pid", model.Id);
                dictionary.Add("planguageid", model.LanguageId);
                dictionary.Add("ptenderid", model.TenderId);
                //dictionary.Add("pdistrictid", model.DistrictId);
                //dictionary.Add("pdepartmentid", model.DepartmentId);
                dictionary.Add("ptendertitle", model.TenderTitle);
                dictionary.Add("ptenderdesc", model.TenderDesc);
                dictionary.Add("pofficeaddress", model.OfficeAddress);
                dictionary.Add("ptotalcost", model.TotalCost);
                dictionary.Add("ptenderstartdate", model.TenderStartDate);
                dictionary.Add("ptenderenddate", model.TenderEndDate);
                dictionary.Add("pimagename", model.ImageName);
                dictionary.Add("pimagepath", model.ImagePath);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pMetaTitle", model.MetaTitle);
                dictionary.Add("pMetaDescription", model.MetaDescription);
                dictionary.Add("pUsername", username);

                var data = dapperConnection.GetListResult<long>("insertorupdatetendermaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into insertorupdatetendermaster", ex.ToString(), "TenderMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }


        public JsonResponseModel AddOrUpdateNew(TenderMasterModelNew model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("pTenderId", model.TenderId);
                dictionary.Add("pTenderTitle", model.TenderTitle);
                dictionary.Add("pTenderNumber", model.TenderNumber);
                dictionary.Add("pTenderDetails", model.TenderDetails);
                dictionary.Add("pPreBidMeetingStartDate", model.PreBidMeetingStartDate);
                dictionary.Add("pPreBidMeetingEndDate", model.PreBidMeetingEndDate);
                dictionary.Add("pLastDateOfSubmition", model.LastDateOfSubmition);
                dictionary.Add("pOpeningBidDate", model.OpeningBidDate);
                dictionary.Add("pProjectEstimateCost", model.ProjectEstimateCost);
                dictionary.Add("pProjectFinalCost", model.ProjectFinalCost);
                dictionary.Add("pNameOfBidder", model.NameOfBidder);
                dictionary.Add("pWorkOrderIssueDate", model.WorkOrderIssueDate);
                dictionary.Add("pTenderDocuments", model.TenderDocumentData);
                dictionary.Add("pByUser", username);
                dictionary.Add("pIsActive", model.IsActive);


                var data = dapperConnection.GetListResult<long>("NWRWS_insertorupdatetendermaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (model.TenderId == 0)
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
                model.TenderId = (long)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into NWRWS_insertorupdatetendermaster", ex.ToString(), "TenderMasterService", "AddOrUpdateNew");
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
        ~TenderMasterService()
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
