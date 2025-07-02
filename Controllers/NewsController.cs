using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace NWRWS.Webs.Controllers
{
    public class NewsController : Controller
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

        public NewsController(IHttpClientFactory _httpClientFactory, ICMSTemplateMasterService _objCMSTemplateMasterService, INewsMasterService _objNewsMasterService)
        {
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objNewsMasterService = _objNewsMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindImportantLinks")]
        public JsonResult BindImportantLinks()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "Important Links").FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json(HttpUtility.HtmlDecode(datas.Content));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json(HttpUtility.HtmlDecode(datas.Content));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [HttpPost]
        [Route("/BindNewsGrid")]
        public JsonResult BindNewsGrid()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                List<NewsModel> lstNewsList = objNewsMasterService.GetListFront(LanguageId).ToList();

                if (lstNewsList.Count() <= 0)
                {
                    lstNewsList = objNewsMasterService.GetListFront(LanguageId).ToList();
                }

                lstNewsList.ForEach(x =>
                {
                    if (x.NewsStartDate != null)
                    {
                        x.NewsDate = x.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("-", "");

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

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Resolution")]
        public IActionResult Resolution()
        {
            return View();
        }

        [HttpPost]
        [Route("/BindResolutionGrid")]
        public JsonResult BindResolutionGrid(string ResolutionSearchParam)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            List<NewsModel> lstNewsModelList = new List<NewsModel>();
            try
            {
                lstNewsModelList = objNewsMasterService.GetResolutionFront(searchValue: ResolutionSearchParam);

                if (lstNewsModelList.Count() == 0)
                {
                    lstNewsModelList = lstNewsModelList.ToList();
                }
                return Json(new { draw = draw, data = lstNewsModelList });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [Route("/Circular")]
        public IActionResult Circular()
        {
            return View();
        }

        [HttpPost]
        [Route("/BindCircularGrid")]
        public JsonResult BindCircularGrid(string CircularSearchParam)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            List<NewsModel> lstNewsModelList = new List<NewsModel>();
            try
            {
                lstNewsModelList = objNewsMasterService.GetCircularFront(searchValue: CircularSearchParam);

                if (lstNewsModelList.Count() == 0)
                {
                    lstNewsModelList = lstNewsModelList.ToList();
                }
                return Json(new { draw = draw, data = lstNewsModelList });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
        [Route("/News")]
        public IActionResult News()
        {
            return View();
        }

        [HttpPost]
        [Route("/BindNewsGridFront")]
        public JsonResult BindNewsGridFront(string NewsSearchParam)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            List<NewsModel> lstNewsModelList = new List<NewsModel>();
            try
            {
                lstNewsModelList = objNewsMasterService.GetNewsFront(searchValue: NewsSearchParam);

                if (lstNewsModelList.Count() == 0)
                {
                    lstNewsModelList = lstNewsModelList.ToList();
                }
                return Json(new { draw = draw, data = lstNewsModelList });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
    }
}
