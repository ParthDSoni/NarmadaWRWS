using AngleSharp.Css.Values;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.Services.Service;
using NWRWS.Webs.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace NWRWS.Webs.Controllers
{
    public class PublicationController : Controller
    {
        #region Controller Variable
        private IPublicationServices publicationService { get; set; }

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

        public PublicationController(IPublicationServices _publicationMasterService, IHttpClientFactory _httpClientFactory)
        {
            this.publicationService = _publicationMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindPublicationGrid")]
        public JsonResult BindPublicationGrid(int publicationTypeId, int currentPage)
        {

            PublicationAllDetails lstPublicationModelList = new PublicationAllDetails();
            try
            {
                int pageSize = 4;
                int skip = (currentPage - 1) * pageSize;
                lstPublicationModelList = publicationService.GetMultiListFront(LanguageId, pageSize, skip, publicationTypeId);
                double pageCount = (double)((decimal)lstPublicationModelList.recordsTotal / Convert.ToDecimal(pageSize));
                lstPublicationModelList.CurrentPage = currentPage;
                lstPublicationModelList.PageCount = pageCount;
                return Json(new { data = lstPublicationModelList.publicationDetails, currentPage = lstPublicationModelList.CurrentPage, pageCount = lstPublicationModelList.PageCount });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [Route("/GetViewCount")]
        [HttpPost]
        public async Task<JsonResult> GetViewCount(int PublicationId, int isDownload)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                var ipaddress = HttpContext.Connection.RemoteIpAddress.ToString();
                objreturn = publicationService.AddgetVisitorsCount(ipaddress, PublicationId, isDownload);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(objreturn);
        }

        [Route("/Publication/{publicationTypeId}")]
        public IActionResult Index(int publicationTypeId)
        {
            ViewBag.PublicationTypeId = publicationTypeId;
            return View();
        }
    }
}
