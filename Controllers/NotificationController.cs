using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace NWRWS.Webs.Controllers
{
    public class NotificationController : Controller
    {
        #region Controller Variable

        protected readonly IBannerService objBannerService;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly ICMSTemplateMasterService objCMSTemplateMasterService;
        protected readonly IEcitizenMasterService objEcitizenMasterService;


        public long LanguageId
        {
            get
            {
                long Lang = 1;
                if (SessionWrapper.Get<long>(this.HttpContext.Session, "LanguageId") == null || SessionWrapper.Get<long>(this.HttpContext.Session, "LanguageId") == 0)
                {
                    SessionWrapper.Set<long>(this.HttpContext.Session, "LanguageId", 1);
                    Lang = 1;
                }
                else
                {
                    Lang = SessionWrapper.Get<long>(this.HttpContext.Session, "LanguageId");
                }
                return Lang;
            }
            set { SessionWrapper.Set<long>(this.HttpContext.Session, "LanguageId", value); }
        }

        #endregion

        #region Controller Constructor

        public NotificationController(IHttpClientFactory _httpClientFactory, ICMSTemplateMasterService _objCMSTemplateMasterService, IEcitizenMasterService _objEcitizenMasterService)
        {
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objEcitizenMasterService = _objEcitizenMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindNotificationGrid")]
        public JsonResult BindNotificationGrid(string? grno,string? Title,DateTime? date, DateTime? date1)
        {
            List<EcitizenModel> lstNewsModelList = new List<EcitizenModel> ();
            try
            {
                lstNewsModelList = objEcitizenMasterService.GetListFront(LanguageId).Where(x => x.EcitizenTypeId == "4").ToList();

                if (grno != null)
                {
                    lstNewsModelList = lstNewsModelList.Where(x => x.GRNumber.ToLower().Contains(grno.ToLower())).ToList();
                }
                if (Title != null)
                {
                    lstNewsModelList = lstNewsModelList.Where(x => x.EcitizenTitle.ToLower().Contains(Title.ToLower())).ToList();
                }
                if (date != null && date1 != null)
                {
                    lstNewsModelList = lstNewsModelList.Where(x => x.EcitizenStartDate >= date && x.EcitizenStartDate <= date1).ToList();
                }
                return Json(lstNewsModelList);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        
        [Route("/Notification")]
        public IActionResult Notification(int publicationTypeId)
        {
            return View();
        }
    }
}
