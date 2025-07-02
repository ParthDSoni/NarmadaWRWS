using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.Services.Service;
using NWRWS.Webs.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Xml.Linq;

namespace NWRWS.Webs.Controllers
{
    public class TenderController : Controller
    {
        #region Controller Variable
        private ITenderMasterService tenderMasterService { get; set; }

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

        public TenderController(ITenderMasterService _tenderMasterService, IHttpClientFactory _httpClientFactory)
        {
            this.tenderMasterService = _tenderMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindTenderGrid")]
        //public JsonResult BindTenderGrid(int? DepartmentName, int? DistrictName, DateTime? date, DateTime? date1)
        public JsonResult BindTenderGrid(string TenderSearchParam)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = HttpContext.Request.Form["start"].FirstOrDefault();
            var length = HttpContext.Request.Form["length"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;

            List<TenderMasterModelNew> lstNewsModelList = new List<TenderMasterModelNew>();
            TenderPageAllDetail pdetail = null;
            try
            {
                //pdetail = tenderMasterService.GetListFront(LanguageId, pageSize, skip, DepartmentName, DistrictName, date, date1);
                pdetail = tenderMasterService.GetListM(searchValue: TenderSearchParam);

                if (pdetail.tenderdetails.Count() == 0)
                {
                    lstNewsModelList = lstNewsModelList.ToList();
                }
                return Json(new { draw = draw, recordsFiltered = pdetail.totalcount[0].RecordsTotal, recordsTotal = pdetail.totalcount[0].RecordsTotal, data = pdetail.tenderdetails, upload = pdetail.uploadedCount });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [HttpGet]
        [Route("/GetTender")]
        public JsonResult GetTender()
        {
           try
            {
                var lsdata = tenderMasterService.GetTenderList();

                return Json(new { data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }
        }


        [HttpGet]
        [Route("/GetDistrictDataCheckBox")]
        public JsonResult GetDistrictDataCheckBox(long lgId)
        {
            List<DistrictMaster> lstDistrictModelList = new List<DistrictMaster>();
            try
            {
                var lsdata = tenderMasterService.GetDistrictList(lgId);

                return Json(new { data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }

        }


        [Route("/GetTenderById")]
        [IgnoreAntiforgeryToken]
        [HttpPost]
        public JsonResult GetTenderById(string id, string langId)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(NWRWS.Common.Functions.FrontDecrypt(HttpUtility.UrlDecode(id)), out long lgid) && long.TryParse(NWRWS.Common.Functions.FrontDecrypt(HttpUtility.UrlDecode(langId)), out long lgLangId))
                {
                    objreturn.strMessage = "";
                    objreturn.isError = false;
                    objreturn.result = tenderMasterService.Get(lgid, lgLangId);
                }
                else
                {
                    objreturn.strMessage = "Enter Valid Id.";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(objreturn);
        }
        
        

        [HttpGet]
        [Route("/GetDepartmentDataCheckBox")]
        public JsonResult GetDepartmentDataCheckBox(long lgId)
        {
            List<DepartmentMaster> lstDepartmentModelList = new List<DepartmentMaster>();
            try
            {
                var lsdata = tenderMasterService.GetDepartmentList(lgId);

                return Json(new { data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }
        }

        [HttpGet]
        [Route("/GetDistrictData")]
        public JsonResult GetDistrictData()
        {
            List<DistrictMaster> lstDistrictModelList = new List<DistrictMaster>();
            try
            {
                var lsdata = tenderMasterService.GetDistrictList(LanguageId);

                return Json(new { data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }

        }

        [HttpGet]
        [Route("/GetDepartmentData")]
        public JsonResult GetDepartmentData()
        {
            List<DepartmentMaster> lstDepartmentModelList = new List<DepartmentMaster>();
            try
            {
                var lsdata = tenderMasterService.GetDepartmentList(LanguageId);

                return Json(new { data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }
        }


        [Route("/Tenders")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
