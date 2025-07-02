using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using Microsoft.AspNetCore.Mvc;
using NWRWS.Model.System;

namespace NWRWS.Webs.Controllers
{
    public class GovernmentResolutionsController : Controller
    {
        #region Controller Variable

        protected readonly IBannerService objBannerService;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly ICMSTemplateMasterService objCMSTemplateMasterService;
        protected readonly IEcitizenMasterService objEcitizenMasterService;


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

        public GovernmentResolutionsController(IHttpClientFactory _httpClientFactory, ICMSTemplateMasterService _objCMSTemplateMasterService, IEcitizenMasterService _objEcitizenMasterService)
        {
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objEcitizenMasterService = _objEcitizenMasterService;
        }

        #endregion

        [HttpPost]
        [Route("/BindGRNewGrid")]
        public JsonResult BindGRNewGrid(string? grno,string? Title, string? Branch, DateTime? date, DateTime? date1)
        {
            List<EcitizenModel> lstNewsModelList = new List<EcitizenModel> ();
            try
            {
                lstNewsModelList = objEcitizenMasterService.GetListFront(LanguageId).Where(x => x.EcitizenTypeId == "3").ToList();

                if (grno != null)
                {
                    lstNewsModelList = lstNewsModelList.Where(x => x.GRNumber.ToLower().Contains(grno.ToLower())).ToList();
                }
                if (Title != null)
                {
                    lstNewsModelList = lstNewsModelList.Where(x => x.EcitizenTitle.ToLower().Contains(Title.ToLower())).ToList();
                }
                if (Branch != null && Branch != "0")
                {
                    lstNewsModelList = lstNewsModelList.Where(x => x.Branch.ToString() == Branch).ToList();
                }
                if (date != null && date1 != null)
                {
                    lstNewsModelList = lstNewsModelList
                        .Where(x => x.EcitizenStartDate >= date && x.EcitizenStartDate <= date1)
                        .ToList();
                }
                return Json(lstNewsModelList);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [HttpPost]
        [Route("/BindBranch")]
        public JsonResult BindBranch(long LGId = 0)
        {

            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                if (LanguageId == 2)
                {
                    lsdata.Add(new ListItem { Text = "-- શાખા પસંદ કરો --", Value = "" });
                }
                else
                {
                    lsdata.Add(new ListItem { Text = "-- Select Branch --", Value = "" });
                }
                if (LGId != 0)
                {
                    lsdata.AddRange(objEcitizenMasterService.GetListGRBranchFront(LGId).Select(x => new ListItem { Text = x.Branch_name, Value = x.Branch_Id.ToString() }).ToList());
                }
                else
                {
                    lsdata.AddRange(objEcitizenMasterService.GetListGRBranchFront(LanguageId).Select(x => new ListItem { Text = x.Branch_name, Value = x.Branch_Id.ToString() }).ToList());
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        [Route("/GovernmentResolutions")]
        public IActionResult GovernmentResolutions()
        {
            return View();
        }
    }
}
