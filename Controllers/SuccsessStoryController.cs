using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWRWS.Webs.Controllers
{
    public class SuccsessStoryController : Controller
    {
        #region Controller Variable
         
        protected readonly ISuccsessStoryServices objSuccsessStoryServices;

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

        public SuccsessStoryController(ISuccsessStoryServices _objSuccsessStoryServices)
        {
            objSuccsessStoryServices = _objSuccsessStoryServices;
        }

        #endregion
        [HttpPost]
        [Route("/GetSuccsessStory")]
        public async Task<JsonResult> GetSuccsessStory()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                long lid = 1;
                List<SuccsessStoryMasterModel> BlogList = objSuccsessStoryServices.GetList(lid).ToList();

                if (BlogList.Count() > 0)
                {
                    objreturn.result = BlogList.Where(x=>x.IsActive == true);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.strMessage = "Story Not Found";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
                objreturn.strMessage = "Record not Found, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }

            return Json(objreturn);
        }
        [Route("Home/SuccsessStory")]
        public IActionResult SuccsessStory()
        {     
            return View();
        }
         
    }
}
