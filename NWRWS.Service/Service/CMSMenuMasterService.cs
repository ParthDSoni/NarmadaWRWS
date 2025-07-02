
using NWRWS.Common;
using NWRWS.Model.Service;
using System.Data;
using NWRWS.IService.Service;
using NWRWS.Model.System;
using System.Transactions;

namespace NWRWS.Services.Service
{
    public class CMSMenuMasterService : ICMSMenuMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public CMSMenuMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public CMSMenuMasterModel Get(long id)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuMaster", ex.ToString(), "CMSMenuMasterService", "Get");
                return null;
            }
        }

        public List<ExamTypeDropDownModel> GetExamTypeList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamTypeDropDownModel>("GetAllExamTypeDetailByLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuMaster", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        public string GetSendemailtoByLanguageId(long LangId, long examtypeid, string AdvertisementNo)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                dictionary.Add("pexamtypeid", examtypeid);
                dictionary.Add("pAdvertisementNo", AdvertisementNo);
                var data = dapperConnection.GetListResult<string>("GetSendemailtoByLanguageId", CommandType.StoredProcedure, dictionary).FirstOrDefault();
               
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetSendemailtoByLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }

        

        public List<ExamDetailsModels> GetExamDetailList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllExamDetailByLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllExamDetailByLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        public List<ExamDetailsModels> GetAllAdvertisementDetailList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllAdvertisementDetailList", CommandType.StoredProcedure, dictionary).ToList();
               
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdvertisementDetailList", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }

        
        public List<ExamDetailsModels> GetExamCalenderDetailList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllExamCalenderDetailByLanguageId", CommandType.StoredProcedure, dictionary).ToList();              
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllExamCalenderDetailByLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        
        public List<ExamDetailsModels> GetExamDetailListForPH(long LangId, long ddlExam, long FormTypeId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                dictionary.Add("pddlExam", ddlExam);
                dictionary.Add("pFormTypeId", FormTypeId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllExamAdvertisementNoByLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllExamAdvertisementNoByLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        
        public List<DocumentVerificationListDetailsModels> GetRecruitmentRulesList(long LangId,long rulestype)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                dictionary.Add("prulestype", rulestype);
                var data = dapperConnection.GetListResult<DocumentVerificationListDetailsModels>("GetAllRecruitmentRulesListLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllRecruitmentRulesListLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        
        public List<ExamDetailsModels> GetUpcomingExamsList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllUpcomingExamsByLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllUpcomingExamsByLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        public List<ExamDetailsModels> GetActiveExamsList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllActiveExamByLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllActiveExamByLanguageId", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        

        public List<AnswerKeyDetailsModels> GetAnswerKeyDetailList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<AnswerKeyDetailsModels>("GetAllExamDetailByLanguageIdForAnswerKey", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllExamDetailByLanguageIdForAnswerKey", ex.ToString(), "CMSMenuMasterService", "GetAnswerKeyDetailList");
                return null;
            }
        }
        public List<DocumentVerificationListDetailsModels> GetDocumentVerificationList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<DocumentVerificationListDetailsModels>("GetAllDocumentVerificationListLanguageId", CommandType.StoredProcedure, dictionary).ToList();
               
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllDocumentVerificationListLanguageId", ex.ToString(), "CMSMenuMasterService", "GetAnswerKeyDetailList");
                return null;
            }
        }
        public List<NotificationListDetailsModels> GetNotificationList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<NotificationListDetailsModels>("GetAllNotificationsListLanguageId", CommandType.StoredProcedure, dictionary).ToList();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNotificationsListLanguageId", ex.ToString(), "CMSMenuMasterService", "GetAnswerKeyDetailList");
                return null;
            }
        }
        public List<ResultListDetailsModels> GetResultList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ResultListDetailsModels>("GetResultListLanguageId", CommandType.StoredProcedure, dictionary).ToList();

                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetResultListLanguageId", ex.ToString(), "CMSMenuMasterService", "GetAnswerKeyDetailList");
                return null;
            }
        }

        public List<SyllabusSchemesDetailsModels> GetSyllabusSchemesDetailList(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<SyllabusSchemesDetailsModels>("GetAllSyllabusSchemesListLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllExamDetailByLanguageIdForAnswerKey", ex.ToString(), "CMSMenuMasterService", "GetAnswerKeyDetailList");
                return null;
            }
        }
        public List<ExamDetailsModels> GetExamDetailListForExamCategory(long LangId)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", LangId);
                var data = dapperConnection.GetListResult<ExamDetailsModels>("GetAllExamDetailByLanguageIdForExamCategory", CommandType.StoredProcedure, dictionary).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuMaster", ex.ToString(), "CMSMenuMasterService", "GetList");
                return null;
            }
        }
        public List<CMSMenuMasterModel> GetList()
        {
            try
            {
                var data = dapperConnection.GetListResult<CMSMenuMasterModel>("GetAllCMSMenuMaster", CommandType.StoredProcedure).ToList();
                //data.ForEach(x =>
                //{
                //    x.IsHomePageChange = (x.IsHomePage == 1 ? true : false);
                //});
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuMaster", ex.ToString(), "CMSMenuMasterService", "GetList");
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
                dictionary.Add("ChangedBy", username);
                dapperConnection.GetListResult<CMSMenuMasterModel>("RemoveCMSMenuMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveCMSMenuMaster", ex.ToString(), "CMSMenuMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel SetHomePageInCMSMenu(long id)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("MenuId", id);
                dapperConnection.GetListResult<CMSMenuMasterModel>("SetHomePageInCMSMenu", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "CMS Menu Is Set As Default successfully.";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into SetHomePageInCMSMenu", ex.ToString(), "CMSMenuMasterService", "SetHomePageInCMSMenu");
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

                    CMSMenuMasterModel masterModel = getDetails.Where(x => x.MenuRank == rank).FirstOrDefault();

                    var cuurentLevelList = getDetails.Where(x => x.ParentId == masterModel.ParentId).OrderBy(x => x.MenuRank).ToList();

                    long minValue= cuurentLevelList.Min(x=> x.MenuRank);
                    long maxValue= cuurentLevelList.Max(x=> x.MenuRank);

                    long updateRank = 0;

                    if (dir == "up" && (rank - 1) < minValue)
                    {
                        jsonResponseModel.strMessage = "This Menu already have min rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else if (dir == "down" && (rank+1) > maxValue)
                    {
                        jsonResponseModel.strMessage = "This Menu already have max rank!";
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else
                    {
                        var indexList=cuurentLevelList.Select((x, i) => new
                        {
                            item = x,
                            index = i
                        }).ToList();

                        foreach (var cuurentLevel in indexList)
                        {
                            if (dir == "up" && cuurentLevel.item.MenuRank== rank)
                            {
                                updateRank = indexList.Where(x=> x.index== (cuurentLevel.index-1)).FirstOrDefault().item.MenuRank;
                                break;
                            }
                            else if (dir == "down" && cuurentLevel.item.MenuRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index + 1)).FirstOrDefault().item.MenuRank;
                                break;
                            }
                        }


                        CMSMenuMasterModel masterupdateRankModel = getDetails.Where(x => x.MenuRank == updateRank).FirstOrDefault();

                        if (masterModel != null && masterupdateRankModel != null)
                        {
                            masterModel.MenuRank=updateRank;
                            masterupdateRankModel.MenuRank=rank;
                            jsonResponseModel = AddOrUpdate(masterModel);
                            jsonResponseModel = AddOrUpdate(masterupdateRankModel);

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

        public JsonResponseModel AddOrUpdate(CMSMenuMasterModel model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("MenuResId", model.MenuResId);
                dictionary.Add("MenuName", model.MenuName);
                dictionary.Add("MenuType", model.MenuType);
                dictionary.Add("MenuRank", model.MenuRank);
                dictionary.Add("ParentId", model.ParentId);
                dictionary.Add("IsActive", model.IsActive);
                dictionary.Add("ChangedBy", model.CreatedBy);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateCMSMenuMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateCMSMenuMaster", ex.ToString(), "CMSMenuMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel AddgetVisitorsCount(string ipaddress)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("p_IPAddress", ipaddress);
                jsonResponseModel.result = dapperConnection.GetListResult<VisitorsCountModelresponse>("WebSiteVisitorsCount", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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



        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~CMSMenuMasterService()
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
