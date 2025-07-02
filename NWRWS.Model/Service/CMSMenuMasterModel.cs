namespace NWRWS.Model.Service
{
    public class ExamTypeDropDownModel
    {
        public long Id { get; set; }
        public string Examtype { get; set; }
        public string EmailIdSendTo { get; set; }
    }

    public class ExamDetailsModelPagging
    {
        public List<ExamDetailsModels> CivilListOfOfficerMasterModels { get; set; }
        public int PageCount { get; set; }
        public int TotalRec { get; set; }
        public int CurrentPageIASList { get; set; }

    }

    public class ExamDetailsModels
    {
        public long ExamtypeId { get; set; }
        public string Examtype { get; set; }
        public long ExamcategoryId { get; set; }
        public string Examcategory { get; set; }
        public string AdvertisementNo { get; set; }
        public string ExamName { get; set; }
        public string PublishDateCast { get; set; }
        public DateTime? PublishDate { get; set; }
        public string CategoryRemarks { get; set; }
        public string DocPath { get; set; }
        public string Note { get; set; }

        public string? TotalPosts { get; set; }
        public long AnswerKeyType { get; set; }
        public long Year { get; set; }
        public string DateOfAnnouncement { get; set; }
        public string ApplicationStartDate { get; set; }
        public string ApplicationEndDate { get; set; }
        public string TentativeExamDate { get; set; }
        public string TentativeMonthofResult { get; set; }


        public DateTime? DateOfAnnouncementDate { get; set; }
        public DateTime? ApplicationStartDateDate { get; set; }
        public DateTime? ApplicationEndDateDate { get; set; }
        public DateTime? TentativeExamDateDate { get; set; }
        public DateTime? TentativeMonthofResultDate { get; set; }

    }
    public class CMSMenuMasterModel
    {
        public long Id { get; set; }
        public long MenuResId { get; set; }
        public string MenuName { get; set; }
        public string MenuType { get; set; }
        public string PageType { get; set; }
        public long? ParentId { get; set; }
        public string ParentName { get; set; }
        public string PageDescription { get; set; }
        public long MenuRank { get; set; }
        public string MenuURL { get; set; }
        public bool IsActive { get; set; }
        public bool IsFullScreen { get; set; }
        public bool IsHomePage { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
    }
    public class AnswerKeyDetailsModelPagging
    {
        public List<AnswerKeyDetailsModels> CivilListOfOfficerMasterModels { get; set; }
        public int PageCount { get; set; }
        public int TotalRec { get; set; }
        public int CurrentPageIASList { get; set; }

    }
    public class AnswerKeyDetailsModels
    {
        public long ExamtypeId { get; set; }
        public string AdvertisementNo { get; set; }
        public string Examtype { get; set; }
        public string ExamName { get; set; }
        public string PublishDate { get; set; }
        public DateTime? PublishDateDate { get; set; }
        public string CategoryRemarks { get; set; }
        public string DocPath { get; set; }
        public string Answerkeyname { get; set; }
        public long Year { get; set; }

    }

    public class DocumentVerificationListModelPagging
    {
        public List<DocumentVerificationListDetailsModels> CivilListOfOfficerMasterModels { get; set; }
        public int PageCount { get; set; }
        public int TotalRec { get; set; }
        public int CurrentPageIASList { get; set; }

    }
    public class DocumentVerificationListDetailsModels
    {
        public long ExamtypeId { get; set; }
        public string AdvertisementNo { get; set; }
        public string ExamName { get; set; }
        public DateTime? PublishDateDate { get; set; }
        public string PublishDate { get; set; }
        public string DocPath { get; set; }
        public string Examtype { get; set; }
        public string? Rulesname { get; set; }
       // public string Answerkeyname { get; set; }
        public long Year { get; set; }

    }


    public class NotificationListModelPagging
    {
        public List<NotificationListDetailsModels> CivilListOfOfficerMasterModels { get; set; }
        public int PageCount { get; set; }
        public int TotalRec { get; set; }
        public int CurrentPageIASList { get; set; }

    }
    public class NotificationListDetailsModels
    {
        public long ExamtypeId { get; set; }
        public string AdvertisementNo { get; set; }
        public string ExamName { get; set; }
        public DateTime? PublishDateDate { get; set; }
        public string PublishDate { get; set; }
        public string DocPath { get; set; }
        public string Examtype { get; set; }
        public string CategoryRemarks { get; set; }
        // public string Answerkeyname { get; set; }
        public long Year { get; set; }

    }
    public class ResultListModelPagging
    {
        public List<ResultListDetailsModels> CivilListOfOfficerMasterModels { get; set; }
        public int PageCount { get; set; }
        public int TotalRec { get; set; }
        public int CurrentPageIASList { get; set; }

    }
    public class ResultListDetailsModels
    {
        public long ExamtypeId { get; set; }
        public string AdvertisementNo { get; set; }
        public string ExamName { get; set; }
        public string PublishDate { get; set; }
        public DateTime? PublishDateDate { get; set; }
        public string CategoryRemarks { get; set; }
        public string DocPath { get; set; }
        public string Examtype { get; set; }
        // public string Answerkeyname { get; set; }
        public long Year { get; set; }

    }
    public class SyllabusSchemesDetailsModelPagging
    {
        public List<SyllabusSchemesDetailsModels> CivilListOfOfficerMasterModels { get; set; }
        public int PageCount { get; set; }
        public int TotalRec { get; set; }
        public int CurrentPageIASList { get; set; }

    }
    public class SyllabusSchemesDetailsModels
    {
        public long ExamtypeId { get; set; }
        public string AdvertisementNo { get; set; }
        public string ExamName { get; set; }
        public string PublishDate { get; set; }
        public string DocPath { get; set; }
        public string Examtype { get; set; }
        public DateTime? PublishDateDate { get; set; }
        public string CategoryRemarks { get; set; }
        public long Year { get; set; }

    }
}
