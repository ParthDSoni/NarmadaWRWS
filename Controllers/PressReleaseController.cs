using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.Services.Service;
using NWRWS.Webs.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;
using System.Net;
using System.Xml.Linq;

namespace NWRWS.Webs.Controllers
{
    public class PressReleaseController : Controller
    {
        #region Controller Variable
        private IPressMasterService pressMasterService { get; set; }

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

        public PressReleaseController(IPressMasterService _pressMasterService, IHttpClientFactory _httpClientFactory)
        {
            this.pressMasterService = _pressMasterService;
        }

        #endregion

        [Route("/Press-Releases/{isDistrict}")]
        public IActionResult PressRelease(int isDistict)
        {
            ViewBag.isDistict = isDistict;
            return View();
        }


        [Route("/GetPressViewCount")]
        [HttpPost]
        public JsonResult GetPressViewCount(long PressId)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                var ipaddress = HttpContext.Connection.RemoteIpAddress.ToString();
                objreturn = pressMasterService.AddgetVisitorsCount(ipaddress, LanguageId, PressId);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(objreturn);
        }

        [Route("/District-Releases")]
        public IActionResult DistrictPressRelease()
        {
            return View();
        }

        [Route("/Department-Releases")]
        public IActionResult DepartmentPressRelease()
        {
            return View();
        }

        [Route("/CMPressRelease")]
        public IActionResult CMPressRelease()
        {
            return View();
        }
        [Route("/PressReleaseDetails/")]
        public IActionResult PressReleaseDetails(int id, int DepId, int DistrictId, int LanguageId)
        {
            PressTempModel presstempmodel = new PressTempModel();
            try
            {
                presstempmodel.Id = id;
                presstempmodel.DepId = DepId;
                presstempmodel.DistrictId = DistrictId;
                if (LanguageId == 0)
                {
                    presstempmodel.lglaluId = 1;
                }
                else
                {
                    presstempmodel.lglaluId = LanguageId;
                }
                //presstempmodel.DepName = DepartmentName;
                //presstempmodel.DistrictName = DistrictName;
                return View(presstempmodel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
            return View(presstempmodel);
        }

        [HttpPost]
        [Route("/PressReleaseDetailsData")]
        public JsonResult PressReleaseDetailsData(int id, int? DepId, int? DistrictId, long LangId)
        {
            PressReleaseMasterModel lstPressReleaselist = new PressReleaseMasterModel();
            try
            {
                if (DepId == 0)
                {
                    DepId = null;
                }
                if (DistrictId == 0)
                {
                    DistrictId = null;
                }
                lstPressReleaselist = pressMasterService.GetById(LangId, id, DepId, DistrictId);

                return Json(lstPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [Route("/PressReleaseDetailsL/")]
        public IActionResult PressReleaseDetailsL(int id, int DepId, int DistrictId, int LanguageId)
        {
            PressTempModel presstempmodel = new PressTempModel();
            try
            {
                presstempmodel.Id = id;
                presstempmodel.DepId = DepId;
                presstempmodel.DistrictId = DistrictId;
                if (LanguageId == 0)
                {
                    presstempmodel.lglaluId = 1;
                }
                else
                {
                    presstempmodel.lglaluId = LanguageId;
                }
                //presstempmodel.DepName = DepartmentName;
                //presstempmodel.DistrictName = DistrictName;
                return View(presstempmodel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
            return View(presstempmodel);
        }

        [HttpPost]
        [Route("/PressReleaseDetailsDataL")]
        public JsonResult PressReleaseDetailsDataL(int id, int? DepId, int? DistrictId, long LangId)
        {
            PressReleaseMasterModel lstPressReleaselist = new PressReleaseMasterModel();
            try
            {
                if (DepId == 0)
                {
                    DepId = null;
                }
                if (DistrictId == 0)
                {
                    DistrictId = null;
                }
                lstPressReleaselist = pressMasterService.GetById(LangId, id, DepId, DistrictId);

                return Json(lstPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [HttpPost]
        [Route("/BindPressReleasesGrid")]
        public JsonResult BindPressReleasesGrid(int? DepartmentId, int? DistrictId, DateTime? date, DateTime? date1)
        {
            List<PressReleaseMasterModel> lstPressReleaselist = new List<PressReleaseMasterModel>();
            try
            {
                lstPressReleaselist = pressMasterService.GetListFront(LanguageId, DepartmentId, DistrictId, date, date1);

                return Json(lstPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
        [HttpPost]
        [Route("/BindCMPressReleasesGrid")]
        public JsonResult BindCMPressReleasesGrid(int? DepartmentId, int? DistrictId, DateTime? date, DateTime? date1)
        {
            List<PressReleaseMasterModel> lstPressReleaselist = new List<PressReleaseMasterModel>();
            try
            {
                lstPressReleaselist = pressMasterService.GetListFront(LanguageId, DepartmentId, DistrictId, date, date1).Where(x => x.CMRelease == true).ToList();

                return Json(lstPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [HttpPost]
        [Route("/BindDepartmentPressRealese")]
        public JsonResult BindDepartmentPressRealese()
        {
            List<PressReleaseMasterModel> lstDepartmentPressReleaselist = new List<PressReleaseMasterModel>();
            try
            {
                lstDepartmentPressReleaselist = pressMasterService.GetListDepartmentReleaseFront(LanguageId);

                return Json(lstDepartmentPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
        [HttpPost]
        [Route("/BindCMPressRealese")]
        public JsonResult BindCMPressRealese()
        {
            List<PressReleaseMasterModel> lstDepartmentPressReleaselist = new List<PressReleaseMasterModel>();
            try
            {
                lstDepartmentPressReleaselist = pressMasterService.GetListCMFront(LanguageId).Where(x => x.CMRelease == true && x.IsActive == true).ToList();

                return Json(lstDepartmentPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
        [HttpPost]
        [Route("/BindDistrictPressRealese")]
        public JsonResult BindDistrictPressRealese()
        {
            List<PressReleaseMasterModel> lstDistrictPressReleaselist = new List<PressReleaseMasterModel>();
            try
            {
                lstDistrictPressReleaselist = pressMasterService.GetListDistrictReleaseFront(LanguageId);

                return Json(lstDistrictPressReleaselist);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }
    }
}
