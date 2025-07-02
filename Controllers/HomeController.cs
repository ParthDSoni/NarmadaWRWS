using Google.Protobuf.WellKnownTypes;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.Services.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Data;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;

namespace NWRWS.Webs.Controllers
{
    public class HomeController : Controller
    {
        #region Controller Variable
        protected readonly IBannerService objBannerService;
        protected readonly IGlobleSerchService objSerchServices;
        protected readonly IHttpClientFactory httpClientFactory;
        protected readonly IMinisterServices objMinisterServices;
        protected readonly IGoiLogoServices objGoiLogoServices;
        protected readonly INewsMasterService objNewsMasterService;
        protected readonly ICMSMenuMasterService objCMSMenuMasterService;
        protected readonly ICMSMenuResourceMasterService objMenuResourceMasterService;
        protected readonly ICMSTemplateMasterService objCMSTemplateMasterService;
        protected readonly IGalleryService objGallaryMasterService;
        protected readonly IBlogService objBlogService;
        protected readonly ISuccsessStoryServices objSuccsessStoryServices;
        protected readonly IPopupServices objPopupServices;
        protected readonly IVideoMasterServices objVideoMasterServices;
        protected readonly IStatesticServices objStatesticService;
        protected readonly ITenderMasterService objTenderMasterService;
        protected readonly IEcitizenMasterService objEcitizenMasterService;
        protected readonly IAnnouncementMasterService objAnnouncementMasterService;
        protected readonly IPublicationTypeMaster PublicationTypeService;
        protected readonly IResearchMasterService objResearchMasterService;
        protected readonly IMediaMasterService objMediaMasterService;
        protected readonly IDropDownService objDropDownService;
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

        public HomeController(IBlogService _objBlogService, IGlobleSerchService _objSerchServices, ISuccsessStoryServices _succsessStoryServices, ICMSTemplateMasterService _objCMSTemplateMasterService, ICategoryServices _CategoryServices, ICMSMenuMasterService _adminMenuMasterService, ICMSMenuResourceMasterService _menuResourceMasterService, IMinisterServices _objMinisterServices, IBannerService _objBannerService, IHttpClientFactory _httpClientFactory, IGoiLogoServices _objGoiLogoServices, INewsMasterService _objNewsMasterService, IGalleryService _objGallaryMasterService, IPopupServices _objPopupServices, IVideoMasterServices _objVideoMasterServices, IStatesticServices _objstatesticService, ITenderMasterService _objTenderMasterService, IEcitizenMasterService _objEcitizenMasterService, IAnnouncementMasterService _objAnnouncementMasterService, IPublicationTypeMaster _publicationTypeService, IResearchMasterService _objResearchMasterService, IMediaMasterService _objMediaMasterService, IDropDownService _objDropDownService)
        {
            objCMSMenuMasterService = _adminMenuMasterService;
            objMenuResourceMasterService = _menuResourceMasterService;
            objBannerService = _objBannerService;
            objMinisterServices = _objMinisterServices;
            httpClientFactory = _httpClientFactory;
            objGoiLogoServices = _objGoiLogoServices;
            objNewsMasterService = _objNewsMasterService;
            objCMSTemplateMasterService = _objCMSTemplateMasterService;
            objGallaryMasterService = _objGallaryMasterService;
            objBlogService = _objBlogService;
            objSuccsessStoryServices = _succsessStoryServices;
            objPopupServices = _objPopupServices;
            objSerchServices = _objSerchServices;
            objVideoMasterServices = _objVideoMasterServices;
            objStatesticService = _objstatesticService;
            objTenderMasterService = _objTenderMasterService;
            objEcitizenMasterService = _objEcitizenMasterService;
            objAnnouncementMasterService = _objAnnouncementMasterService;
            PublicationTypeService = _publicationTypeService;
            objResearchMasterService = _objResearchMasterService;
            objMediaMasterService = _objMediaMasterService;
            objDropDownService = _objDropDownService;
        }

        #endregion

        [IgnoreAntiforgeryToken]
        [Route("/ViewFile")]
        public async Task<ActionResult> ViewFile(string fileName)
        {
            string strReturnPath = "";

            JsonResponseModel objreturn = new JsonResponseModel();
            fileName = HttpUtility.UrlDecode(fileName.ToString().Replace("?", "").Replace(");", "")).Replace("'", "").Trim();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                try
                {
                    fileName = fileName.Replace("HASH_HASH", "+").Replace("HASH__HASH", "/");
                    fileName = NWRWS.Common.Functions.FrontDecrypt(fileName).Replace("_", " ");
                }
                catch (Exception ex)
                {

                }
                var docData = await Functions.DownloadFile(httpClientFactory, fileName);
                if (docData != null)
                {
                    if (docData.isError)
                    {
                        objreturn = docData;
                        return Redirect(Url.Content("~" + strReturnPath));
                    }
                    else
                    {
                        objreturn.isError = false;
                        objreturn.strMessage = "";
                        string ContentType = "";
                        string extension = System.IO.Path.GetExtension(docData.result.Filename).Replace(".", "").ToUpper();

                        new FileExtensionContentTypeProvider().TryGetContentType(docData.result.Filename, out ContentType);

                        if (Functions.ValidateFileExtention(extension, FileType.ImageType) || Functions.ValidateFileExtention(extension, FileType.AllType) || Functions.ValidateFileExtention(extension, FileType.VideoType))
                        {
                            return new FileContentResult(docData.result.DataBytes, ContentType);
                        }
                        else
                        {
                            if (extension == ("apk").ToUpper())
                            {
                                ContentType = "application/vnd.android.package-archive";
                            }
                            return File(docData.result.DataBytes, ContentType, docData.result.Filename);
                        }
                    }
                }
                else
                {
                    objreturn.isError = true;
                    objreturn.strMessage = "File Don't able to View";
                    return Redirect(strReturnPath);
                }
            }
            else
            {
                objreturn.isError = true;
                objreturn.strMessage = "Please Enter File Path.";
                return Redirect(strReturnPath);
            }
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Index1")]
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult HomePage()
        {
            // return Redirect(Webs.Common.Functions.GetHomePage(Url));
            return Redirect(Url.Content("~/Index"));
        }

