using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface ICMSMenuMasterService : IDisposable
    {
        CMSMenuMasterModel Get(long id);

        List<CMSMenuMasterModel> GetList();

        List<ExamTypeDropDownModel> GetExamTypeList(long LangId);
        string GetSendemailtoByLanguageId(long LangId,long examtypeid, string AdvertisementNo);

        List<ExamDetailsModels> GetExamDetailList(long LangId);
        List<ExamDetailsModels> GetAllAdvertisementDetailList(long LangId);

        List<ExamDetailsModels> GetExamCalenderDetailList(long LangId);

        List<ExamDetailsModels> GetExamDetailListForPH(long LangId,long ddlExam, long FormTypeId);

        List<ExamDetailsModels> GetUpcomingExamsList(long LangId);

        List<ExamDetailsModels> GetActiveExamsList(long LangId);
        List<AnswerKeyDetailsModels> GetAnswerKeyDetailList(long LangId);

        List<DocumentVerificationListDetailsModels> GetDocumentVerificationList(long LangId);

        List<DocumentVerificationListDetailsModels> GetRecruitmentRulesList(long LangId,long rulestype);

        List<NotificationListDetailsModels> GetNotificationList(long LangId);
        List<ResultListDetailsModels> GetResultList(long LangId);
        List<SyllabusSchemesDetailsModels> GetSyllabusSchemesDetailList(long LangId);
        List<ExamDetailsModels> GetExamDetailListForExamCategory(long LangId);

        JsonResponseModel SetHomePageInCMSMenu(long id);

        JsonResponseModel SwapSequance(long rank, string dir, string username);

        JsonResponseModel Delete(long id, string username);

        JsonResponseModel AddOrUpdate(CMSMenuMasterModel model);

        JsonResponseModel AddgetVisitorsCount(string ipaddress);



    }
}
