using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using Microsoft.AspNetCore.Mvc;

namespace NWRWS.Webs.Controllers
{
    public class WhatsNewController : Controller
    {
        #region Controller Variable

        protected readonly IBannerService objBannerService;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly ICMSTemplateMasterService objCMSTemplateMasterService;
        protected readonly INewsMasterService objNewsMasterService;


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

        public WhatsNewController(IHttpClientFactory _httpClientFactory, ICMSTemplateMasterService _objCMSTemplateMasterService, INewsMasterService _objNewsMasterService)
        {
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objNewsMasterService = _objNewsMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindWhatsNewGrid")]
        public JsonResult BindWhatsNewGrid()
        {
            try
            {
                List<NewsModel> lstNewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "2").ToList();

                if (lstNewsList.Count() == 0)
                {
                    lstNewsList = objNewsMasterService.GetListFront(1).Where(x => x.NewsTypeId == "2").ToList();
                }
                //else
                //{
                //    lstNewsList
                //}
                //lstNewsList.ForEach(x =>
                //{
                //    if (x.NewsStartDate != null)
                //    {
                //        x.NewsDate = x.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("-", "");

                //    }
                //});
                return Json(lstNewsList);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}


        [HttpPost]
        [Route("/BindWhatsPastEventGrid")]
        public JsonResult BindWhatsPastEventGrid()
        {

            try
            {
                List<NewsModel> lstNewsPastEventList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "P").ToList();

                if (lstNewsPastEventList.Count() <= 0)
                {
                    lstNewsPastEventList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "P").ToList();
                }
                lstNewsPastEventList.ForEach(x =>
                {
                    if (x.NewsStartDate != null)
                    {
                        x.NewsDate = x.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("-", "");

                    }
                });
                return Json(lstNewsPastEventList);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [HttpPost]
        [Route("/BindWhatsAudioGrid")]
        public JsonResult BindWhatsAudioGrid()
        {

            try
            {
                List<NewsModel> lstNewsAudioList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "A").ToList();

                if (lstNewsAudioList.Count() <= 0)
                {
                    lstNewsAudioList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "A").ToList();
                }
                lstNewsAudioList.ForEach(x =>
                {
                    if (x.NewsStartDate != null)
                    {
                        x.NewsDate = x.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("-", "");

                    }
                });
                return Json(lstNewsAudioList);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        public IActionResult Index(string type)
        {
            ViewBag.Type = type;
            return View();
        }
    }
}
