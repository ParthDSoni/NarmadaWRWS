using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Services.Service;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Web;

namespace NWRWS.Webs.Controllers
{
    public class EarthquakeController : Controller
    {
        #region Controller Variable

        protected readonly IBannerService objBannerService;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly ICMSTemplateMasterService objCMSTemplateMasterService;
        protected readonly IEarthquakeMasterService objEarthquakeMasterService;

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

        public EarthquakeController(IHttpClientFactory _httpClientFactory, ICMSTemplateMasterService _objCMSTemplateMasterService, IEarthquakeMasterService _objEarthquakeMasterService)
        {
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objEarthquakeMasterService = _objEarthquakeMasterService;
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

        public IActionResult Index()
        {
            return View();
        }

        [Route("/Earthquake")]
        public IActionResult Earthquake()
        {
            return View();
        }

        [HttpPost]
        [Route("/BindEarthqaukeGridFront")]
        public JsonResult BindEarthqaukeGridFront(string FromMagnitude, string ToMagnitude, string FromDate, string ToDate)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            List<EarthquakeMasterModel> lstList = new List<EarthquakeMasterModel>();
            lstList = objEarthquakeMasterService.GetListFront().OrderByDescending(x => x.Date).ToList();

            try
            {
                float fromMagnitude;
                float toMagnitude;
                if (FromMagnitude != null && ToMagnitude != null)
                {

                    float.TryParse(FromMagnitude, out fromMagnitude);
                    float.TryParse(ToMagnitude, out toMagnitude);
                    lstList = lstList.Where(x =>x.Magnitude >=fromMagnitude && x.Magnitude <= toMagnitude).OrderByDescending(x => x.Date).ToList();
                }
                if (FromDate != null && ToDate != null)
                {
                    DateTime fromDateParsed, toDateParsed;
                    if (DateTime.TryParseExact(FromDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDateParsed) &&
                        DateTime.TryParseExact(ToDate, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDateParsed))
                    {
                        lstList = lstList.Where(x =>
                        {
                            DateTime dateValue;
                            return DateTime.TryParseExact(x.Date, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateValue) &&
                                   dateValue >= fromDateParsed && dateValue <= toDateParsed;
                        }).OrderByDescending(x => x.Date).ToList();
                    }
                    else
                    {
                        throw new Exception("Invalid FromDate or ToDate format.");
                    }
                }

                if (lstList.Count() == 0)
                {
                    lstList = lstList.ToList();
                }
                return Json(new { draw = draw, data = lstList });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
    }
}
