using NWRWS.Common;
using NWRWS.IService.Service;
using Microsoft.AspNetCore.Mvc;
using NWRWS.Model.System;
using NWRWS.Services.Service;

namespace NWRWS.Webs.Controllers
{
    public class FrontDropDownController : Controller
    {
        #region Controller Variable

        private readonly IRegionMasterService regionMasterService;
        private readonly IBasinMasterService basinMasterService;
        private readonly IRiverDamMasterService riverDamMasterService;
        private readonly ICanalMasterService CanalMasterService;

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

        public FrontDropDownController(IHttpClientFactory _httpClientFactory, IRegionMasterService regionMasterService, IBasinMasterService basinMasterService, IRiverDamMasterService riverDamMasterService, ICanalMasterService CanalMasterService)
        {
            this.riverDamMasterService = riverDamMasterService;
            this.regionMasterService = regionMasterService;
            this.basinMasterService = basinMasterService;
            this.CanalMasterService = CanalMasterService;
        }

        #endregion

        #region Controller Method

        [Route("/Dam")]
        public IActionResult Index(string? riverId, string? canalId, string? damId)
        {
            // You can use riverId, canalId, or damId here
            return View();
        }

        #region Dam Resources

        [Route("/RiverResources")]
        [Route("/DamResources")]
        [Route("/CanalResources")]
        public IActionResult CommonResources()
        {
            var path = HttpContext.Request.Path.Value?.ToLower();

            if(path.Contains("riverresources"))
            {
                ViewData["ScriptFile"] = "RiverResources.js";
                ViewData["Title"] = "River Resources";
            }
            else if (path.Contains("damresources"))
            {
                ViewData["ScriptFile"] = "DamResources.js";
                ViewData["Title"] = "Dam Resources";
            }
            else if (path.Contains("canalresources"))
            {
                ViewData["ScriptFile"] = "CanalResources.js";
                ViewData["Title"] = "Canal Resources";
            }

            return View();
        }

        [HttpPost("/GetAllDamDataTable")]
        public JsonResult GetAllDamDataTable(long regionId = 0, long basinId = 0)
        {
            try
            {
                var lsdata = riverDamMasterService.GetDamListF((int)LanguageId, regionId, basinId);
                return Json(lsdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        [Route("/GetDamById")]
        public JsonResult GetDamById(string damId)
        {
            try
            {
                long.TryParse(Functions.FrontDecrypt(damId), out long lgId);
                var lsdata = riverDamMasterService.GetDam(lgId, (int)LanguageId);
                return Json(lsdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        #endregion

        #region River Resources

        [HttpPost("/GetAllRiverDataTable")]
        public JsonResult GetAllRiverDataTable(long regionId = 0, long basinId = 0)
        {
            try
            {
                var lsdata = riverDamMasterService.GetRiverListF((int)LanguageId, regionId, basinId);
                return Json(lsdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        [Route("/GetRiverById")]
        public JsonResult GetRiverById(string riverId)
        {
            try
            {
                long.TryParse(Functions.FrontDecrypt(riverId), out long lgId);
                var lsdata = riverDamMasterService.GetRiver(lgId, (int)LanguageId);
                return Json(lsdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        #endregion

        #region Canal Resources

        [HttpPost("/GetAllCanalDataTable")]
        public JsonResult GetAllCanalDataTable(long regionId = 0, long basinId = 0)
        {
            try
            {
                var lsdata = CanalMasterService.GetCanalListF((int)LanguageId, regionId, basinId);
                return Json(lsdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        [Route("/GetCanalById")]
        public JsonResult GetCanalById(string canalId)
        {
            try
            {
                long.TryParse(Functions.FrontDecrypt(canalId), out long lgId);
                var lsdata = CanalMasterService.GetCanal(lgId, (int)LanguageId);
                return Json(lsdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        #endregion

        [HttpPost("/GetAllRegionFront")]
        public JsonResult GetAllRegionFront(int languageid = 1)
        {
            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.AddRange(regionMasterService.GetListF(languageid).Select(x => new ListItem { Text = x.Name, Value = x.RegionId.ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(lsdata);
        }

        [HttpPost("/GetAllBasinFront")]
        public JsonResult GetAllBasinFront(int languageid = 1, long regionId = 1)
        {
            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.AddRange(basinMasterService.GetListF(languageid, regionId).Select(x => new ListItem { Text = x.Name, Value = x.BasinId.ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(lsdata);
        }

        [HttpPost("/GetAllDamFront")]
        public JsonResult GetAllDamFront(int languageid = 1, long regionId = 0, long basinId = 0)
        {
            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.AddRange(riverDamMasterService.GetDamListF(languageid, regionId, basinId).Select(x => new ListItem { Text = x.Name, Value = x.DamId.ToString(), UploadDocumentPath = x.UploadDocumentPath, Description = x.Description }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(lsdata);
        }

        [HttpPost("/GetAllRiverFront")]
        public JsonResult GetAllRiverFront(int languageid = 1, long regionId = 0, long basinId = 0)
        {
            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.AddRange(riverDamMasterService.GetRiverListF(languageid, regionId, basinId).Select(x => new ListItem { Text = x.Name, Value = x.Id.ToString(), UploadDocumentPath = x.UploadDocumentPath, Description = x.Description }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(lsdata);
        }

        [HttpPost("/GetAllCanalFront")]
        public JsonResult GetAllCanalFront(int languageid = 1, long regionId = 1, long canalId = 1)
        {
            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.AddRange(CanalMasterService.GetCanalListF(languageid, regionId, canalId).Select(x => new ListItem { Text = x.Name, regionId = x.RegionId, languageId = x.LanguageId, Value = x.CanalId.ToString(), IsActive = x.IsActive, CanalId = Convert.ToInt32(x.CanalId), UploadDocumentPath = x.UploadDocumentPath, Description = x.Description }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(lsdata);
        }
        #endregion
    }
}