        public IActionResult TenderDetails(string id)
        {
            TenderMasterModelNew model = new TenderMasterModelNew();
            try
            {
                if (long.TryParse(NWRWS.Common.Functions.FrontDecrypt(HttpUtility.UrlDecode(id)), out long lgid))
                {
                    model = objTenderMasterService.Get(lgid);
                    
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return View(model);
        }
        public IActionResult RecruitmentDetails(string id)
        {
            NewsModel model = new NewsModel();
            try
            {
                if (long.TryParse(NWRWS.Common.Functions.FrontDecrypt(HttpUtility.UrlDecode(id)), out long lgid))
                {
                    model = objNewsMasterService.Get(lgid);

                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return View(model);
        }

        [HttpPost]
        [Route("/BindLanguage")]
        public JsonResult BindLanguage()
        {

            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.AddRange(objMenuResourceMasterService.GetListLanguage().Select(x => new ListItem { Text = x.Name, Value = x.Id.ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        [Route("/BindAnnouncements")]
        public JsonResult GetAnnouncements()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<AnnouncementModel> lstAnnouncementList = objAnnouncementMasterService.GetList(LanguageId).Where(x => x.IsActive == true).OrderByDescending(c => c.CreatedDate).Take(5).ToList();
                if (lstAnnouncementList.Count() > 0)
                {
                    objreturn.result = lstAnnouncementList;
                    objreturn.isError = false;
                }

                //else
                //{
                //    objreturn.strMessage = "Banner Not Found";
                //    objreturn.isError = true;
                //    objreturn.type = PopupMessageType.error.ToString();
                //}
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

        [Route("/BindResearch")]
        public JsonResult BindResearch(long id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<ResearchMasterModel> lstResearchList= objResearchMasterService.GetList(LanguageId, id).ToList();
                if (lstResearchList.Count() > 0)
                {
                    objreturn.result = lstResearchList;
                    objreturn.isError = false;
                }

                //else
                //{
                //    objreturn.strMessage = "Banner Not Found";
                //    objreturn.isError = true;
                //    objreturn.type = PopupMessageType.error.ToString();
                //}
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

        [Route("/BindMedia")]
        public JsonResult BindMedia()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<MediaMasterModel> lstMediaList = objMediaMasterService.GetList(LanguageId).Where(x => x.IsActive == true).ToList();
                if (lstMediaList.Count() > 0)
                {
                    objreturn.result = lstMediaList;
                    objreturn.isError = false;
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

        [HttpPost]
        [Route("/BindPressReleaseLogo")]
        public JsonResult BindPressReleaseLogo()
        {
            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "PressReleaseLogo" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindFooter")]
        public JsonResult BindFooter()
        {

            try
            {


                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "FootertDesign" && x.IsActive == true).FirstOrDefault();


                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindRelatedlinks")]
        public JsonResult BindRelatedlinks()
        {

            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "RelatedLinks" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindHeader")]
        public JsonResult BindHeader()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "HeaderDesign" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindMainMobileMenuFooter")]
        public JsonResult BindMainMobileMenuFooter()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "MainMobileMenuFooter" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/Bindmainlogo")]
        public JsonResult Bindmainlogo()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "MainLogo" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindProjectTemplate")]
        public JsonResult BindProjectTemplate()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "ProjectsTempate" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindPolicyTemplate")]
        public JsonResult BindPolicyTemplate()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "PoliciesTempate" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindScienceandTechnologiesTemplate")]
        public JsonResult BindScienceandTechnologiesTemplate()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "ScienceAndTechnologies" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindMinisterList")]
        public JsonResult BindMinisterList()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "MinisterList" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindPublicationTypeData")]
        public JsonResult BindPublicationTypeData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            try
            {
                var lsdata = PublicationTypeService.GetList().Where(x=>x.IsActive==true);

                return Json(new { draw = draw, recordsFiltered = lsdata.Count(), recordsTotal = lsdata.Count(), data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }

        }
        [HttpPost]
        [Route("/BindWelComeNote")]
        public JsonResult BindWelComeNote(string templatename)
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                //var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "WelcomeNote" && x.IsActive == true).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == templatename && x.IsActive == true).FirstOrDefault();
                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindQuickLinks")]
        public JsonResult BindQuickLinks()
        {

            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "QuickLinks" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        [Route("GetAllPopupDetails")]
        public async Task<JsonResult> GetAllPopupDetails()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<PopupModel> lstPopupList = objPopupServices.GetList().Where(x => x.IsActive == true).ToList();
                if (lstPopupList.Count() > 0)
                {
                    objreturn.result = lstPopupList;
                    objreturn.isError = false;
                }

                //else
                //{
                //    objreturn.strMessage = "Banner Not Found";
                //    objreturn.isError = true;
                //    objreturn.type = PopupMessageType.error.ToString();
                //}
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
        [HttpPost]
        [Route("/GSCPSInnovations")]
        public JsonResult GSCPSInnovations()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "GSCPSInnovationsArea" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindHomeSideBarLink")]
        public JsonResult BindHomeSideBarLink()
        {

            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "HomeSideBarLink" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindCitizenCorner")]
        public JsonResult BindCitizenCorner()
        {

            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "CitizenCorner" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindHomeQuickLinksList")]
        public JsonResult BindHomeQuickLinksList()
        {

            try
            {
                var data = objMenuResourceMasterService.GetListFront(LanguageId).Where(x => x.col_menu_type == "3" && x.IsActive == true).OrderBy(x => x.col_parent_id).ThenBy(n => n.MenuRank).ToList();

                if (data != null)
                {
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(new List<CMSMenuMasterModel>());
        }

        [HttpPost]
        [Route("/BindCalander")]
        public JsonResult BindCalander()
        {

            try
            {
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "CalanderArea" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));

                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindSiteMap")]
        public JsonResult BindSiteMap()
        {

            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("");
                List<CMSMenuResourceModel> lstMenuList = objMenuResourceMasterService.GetListFront(LanguageId).OrderBy(x => x.col_parent_id).ThenBy(n => n.MenuRank).ToList();


                List<CMSMenuResourceModel> lstMainMenuList = lstMenuList.Where(z => z.col_menu_type != "2" && z.col_menu_type != "3").OrderBy(x => x.col_parent_id).ThenBy(n => n.MenuRank).ToList();
                //var data=objCMSTemplateMasterService.GetList().Where(x => x.TemplateType == CMSTemplateType.FooterLayout.ToString()).FirstOrDefault();
                #region Main Menu

                if (lstMainMenuList != null)
                {

                    foreach (var mainMenu in lstMainMenuList.Where(x => x.col_parent_id == 0).ToList())
                    {
                        string strPath = mainMenu.MenuURL;
                        if (mainMenu.MenuURL != "#")
                        {

                            if (mainMenu.ResourceType == "0")
                            {
                                mainMenu.MenuURL = "/Home/" + mainMenu.MenuURL.Replace(@"\\", "\\");
                            }
                            else if (mainMenu.MenuURL.ToLower().StartsWith("http"))
                            {
                                mainMenu.MenuURL = mainMenu.MenuURL;
                            }
                            else
                            {

                                mainMenu.MenuURL = ("/" + mainMenu.MenuURL).Replace(@"//", "/");
                            }

                            if (mainMenu.MenuURL.StartsWith("/"))
                            {
                                strPath = mainMenu.MenuURL.ToString();
                            }

                        }
                        stringBuilder.Append("<div class='col-lg-3 col-md-4 col-sm-12 col-12'>                              ");
                        stringBuilder.Append("    <div class='single_sitemap_block'>                                        ");

                        stringBuilder.Append("<h4><a " + (mainMenu.IsRedirect ? " target=\"_blank\"" : "") + " href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : (mainMenu.MenuURL.ToLower().StartsWith("http") ? strPath : Url.Content("~" + strPath))) + "'>" + mainMenu.MenuName + "</a></h4>");

                        List<CMSMenuResourceModel> lstSubList = lstMainMenuList.Where(x => x.col_parent_id == mainMenu.Id).OrderBy(x => x.col_parent_id).ToList();
                        if (lstSubList.Count() > 0)
                        {

                            stringBuilder.Append("        <ul>                                                                  ");
                            foreach (CMSMenuResourceModel item in lstSubList)
                            {
                                stringBuilder.Append(GetSubList(item, lstMainMenuList));
                            }
                            stringBuilder.Append("        </ul>                                                                 ");

                        }

                        stringBuilder.Append("    </div>                                                                    ");
                        stringBuilder.Append("</div>");

                    }

                }

                #endregion

                return Json((HttpUtility.HtmlEncode(stringBuilder.ToString())));
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json("");
        }

        private string GetSubList(CMSMenuResourceModel mainMenu, List<CMSMenuResourceModel> lstMainMenuList)
        {
            StringBuilder stringBuilder = new StringBuilder();


            string strPath = mainMenu.MenuURL;
            if (mainMenu.MenuURL != "#")
            {

                if (mainMenu.ResourceType == "0")
                {
                    mainMenu.MenuURL = "/Home/" + mainMenu.MenuURL.Replace(@"\\", "\\");
                }
                else if (mainMenu.MenuURL.ToLower().StartsWith("http"))
                {
                    mainMenu.MenuURL = mainMenu.MenuURL;
                }
                else
                {

                    mainMenu.MenuURL = ("/" + mainMenu.MenuURL).Replace(@"//", "/");
                }

                if (mainMenu.MenuURL.StartsWith("/"))
                {
                    strPath = mainMenu.MenuURL.ToString();
                }

            }
            List<CMSMenuResourceModel> lstSubList = lstMainMenuList.Where(x => x.col_parent_id == mainMenu.Id).OrderBy(x => x.col_parent_id).ToList();
            if (lstSubList.Count() > 0)
            {
                stringBuilder.Append("<div class='col-lg-3 col-md-4 col-sm-12 col-12'>                              ");
                stringBuilder.Append("    <div class='single_sitemap_block'>                                        ");

                stringBuilder.Append("<h4><a " + (mainMenu.IsRedirect ? " target=\"_blank\"" : "") + " href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : (mainMenu.MenuURL.ToLower().StartsWith("http") ? strPath : Url.Content("~" + strPath))) + "'>" + mainMenu.MenuName + "</a></h4>");


                stringBuilder.Append("        <ul>                                                                  ");
                foreach (CMSMenuResourceModel item in lstSubList)
                {
                    stringBuilder.Append(GetSubList(item, lstMainMenuList));
                }
                stringBuilder.Append("        </ul>                                                                 ");


                stringBuilder.Append("    </div>                                                                    ");
                stringBuilder.Append("</div>");
            }

            else
            {
                stringBuilder.Append("<li><a " + (mainMenu.IsRedirect ? " target=\"_blank\"" : "") + " href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : (mainMenu.MenuURL.ToLower().StartsWith("http") ? strPath : Url.Content("~" + strPath))) + "'>" + mainMenu.MenuName + "</a></li>");

            }
            return stringBuilder.ToString();
        }

        [HttpPost]
        [Route("/UpdateLanguage")]
        public JsonResult UpdateLanguage(long langId)
        {
            LanguageId = langId;

            return Json(LanguageId);
        }

        [Route("Home/{strCurrentPath}")]
        public IActionResult PageDetails(string strCurrentPath)
        {
            //string strQury= Request.Query.ToString();
            if (!string.IsNullOrEmpty(strCurrentPath))
            {
                ViewBag.pagename = strCurrentPath;
                Regex r = new Regex(@"^[a-zA-Z0-9-_/]+$");


                if (r.IsMatch(strCurrentPath))
                {
                    var data = objCMSMenuMasterService.GetList().Where(x => x.MenuURL == strCurrentPath).FirstOrDefault();

                    if (data != null)
                    {
                        if (strCurrentPath.ToUpper().StartsWith("HTTPS") || strCurrentPath.ToUpper().StartsWith("HTTP"))
                        {
                            strCurrentPath = "https";
                            strCurrentPath = strCurrentPath + "://" + data.PageDescription.Replace("<p>", "").Replace("</p>", "").Trim();
                            if (!strCurrentPath.ToUpper().EndsWith("ASPX"))
                            {
                                strCurrentPath = strCurrentPath + "/";
                            }

                            return Redirect(strCurrentPath);
                        }
                        else
                        {
                            var menuresourcedata = objMenuResourceMasterService.GetMenuRes(data.MenuResId, LanguageId);
                            if (menuresourcedata != null)
                            {
                                data.PageDescription = (HttpUtility.HtmlDecode(menuresourcedata.PageDescription));


                                if (menuresourcedata.TemplateId != "0" && menuresourcedata.TemplateId != null)
                                {
                                    var newstring = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(menuresourcedata.TemplateId), LanguageId);

                                    if (newstring != null)
                                    {
                                        if (menuresourcedata.PageDescription != null)
                                        {
                                            string strMain = menuresourcedata.PageDescription.Replace("{{" + newstring.TemplateName.Replace(" ", "") + "}}", newstring.Content);
                                            data.PageDescription = (HttpUtility.HtmlDecode(strMain));
                                        }

                                    }
                                    return View(data);
                                }
                                return View(data);

                            }
                            else
                            {
                                menuresourcedata = objMenuResourceMasterService.Get(data.MenuResId, 1);

                                data.PageDescription = (HttpUtility.HtmlDecode(menuresourcedata.PageDescription));



                                if (menuresourcedata.TemplateId != "0" && menuresourcedata.TemplateId != null)
                                {
                                    var newstring = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(menuresourcedata.TemplateId), 1);

                                    if (newstring != null)
                                    {
                                        string strMain = menuresourcedata.PageDescription.Replace("{{" + newstring.TemplateName.Replace(" ", "") + "}}", newstring.Content);
                                        data.PageDescription = (HttpUtility.HtmlDecode(strMain));

                                    }
                                    return View(data);
                                }

                            }


                            return View(data);
                        }
                    }
                }
            }
            //return Redirect(Url.Content(Webs.Common.Functions.GetHomePage(Url)));
            return Redirect(Url.Content("~/Index"));
        }

        [Route("GetAllBannerF")]
        public JsonResult GetAllBannerF()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<BannerModel> lstNewsList = objBannerService.GetListF().Where(x => x.IsActive == true).OrderBy(x => x.BannerRank).ToList();
                if (lstNewsList.Count() > 0)
                {
                    objreturn.result = lstNewsList;
                    objreturn.isError = false;
                }

                //else
                //{
                //    objreturn.strMessage = "Banner Not Found";
                //    objreturn.isError = true;
                //    objreturn.type = PopupMessageType.error.ToString();
                //}
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

        [Route("GetAllMinister")]
        public async Task<JsonResult> GetAllMinisters()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {

                List<MinisterModel> ministerList = objMinisterServices.GetListFront(LanguageId).OrderBy(x => x.MinisterRank).ToList();

                if (ministerList.Count() == 0)
                {
                    ministerList = objMinisterServices.GetListFront(LanguageId).OrderBy(x => x.MinisterRank).ToList();
                }

                if (ministerList.Count() > 0)
                {
                    objreturn.result = ministerList;
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.result = ministerList;
                    objreturn.isError = false;
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

        [Route("GetAllBrandLogo")]
        public JsonResult GetAllBrandLogo()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {

                List<GoiLogoModel> LogoList = objGoiLogoServices.GetList(LanguageId).ToList();

                if (LogoList.Count() > 0)
                {
                    objreturn.result = LogoList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.result = LogoList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
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
        [Route("GetAudio")]
        public JsonResult GetAudio()

        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<NewsModel> NewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "A").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "A").ToList();

                }

                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                /*foreach (var item in NewsList)
                {
                    string strDate = "";
                    if (item.NewsStartDate != null)
                    {
                        item.NewsDate = item.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("/", "-");
                        strDate = item.NewsDate;
                    }*/

                else
                {
                    objreturn.isError = false;
                }

            }
            //}
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
                objreturn.strMessage = "Record not Found, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }

            return Json(objreturn);
        }

        [Route("GetNews")]
        public JsonResult GetNews()

        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {

                List<NewsModel> NewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "2").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objNewsMasterService.GetListFront(1).Where(x => x.NewsTypeId == "2").ToList();

                }
                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                /*foreach (var item in NewsList)
                {
                    string strDate = "";
                    if (item.NewsStartDate != null)
                    {
                        item.NewsDate = item.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("/", "-");
                        strDate = item.NewsDate;
                    }*/

                else
                {
                    objreturn.isError = false;
                }

            }
            //}
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
                objreturn.strMessage = "Record not Found, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }

            return Json(objreturn);
        }



        [Route("GetTenders")]
        public JsonResult GetTenders()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<EcitizenModel> lstTenderModelList = new List<EcitizenModel>();

                lstTenderModelList = objEcitizenMasterService.GetListFront(LanguageId).Where(x => x.EcitizenTypeId == "5").ToList();

                if (lstTenderModelList.Count() > 0)
                {
                    objreturn.result = lstTenderModelList.Where(x => x.IsActive == true).ToList();
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
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
        [Route("GetStatasticcount")]
        public JsonResult GetStatasticcount()

        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<StatesticModel> NewsList = objStatesticService.GetListFront(LanguageId).Where(x => x.StatesticTypeId == "1").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objStatesticService.GetListFront(1).Where(x => x.StatesticTypeId == "1").ToList();

                }
                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
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
        [Route("GetActivity")]
        public JsonResult GetActivity()

        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<StatesticModel> NewsList = objStatesticService.GetListFront(LanguageId).Where(x => x.StatesticTypeId == "2").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objStatesticService.GetListFront(1).Where(x => x.StatesticTypeId == "2").ToList();

                }
                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
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


        [Route("GetCitizenCorner")]
        public JsonResult GetCitizenCorner()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<StatesticModel> NewsList = objStatesticService.GetListFront(LanguageId).Where(x => x.StatesticTypeId == "3").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objStatesticService.GetListFront(1).Where(x => x.StatesticTypeId == "3").ToList();

                }
                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
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


        [Route("BindLatestTenders")]
        public JsonResult BindLatestTenders()

        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {

                List<NewsModel> NewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "3").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objNewsMasterService.GetListFront(1).Where(x => x.NewsTypeId == "3").ToList();

                }
                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
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

        [Route("GetPastEvent")]
        public JsonResult GetPastEvent()

        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<NewsModel> NewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "P").ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objNewsMasterService.GetListFront(1).Where(x => x.NewsTypeId == "P").ToList();

                }

                if (NewsList.Count() > 0)
                {
                    objreturn.result = NewsList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }
                /*foreach (var item in NewsList)
                {
                    string strDate = "";
                    if (item.NewsStartDate != null)
                    {
                        item.NewsDate = item.NewsStartDate.Value.ToString("dd/MM/yyyy").Replace("/", "-");
                        strDate = item.NewsDate;
                    }*/

                else
                {
                    objreturn.isError = false;
                }

            }
            //}
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
                objreturn.strMessage = "Record not Found, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }

            return Json(objreturn);
        }
        [Route("/GetTenderData")]
        public JsonResult GetTenderData()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                dynamic data = objTenderMasterService.GetTenderList();
               
                if (data.Count > 0)
                {
                    objreturn.result = data;
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
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

        [Route("GetNewsCircularResolution")]
        public JsonResult GetNewsCircularResolution()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {

                List<NewsModel> NewsList = objNewsMasterService.GetNewsCircularResolution(LanguageId).ToList();

                if (NewsList.Count() == 0)
                {
                    NewsList = objNewsMasterService.GetListFront(1).ToList();
                }

                if (NewsList.Count() > 0)
                {
                    //NewsList = objNewsMasterService.GetListFront(LanguageId).Where(x => x.NewsTypeId == "1").ToList();
                    objreturn.result = NewsList;
                    objreturn.isError = false;
                }

                else
                {

                    objreturn.isError = false;

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

        [Route("GetSuccsessStory")]
        public JsonResult GetSuccsessStory()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                long lid = 1;
                List<SuccsessStoryMasterModel> BlogList = objSuccsessStoryServices.GetList(lid).ToList();

                if (BlogList.Count() > 0)
                {
                    objreturn.result = BlogList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.result = BlogList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
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
        [HttpPost]
        [Route("/GetAllAlbumCMFirstData")]
        public JsonResult GetAllAlbumCMFirstData(long department, string? Title, DateTime? date, DateTime? date1, int currentPage)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            GalleryAllDetails lstGalleryModelList = new GalleryAllDetails();
            try
            {
                int pageSize = 3;
                int skip = (currentPage - 1) * pageSize;
                double pageCount = (double)((decimal)objGallaryMasterService.GetCMAlbum().Count() / Convert.ToDecimal(pageSize));
                lstGalleryModelList.CurrentPage = currentPage;
                lstGalleryModelList.PageCount = (int)Math.Ceiling(pageCount);
                var lsdata = objGallaryMasterService.GetCMAlbum();

                if (Title != null)
                {
                    lsdata = lsdata.Where(x => x.PlaceName.ToLower().Contains(Title.ToLower())).ToList();
                    lstGalleryModelList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                }
                if (department != 0)
                {
                    lsdata = lsdata.Where(x => x.DepartmentId == department).ToList();
                    lstGalleryModelList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                }

                if (date != null && date1 != null)
                {
                    if (date <= date1)
                    {
                        lsdata = lsdata.Where(x => x.GalleryDate >= date && x.GalleryDate <= date1).ToList();
                        lstGalleryModelList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                    }
                    else
                    {
                        objreturn.strMessage = "DateValidationError";
                        objreturn.isError = true;
                        objreturn.type = PopupMessageType.error.ToString();
                        return Json(objreturn);
                    }
                }
                lsdata = lsdata.Skip(skip).Take(pageSize).ToList();
                objreturn.strMessage = "";
                objreturn.isError = false;
                if (lsdata.Any())
                {
                    objreturn.result = lsdata;
                }
                else
                {
                    objreturn.strMessage = "Record not Found.";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objreturn.strMessage = "Record not Found.";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(new { data = objreturn.result, currentPage = lstGalleryModelList.CurrentPage, pageCount = lstGalleryModelList.PageCount });
        }

        [HttpPost]
        [Route("/GetAllAlbumFirstData")]
        public JsonResult GetAllAlbumFirstData(long department, string? Title,DateTime? date, DateTime? date1, int currentPage)
       {
            JsonResponseModel objreturn = new JsonResponseModel();
            GalleryAllDetails lstGalleryModelList = new GalleryAllDetails();
            try
            {
                int pageSize = 6;
                int skip = (currentPage - 1) * pageSize;
                double pageCount = (double)((decimal)objGallaryMasterService.GetAlbum().Count() / Convert.ToDecimal(pageSize));
                lstGalleryModelList.CurrentPage = currentPage;
                lstGalleryModelList.PageCount = (int)Math.Ceiling(pageCount);
                var lsdata = objGallaryMasterService.GetAlbum();

                if (Title != null)
                {
                    lsdata = lsdata.Where(x => x.PlaceName.ToLower().Contains(Title.ToLower())).ToList();
                    lstGalleryModelList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                }
                if (department != 0)
                {
                    lsdata = lsdata.Where(x => x.DepartmentId == department).ToList();
                    lstGalleryModelList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                }

                if (date != null && date1 != null)
                {
                    if (date <= date1)
                    {
                        lsdata = lsdata.Where(x => x.GalleryDate >= date && x.GalleryDate <= date1).ToList();
                        lstGalleryModelList.PageCount = (int)Math.Ceiling((double)((decimal)lsdata.Count() / Convert.ToDecimal(pageSize)));
                    }
                    else
                    {
                        objreturn.strMessage = "DateValidationError";
                        objreturn.isError = true;
                        objreturn.type = PopupMessageType.error.ToString();
                        return Json(objreturn);
                    }
                }
                lsdata = lsdata.Skip(skip).Take(pageSize).ToList();
                objreturn.strMessage = "";
                objreturn.isError = false;
                if (lsdata.Any())
                {
                    objreturn.result = lsdata;
                }
                else
                {
                    objreturn.strMessage = "Record not Found.";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objreturn.strMessage = "Record not Found.";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(new { data = objreturn.result, currentPage = lstGalleryModelList.CurrentPage, pageCount = lstGalleryModelList.PageCount });
        }
        [HttpPost]
        [Route("/GetAllImagesByAlbum")]
        public JsonResult GetAllImagesByAlbum(string AlbumId)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            GalleryAllDetails lstGalleryModelList = new GalleryAllDetails();
            try
            {
                long.TryParse(Functions.FrontDecrypt(AlbumId), out long lgId);

                var lsdata = objGallaryMasterService.Get(lgId).lstGalleryImagesModels;
                objreturn.strMessage = "";
                objreturn.isError = false;
                if (lsdata.Any())
                {
                    objreturn.result = lsdata;
                }
                else
                {
                    objreturn.strMessage = "Record not Found.";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objreturn.strMessage = "Record not Found.";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(new { data = objreturn.result, currentPage = lstGalleryModelList.CurrentPage, pageCount = lstGalleryModelList.PageCount });
        }
        [Route("/PhotoGallery")]
        public IActionResult PhotoGallery()
        {
            return View();
        }
        [Route("/CMPhotoGallery")]
        public IActionResult CMPhotoGallery()
        {
            return View();
        }
        [Route("/SiteMap")]
        public IActionResult SiteMap()
        {
            return View();
        }
        [Route("Admin/EventGallary")]
        public IActionResult EventGallary()
        {
            EventTypeModel eventGallaryDisplayModel = new EventTypeModel();
            try
            {

                string strQuery = HttpContext.Request.QueryString.ToString();
                strQuery = Functions.FrontDecrypt(WebUtility.UrlDecode(strQuery.Replace("?", "")));
                string[] data = strQuery.Split("||");

                data = data.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                string eventid = strQuery.ToString();
                eventGallaryDisplayModel.Id = Convert.ToInt32(eventid.ToString());
                //var eventList = objGallaryMasterService.GetEventTypeList();

                //string strEvent = "", strEventBName = "";

                //var eventname = eventList.Where(x => x.Id.ToString() == eventid).FirstOrDefault();
                //strEvent = eventname.Title.ToString();

                //strEventBName = " Event Type Name is <b>" + eventname.Title + "</b> " + strEventBName + " ";
                //eventGallaryDisplayModel.Title = strEvent;



                return View(eventGallaryDisplayModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
            return View(eventGallaryDisplayModel);
        }
        [HttpPost]
        [Route("/GetAllAlbumImages")]
        public JsonResult GetAllAlbumImages(long id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                var lsdata = objGallaryMasterService.GetAlbumImages(id);

                objreturn.strMessage = "";
                objreturn.isError = false;
                if (lsdata != null)
                {
                    objreturn.result = lsdata;
                }
                else
                {
                    objreturn.strMessage = "Record not Found.";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objreturn.strMessage = "Record not Found.";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(objreturn);

        }

        public IActionResult SchemesDetailsPage(string id)
        {
            BlogMasterModel model = new BlogMasterModel();
            try
            {
                if (long.TryParse(NWRWS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    model = objBlogService.Get(lgid, LanguageId);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return View(model);
        }
        public IActionResult NewsDetailsPage(string id)
        {
            NewsModel model = new NewsModel();
            try
            {
                if (long.TryParse(NWRWS.Common.Functions.FrontDecrypt(HttpUtility.UrlDecode(id)), out long lgid))
                {
                    model = objNewsMasterService.Get(lgid, LanguageId);

                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return View(model);
        }

        [HttpGet]
        [Route("/Search")]
        public IActionResult Search(string serch)
        {
            SearchFormModel searchFormModel = new SearchFormModel();
            searchFormModel.serch = serch;
            return View(searchFormModel);
        }

        [Route("/GetWebSiteVisitorsCount")]
        [HttpPost]
        public async Task<JsonResult> GetWebSiteVisitorsCount()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                var ipaddress = HttpContext.Connection.RemoteIpAddress.ToString();
                objreturn = objCMSMenuMasterService.AddgetVisitorsCount(ipaddress);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(objreturn);
        }

        [Route("/GlobalSearch")]
        [HttpPost]
        public JsonResult GlobalSearch(string search)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<GlobleSerchModel> lstGbList = objSerchServices.GetList(Functions.FrontDecrypt(HttpUtility.UrlDecode(search)), LanguageId);
                objreturn.result = lstGbList;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json(objreturn);
        }

        [HttpGet]
        [Route("/BindUpcomingExam")]
        public JsonResult BindUpcomingExam()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                var lsdata = objCMSMenuMasterService.GetUpcomingExamsList(LanguageId);

                if (lsdata.Count() == 0)
                {
                    lsdata = objCMSMenuMasterService.GetUpcomingExamsList(1).ToList();
                }
                if (lsdata.Count() > 0)
                {
                    objreturn.result = lsdata;
                    objreturn.isError = false;
                }
                else
                {
                    objreturn.isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objreturn.strMessage = "Record not Found.";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(objreturn);


        }
        [Route("/Home/VideoGallery")]
        public IActionResult VideoGallery()
        {
            return View();
        }
        [Route("/AllSchemes")]
        public IActionResult AllSchemes()
        {
            return View();
        }
        [Route("GetVideoGallery")]
        public JsonResult GetVideoGallery()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<VideoMasterModel> VideoList = objVideoMasterServices.GetList().ToList();

                if (VideoList.Count() > 0)
                {
                    objreturn.result = VideoList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.result = VideoList.Where(x => x.IsActive == true);
                    objreturn.isError = false;
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

        [HttpPost]
        [Route("/BindMessage")]
        public JsonResult BindMessage()
        {
            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "MessageDesign" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindCMWideget")]
        public JsonResult BindCMWideget()
        {
            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "CMWidget" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindTwitterFeed")]
        public JsonResult BindTwitterFeed()
        {
            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "TwitterFeed" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindInstagramFeed")]
        public JsonResult BindInstagramFeed()
        {
            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "InstagramFeed" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
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
        [Route("/BindFaceBookFeed")]
        public JsonResult BindFaceBookFeed()
        {
            try
            {
                var data = objCMSTemplateMasterService.GetList().Where(x => x.TemplateName == "FaceBookFeed" && x.IsActive == true).FirstOrDefault();

                if (data != null)
                {
                    var datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), LanguageId);
                    if (datas != null)
                    {
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                    else
                    {
                        datas = objCMSTemplateMasterService.GetByTemp(Convert.ToInt32(data.TemplateId), 1);
                        return Json((HttpUtility.HtmlDecode(datas.Content)));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }
            return Json("");
        }

        // [HttpPost]
        [Route("GetLatestSchemes")]
        public JsonResult GetLatestSchemes()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<BlogMasterModel> SchemeBlogList = objBlogService.GetList(LanguageId).ToList();

                if (SchemeBlogList.Count() == 0)
                {
                    SchemeBlogList = objBlogService.GetList(1).ToList();
                }
                if (SchemeBlogList.Count() > 0)
                {
                    objreturn.result = SchemeBlogList.Where(x => x.IsActive == true && x.TypeId == 1);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.isError = false;
                    //objreturn.strMessage = "Blog Not Found";
                    //objreturn.isError = true;
                    //objreturn.type = PopupMessageType.error.ToString();
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

        // [HttpPost]
        [Route("GetProjectSchemes")]
        public JsonResult GetProjectSchemes()
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<BlogMasterModel> SchemeBlogList = objBlogService.GetList(LanguageId).ToList();

                if (SchemeBlogList.Count() == 0)
                {
                    SchemeBlogList = objBlogService.GetList(1).ToList();
                }
                if (SchemeBlogList.Count() > 0)
                {
                    objreturn.result = SchemeBlogList.Where(x => x.IsActive == true && x.TypeId == 2);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.isError = false;
                    //objreturn.strMessage = "Blog Not Found";
                    //objreturn.isError = true;
                    //objreturn.type = PopupMessageType.error.ToString();
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

        [Route("GetBlog")]
        public async Task<JsonResult> GetBlog(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                List<BlogMasterModel> SchemeBlogList = objBlogService.GetList(LanguageId).ToList();

                if (SchemeBlogList.Count() == 0)
                {
                    SchemeBlogList = objBlogService.GetList(1).ToList();
                }
                if (SchemeBlogList.Count() > 0)
                {
                    objreturn.result = SchemeBlogList.Where(x => x.IsActive == true && x.TypeId == 1);
                    objreturn.isError = false;
                }

                else
                {
                    objreturn.isError = false;
                    //objreturn.strMessage = "Blog Not Found";
                    //objreturn.isError = true;
                    //objreturn.type = PopupMessageType.error.ToString();
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
        [Route("/CSSCustom/{strFileName}.css")]
        public ContentResult GetTheme(string strFileName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string CSSCODE = "";
            using (ICssMasterService CSSMasterService = new CssMasterService())
            {
                var data = CSSMasterService.GetFileByName(strFileName);
                if (data != null)
                {
                    stringBuilder.Append(data.Cssfile);
                }
            }
            CSSCODE = stringBuilder.ToString();
            Response.ContentType = "text/css";
            return Content(CSSCODE, "text/css");
        }

        [IgnoreAntiforgeryToken]
        [Route("/DownloadFile")]
        public async Task<ActionResult> DownloadFile(string fileName)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            fileName = HttpUtility.UrlDecode(fileName.ToString().Replace("?", "").Replace(");", "")).Replace("'", "").Trim();
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                    fileName = fileName.Replace("HASH_HASH", "+").Replace("HASH__HASH", "/");
                    fileName = Functions.FrontDecrypt(fileName).Replace("_", " ");
                    var docData = await Functions.DownloadFile(httpClientFactory, fileName);
                    if (docData != null)
                    {
                        if (docData.isError)
                        {
                            objreturn = docData;
                            ViewData.Add("DataIssue", objreturn);
                            return Redirect(Url.Content("~/Index"));
                        }
                        else
                        {
                            objreturn.isError = false;
                            objreturn.strMessage = "";
                            string ContentType = "";
                            new FileExtensionContentTypeProvider().TryGetContentType(docData.result.Filename, out ContentType);

                            objreturn.result = new { dataBytes = docData.result.DataBytes, contentType = ContentType, fileName = docData.result.Filename };

                            return File(docData.result.DataBytes, ContentType, docData.result.Filename);
                        }
                    }
                    else
                    {
                        objreturn.isError = true;
                        objreturn.strMessage = "File Don't able to Download";
                        return Redirect(Url.Content("~/Index"));
                    }
            }
            else
            {
                objreturn.isError = true;
                objreturn.strMessage = "Please Enter File Path.";
                return Redirect(Url.Content("~/Index"));
            }
        }

        [Route("/GetAllFrontImagesById")]
        public JsonResult GetAllFrontImagesById(string blogMasterId)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(NWRWS.Common.Functions.FrontDecrypt(blogMasterId), out long lgid))
                {

                    List<BlogMasterModel> SchemeBlogList = objBlogService.GetList(LanguageId).ToList();

                    if (SchemeBlogList.Count() == 0)
                    {
                        SchemeBlogList = objBlogService.GetList(1).ToList();
                    }
                    if (SchemeBlogList.Count() > 0)
                    {
                        objreturn.result = SchemeBlogList.Where(x => x.IsActive == true && x.TypeId == 1 && x.BlogMasterId == lgid);
                        objreturn.isError = false;
                    }
                    else
                    {
                        objreturn.isError = false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                objreturn.strMessage = "Record not Found.";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(objreturn);

        }

        [HttpGet("/GetAllDistrict")]
        public JsonResponseModel GetCountries()
        {
            JsonResponseModel res = new JsonResponseModel();
            try
            {
                var data = objDropDownService.GetAllCountries();
                res.result = data;
                res.isError = false;
            }
            catch (Exception ex)
            {
                res.isError = true;
                res.result = null;
                res.strMessage = ex.Message;
            }
            return res;
        }
        [HttpGet("/GetAllStatesByCountry")]
        public JsonResponseModel GetAllStatesByCountry(long country)
        {
            JsonResponseModel res = new JsonResponseModel();
            try
            {
                var data = objDropDownService.GetAllStatesByCountry(country);
                res.result = data;
                res.isError = false;
            }
            catch (Exception ex)
            {
                res.isError = true;
                res.result = null;
                res.strMessage = ex.Message;
            }
            return res;
        }
        [HttpGet("/GetAllDistrictsByState")]
        public JsonResponseModel GetAllDistrictsByState(long state)
        {
            JsonResponseModel res = new JsonResponseModel();
            try
            {
                var data = objDropDownService.GetAllDistrictsByState(state);
                res.result = data;
                res.isError = false;
            }
            catch (Exception ex)
            {
                res.isError = true;
                res.result = null;
                res.strMessage = ex.Message;
            }
            return res;
        }
    }
}
