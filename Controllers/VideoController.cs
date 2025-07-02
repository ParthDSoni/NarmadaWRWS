using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NWRWS.Webs.Controllers
{
    public class VideoController : Controller
    {
        #region Controller Variable

        protected readonly IVideoMasterServices objVideoMasterServices;

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

        public VideoController(IVideoMasterServices _objVideoMasterServices)
        {
            objVideoMasterServices = _objVideoMasterServices;
        }

        #endregion
        [HttpPost]
        [Route("/GetVideo")]
        public async Task<JsonResult> GetVideo(long department, string? title, DateTime? date, DateTime? date1, int currentPage)
        {
            JsonResponseModel objReturn = new JsonResponseModel();
            VideoAllDetails videoList = new VideoAllDetails();

            try
            {
                int pageSize = 3;
                int skip = (currentPage - 1) * pageSize;
                double pageCount = (double)((decimal)objVideoMasterServices.GetList().Count() / Convert.ToDecimal(pageSize));
                videoList.CurrentPage = currentPage;
                videoList.PageCount = (int)Math.Ceiling(pageCount);
                var lsdata = objVideoMasterServices.GetList();

                if (!string.IsNullOrEmpty(title))
                {
                    lsdata = lsdata.Where(x => x.VideoTitle.ToLower().Contains(title.ToLower())).ToList();
                    videoList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                }

              

                if (date != null && date1 != null)
                {
                    if (date <= date1)
                    {
                        lsdata = lsdata.Where(x => x.VideoDate >= date && x.VideoDate <= date1).ToList();
                        videoList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                    }
                    else
                    {
                        objReturn.strMessage = "DateValidationError";
                        objReturn.isError = true;
                        objReturn.type = PopupMessageType.error.ToString();
                        return Json(objReturn);
                    }
                }
                lsdata = lsdata.Skip(skip).Take(pageSize).ToList();
                objReturn.strMessage = "";
                objReturn.isError = false;

                if (lsdata.Any())
                {
                    objReturn.result = lsdata;
                }
                else
                {
                    objReturn.strMessage = "Record not Found.";
                    objReturn.isError = true;
                    objReturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objReturn.strMessage = "An error occurred while processing your request.";
                objReturn.isError = true;
                objReturn.type = PopupMessageType.error.ToString();
            }

            return Json(new { data = objReturn.result, currentPage = videoList.CurrentPage, pageCount = videoList.PageCount });
        }

        [Route("/VideoGallery")]
        public IActionResult VideoGallery()
        {
            return View();
        }

    }
}
