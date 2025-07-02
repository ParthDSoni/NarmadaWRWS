using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWRWS.Webs.Controllers
{
    public class BlogController : Controller
    {
        #region Controller Variable

        protected readonly IBlogService objBlogService;
        protected readonly IResearchMasterService objResearchService;

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

        public BlogController(IBlogService _objBlogService, IResearchMasterService objResearchService)
        {
            objBlogService = _objBlogService;
            this.objResearchService = objResearchService;
        }

        #endregion

        [HttpPost("/GetBlog")]
        public JsonResult GetBlog(int currentPage)
        {
            try
            {

                BlogListMasterModel lstBlogList = new BlogListMasterModel();
                lstBlogList.BlogListMasterModels = objBlogService.GetList(LanguageId).Where(x => x.IsActive == true && x.TypeId == 1).ToList();

                return Json(lstBlogList);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        [Route("/SchemeDetails/{id?}")]
        public IActionResult SchemeDetails(string? id)
        {
            ViewBag.Id = id;
            return View();
        }
		[Route("/RegionDetails/{id?}")]
		public IActionResult RegionDetails(string? id)
		{
			ViewBag.Id = id;
			return View();
		}
		[HttpPost("/BindSchemeList")]
        public JsonResult BindSchemeList(long SchmeTypeMasterId=1)
        {
            try
            {
                var lstdata = objResearchService.GetList(LanguageId, SchmeTypeMasterId).Where(x => x.IsActive).ToList();

                return Json(lstdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }
		[HttpPost("/BindRegionList")]
		public JsonResult BindRegionList(long SchmeTypeMasterId=2)
		{
			try
			{
				var lstdata = objResearchService.GetList(LanguageId, SchmeTypeMasterId).Where(x => x.IsActive).ToList();

				return Json(lstdata);
			}
			catch (Exception ex)
			{
				ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
			}
			return Json("");
		}

		[HttpPost("/BindScheme")]
        public JsonResult BindScheme(string schemeId)
        {
            try
            {
                long.TryParse(Functions.FrontDecrypt(schemeId), out long lgSchemeId);
                var lstdata = objResearchService.Get(lgSchemeId, LanguageId);

                return Json(lstdata);
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }
		[HttpPost("/BindRegion")]
		public JsonResult BindRegion(string schemeId)
		{
			try
			{
				long.TryParse(Functions.FrontDecrypt(schemeId), out long lgSchemeId);
				var lstdata = objResearchService.Get(lgSchemeId, LanguageId);

				return Json(lstdata);
			}
			catch (Exception ex)
			{
				ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
			}
			return Json("");
		}

		[Route("Home/ProjectDetails")]
        public IActionResult ProjectDetails()
        {
            return View();
        }

    }
}
