using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using Microsoft.AspNetCore.Mvc;

namespace NWRWS.Webs.Controllers
{
    public class RightToInformationController : Controller
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

        public RightToInformationController(IHttpClientFactory _httpClientFactory, ICMSTemplateMasterService _objCMSTemplateMasterService, IEcitizenMasterService _objEcitizenMasterService)
        {
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objEcitizenMasterService = _objEcitizenMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindRTIGrid")]
        public JsonResult BindRTIGrid()
        {

            try
            {
                List<EcitizenModel> lstNewsList = objEcitizenMasterService.GetListFront(LanguageId).Where(x => x.EcitizenTypeId == "2").ToList();

                if (lstNewsList.Count() <= 0)
                {
                    lstNewsList = objEcitizenMasterService.GetListFront(LanguageId).Where(x => x.EcitizenTypeId == "2").ToList();
                }
                lstNewsList.ForEach(x =>
                {
                    if (x.EcitizenStartDate != null)
                    {
                        x.NewsDate = x.EcitizenStartDate.Value.ToString("dd/MM/yyyy").Replace("-", "");

                    }
                });
                return Json(lstNewsList);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        
        [Route("/RTI")]
        public IActionResult RTI()
        {
            //ViewBag.Type = type;
            return View();
        }
    }
}
