using AngleSharp;
using AngleSharp.Css;
using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Ganss.Xss;
using Google.Protobuf.WellKnownTypes;
using NWRWS.IService.Service;
using NWRWS.Services.Service;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.IService.System;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.Services.Service;
using NWRWS.Services.System;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using NuGet.ContentModel;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Asn1.Pkcs;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Linq;
using static Google.Protobuf.WellKnownTypes.Field.Types;
using static System.Collections.Specialized.BitVector32;


namespace NWRWS.Webs.Common
{
    public class Functions
    {
        public static string regGlobalValidation;
        public static string regName;
        public static string regMobileNo;
        public static string regPincode;
        public static string regNumber;
        public static string regEmail;
        public static string regPassword;
        public static string regURL;
        public static string dateFormat;
        public static bool allowModalOutsideClick;
        public static bool allowKeyboardInputOnDate;
        public static bool allowKeyboardInputOnTime;
        public static bool allowInspectElement;
        protected IHttpClientFactory httpClientFactory { get; set; }
        internal static void GetAllDependencyInjection(IServiceCollection services)
        {
            services.AddAntiforgery(options =>
            {
                // Set Cookie properties using CookieBuilder properties†.
                options.FormFieldName = "AntiforgeryFieldname";
                options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.HttpOnly = true;
                options.SuppressXFrameOptionsHeader = false;
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue; // <-- ! long.MaxValue
                options.MultipartBoundaryLengthLimit = int.MaxValue;
                options.MultipartHeadersCountLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRoleMasterService, RoleMasterService>();
            services.AddScoped<IAdminMenuMasterService, AdminMenuMasterService>();
            services.AddScoped<IMenuResourceMasterService, MenuResourceMasterService>();
            services.AddScoped<IUserMasterService, UserMasterService>();
            services.AddScoped<IMenuRightsMasterService, MenuRightsMasterService>();
            services.AddScoped<ICMSMenuResourceMasterService, CMSMenuResourceMasterService>();
            services.AddScoped<ICMSMenuMasterService, CMSMenuMasterService>();
            services.AddScoped<ICMSTemplateMasterService, CMSTemplateMasterService>();
            services.AddScoped<ISMTPMasterService, SMTPMasterService>();
            services.AddScoped<ITenderMasterService, TenderMasterService>();
            services.AddScoped<ICouchDBMasterService, CouchDBMasterService>();
            services.AddScoped<ICssMasterService, CssMasterService>();
            services.AddScoped<IJsMasterService, JsMasterService>();
            services.AddScoped<IBannerService, BannerService>();
            services.AddScoped<IMinisterServices, MinisterMasterService>();
            services.AddScoped<IPopupServices, PopupMasterService>();
            services.AddScoped<INewsMasterService, NewsMasterService>();
            services.AddScoped<IGoiLogoServices, GoiLogoService>();
            services.AddScoped<ICategoryServices, CategoryMasterService>();
            services.AddScoped<IDocumentServices, DocumentService>();
            services.AddScoped<IWebAPIDataService, GetWebAPIData>();
            services.AddScoped<IGalleryService, GalleryService>();
            services.AddScoped<IFeedbackServices, FeedbackService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IPayScalServices, PayScalServices>();
            services.AddScoped<ISuccsessStoryServices, SuccsessStoryServices>();
            services.AddScoped<IVideoMasterServices, VideoMasterServices>();
            services.AddScoped<IGlobleSerchService, GlobleSerchService>();
            services.AddScoped<IEcitizenMasterService, EcitizenMasterService>();
            services.AddScoped<IStatesticServices, StatesticService>();
            services.AddScoped<IAnnouncementMasterService, AnnouncementMasterService>();
            services.AddScoped<IPublicationTypeMaster, PublicationTypeService>();
            services.AddScoped<IPublicationServices,PublicationMasterService>();
            services.AddScoped<IPressMasterService, PressReleaseMasterService>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<IDistrictMaster, DistrictService>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IResearchMasterService, ResearchMasterService>();
            services.AddScoped<IMediaMasterService, MediaMasterService>();
            services.AddScoped<IEarthquakeMasterService, EarthquakeMasterService>();
            services.AddScoped<IRegionMasterService, RegionMasterService>();
            services.AddScoped<IBasinMasterService, BasinMasterService>();
            services.AddScoped<ICanalMasterService, CanalMasterService>();
            services.AddScoped<IRiverDamMasterService, RiverDamMasterService>();
            services.AddScoped<IDropDownService, DropDownService>();
        }
        #region Admin Panel Breadcum

        public static string GetBreadcum(IUrlHelper url, HttpContext httpContext)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string strCurrentPath = httpContext.Request.Path.Value.ToString();

            var UserModel = SessionWrapper.Get<SessionUserModel>(httpContext.Session, "UserDetails");
            if (null == UserModel)
            {
                httpContext.Response.Redirect(url.Content("~/Account/Index"));
            }
            else
            {
                using (IUserMasterService userMasterService = new UserMasterService())
                using (IAdminMenuMasterService adminMenuMasterService = new AdminMenuMasterService())
                using (IMenuResourceMasterService menuResourceMasterService = new MenuResourceMasterService())
                using (IRoleMasterService roleMasterService = new RoleMasterService())
                using (IMenuRightsMasterService menuRightsMasterService = new MenuRightsMasterService())
                {
                    if (UserModel.RoleId == 0)
                    {
                        ErrorLogger.Error(" Functions \n\r CreateMainLayoutMenu \n\r Role " + UserModel.RoleId, "RoleId As 0", "Functions", "CreateMainLayoutMenu", "", true);
                        return "";
                    }
                    else if (string.IsNullOrWhiteSpace(strCurrentPath))
                    {
                        ErrorLogger.Error(" Functions \n\r CreateMainLayoutMenu \n\r Role " + UserModel.RoleId, "CurrentPath As " + strCurrentPath, "Functions", "CreateMainLayoutMenu", "", true);
                        return "";
                    }
                    else
                    {
                        List<MenuRightsMasterModel> lstMenuList = menuRightsMasterService.GetListByRoleId(UserModel.RoleId).OrderBy(x => x.ParentId).ThenBy(n => n.MenuRank).ToList();
                        var mainModel = lstMenuList.FirstOrDefault(x => x.MenuURL == strCurrentPath);

                        stringBuilder.Append("");

                        stringBuilder.Append("<ul class='breadcrumbs'>");

                        if (mainModel != null)
                        {
                            stringBuilder.Append("    <li>");
                            stringBuilder.Append("        <a href='#'>");
                            stringBuilder.Append("            <span class='text'>" + mainModel.Name + "</span>");
                            stringBuilder.Append("        </a>");
                            stringBuilder.Append("    </li>");

                            stringBuilder.Append(CreateSubBreadCum(lstMenuList, mainModel.ParentId));

                        }
                        stringBuilder.Append("    <li>");
                        stringBuilder.Append("        <a href='/Admin/Dashboard'>");
                        stringBuilder.Append("            <span class='icon icon-home'></span>");
                        stringBuilder.Append("        </a>");
                        stringBuilder.Append("    </li>");
                        stringBuilder.Append("</ul>");
                    }
                }

            }
            return stringBuilder.ToString();
        }

        private static string CreateSubBreadCum(List<MenuRightsMasterModel> lstMenuList, long parentId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var mainModel = lstMenuList.FirstOrDefault(x => x.Id == parentId);
            if (mainModel != null && parentId > 0)
            {
                if (mainModel.ParentId > 0)
                {
                    stringBuilder.Append(CreateSubBreadCum(lstMenuList, mainModel.ParentId));
                }
                else
                {
                    stringBuilder.Append("    <li>");
                    stringBuilder.Append("        <a href='#'>");
                    stringBuilder.Append("            <span class='text'>" + mainModel.Name + "</span>");
                    stringBuilder.Append("        </a>");
                    stringBuilder.Append("    </li>");
                }
            }
            else
            {
                return "";
            }
            return stringBuilder.ToString();
        }

        #endregion

        #region Public Breadcum
        public static string[] getHtmlSplitted(String text)
        {
            var list = new List<string>();
            var pattern = "(<code>|</code>)";
            var isInTag = false;
            var inTagValue = String.Empty;

            foreach (var subStr in Regex.Split(text, pattern))
            {
                if (subStr.Equals("<code>"))
                {
                    isInTag = true;
                    continue;
                }
                else if (subStr.Equals("</code>"))
                {
                    isInTag = false;
                    list.Add(String.Format("<code>{0}</code>", inTagValue));
                    continue;
                }

                if (isInTag)
                {
                    inTagValue = subStr;
                    continue;
                }

                list.Add(subStr);

            }
            return list.ToArray();
        }
        public static string CKEditerSanitizer(string CkHtml)
        {
            string strUPdateHTml;
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            var sanitizer = new HtmlSanitizer();
            if (CkHtml != null)
            {
                sanitizer.AllowedAttributes.Add("class");
                sanitizer.AllowedAttributes.Add("style");
                sanitizer.AllowedAttributes.Add("target");

                List<string> sas = getHtmlSplitted(CkHtml).ToList();
                List<string> sasUPdate = new List<string>();
                foreach (string sasd in sas)
                {
                    string strALocalVar = "";
                    string strALocalUPdateVar = "";
                    if (sasd.StartsWith("<code>"))
                    {
                        strALocalVar = HttpUtility.HtmlDecode(sasd);
                        strALocalUPdateVar = sanitizer.Sanitize(strALocalVar);
                    }
                    else
                    {
                        strALocalUPdateVar = (strALocalVar) = sasd;
                    }
                    sasUPdate.Add(strALocalUPdateVar);

                }
                CkHtml = string.Join(" ", sasUPdate);
            }
            else
            {
                CkHtml = "";
            }
            return sanitizer.Sanitize(CkHtml);
        }
        public static string GetPublicBreadcum(IUrlHelper url, HttpContext httpContext, long lgid)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string strCurrentPath = httpContext.Request.Path.Value.ToString().Replace("/Home/", "");

            {
                using (ICMSMenuResourceMasterService userMasterService = new CMSMenuResourceMasterService())
                {
                    {
                        List<CMSMenuResourceModel> lstMenuList = userMasterService.GetListFront(lgid).OrderByDescending(x => x.col_parent_id).ThenByDescending(n => n.MenuRank).ToList();
                        var mainModel = lstMenuList.FirstOrDefault(x => x.MenuURL == strCurrentPath);

                        stringBuilder.Append("");
                        stringBuilder.Append("<li>");
                        // stringBuilder.Append("<nav aria-label=\"breadcrumb\"'>");
                        // stringBuilder.Append("<ol class=\"breadcrumb\"'>");
                        if (lgid == 2)
                        {
                            stringBuilder.Append("<a title = \"Homepage\" href=\"" + url.Content("~/Index") + "\" ><i class=\"ti ti-home\"></i>&nbsp;&nbsp;હોમ</a>");

                            // stringBuilder.Append("<li class=\"breadcrumb-item\"><a href=\"" + url.Content("~/Index") + "\">હોમ</a></li>");
                        }
                        else
                        {
                            // stringBuilder.Append("<li class=\"breadcrumb-item\"><a href=\"" + url.Content("~/Index") + "\">Home</a></li>");
                            stringBuilder.Append("<a title = \"Homepage\" href=\"" + url.Content("~/Index") + "\" ><i class=\"ti ti-home\"></i>&nbsp;&nbsp;Home</a>");
                        }
                        stringBuilder.Append("</li>");
                        if (mainModel != null)
                        {
                            stringBuilder.Append(CreatePublicSubBreadCum(lstMenuList, mainModel.col_parent_id));
                           // stringBuilder.Append("<li class=\"breadcrumb-item active\"></li>");
                            //stringBuilder.Append("&nbsp;");
                            //stringBuilder.Append("<span class=\"ttm-bread-sep ttm-textcolor-white\">&nbsp; → &nbsp;</span><span class=\"ttm-textcolor-skincolor\">" + mainModel.MenuName + "</span>");
                            stringBuilder.Append("<li><span></span></li> <li><a href=\"#\">" + mainModel.MenuName + "</a></li>");
                        }
                       // stringBuilder.Append("</ol");
                       // stringBuilder.Append("</nav>");
                    }
                }

            }
            return stringBuilder.ToString();
        }
        private static string CreatePublicSubBreadCum(List<CMSMenuResourceModel> lstMenuList, long parentId)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var mainModel = lstMenuList.FirstOrDefault(x => x.Id == parentId);
            if (mainModel != null && parentId > 0)
            {                
                {
                    stringBuilder.Append(CreatePublicSubBreadCum(lstMenuList, mainModel.col_parent_id));
                    
                    stringBuilder.Append("<li><span></span></li> <li><a href=\"#\">" + mainModel.MenuName + "</a></li>");
                    //stringBuilder.Append("<li><span class=\"ttm-bread-sep ttm-textcolor-white\">&nbsp; → &nbsp;</span><span class=\"ttm-textcolor-skincolor\">" + mainModel.MenuName + "</span><li>");
                    //stringBuilder.Append("</li>");
                    //stringBuilder.Append("&nbsp;");
                }
            }
            else
            {
                return "";
            }
            return stringBuilder.ToString();
        }
        #endregion
        #region Public Quick Link


        public static string GetQuickLink(IUrlHelper url, HttpContext httpContext, long lgid)
        {

            StringBuilder stringBuilder = new StringBuilder();
            string strCurrentPath = httpContext.Request.Path.Value.ToString();

            {
                using (ICMSMenuResourceMasterService userMasterService = new CMSMenuResourceMasterService())
                {
                    if (strCurrentPath != "SiteMap")
                    {
                        List<CMSMenuResourceModel> lstMenuList = userMasterService.GetListFront(lgid).OrderBy(x => x.col_parent_id).ThenBy(n => n.MenuRank).ToList();

                        lstMenuList.ForEach(mainMenu =>
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
                        });
                        var mainModel = lstMenuList.FirstOrDefault(x => strCurrentPath.StartsWith(x.MenuURL));

                        if (mainModel != null)
                        {
                            stringBuilder.Append("<aside class=\"widget widget-nav-menu\">");
                            stringBuilder.Append("<ul>");

                            if (mainModel != null)
                            {
                                if (mainModel.col_menu_type != "2")
                                {
                                    var suubMenuList = lstMenuList.Where(x => x.col_parent_id != 0 || x.Id == mainModel.Id).ToList().Where(z => z.col_menu_type != "2" && z.col_menu_type != "3").Where(x => x.col_parent_id == mainModel.col_parent_id).ToList();

                                    foreach (var subMenu in suubMenuList)
                                    {
                                        string strPath = (string.IsNullOrWhiteSpace(subMenu.MenuURL) || subMenu.MenuURL == "#" ? "#" : (subMenu.MenuURL.ToLower().StartsWith("http") ? subMenu.MenuURL : url.Content("~" + subMenu.MenuURL))).Replace(@"\\", "\\");
                                        stringBuilder.Append("<li " + (strCurrentPath.StartsWith(subMenu.MenuURL) ? " class=\"active\"" : "") + "><a " + (subMenu.IsRedirect ? " target=\"_blank\"" : "") + " href='" + strPath + "'>" + subMenu.MenuName + "</a></li>");
                                    }
                                }
                                stringBuilder.Append("</ul>");
                                stringBuilder.Append("</aside>");
                            }
                        }
                    }
                    else
                    {
                        stringBuilder.Append("");
                    }
                }
            }
            return stringBuilder.ToString();
        }

        #endregion
        internal static string GetPathFrom(string strPath)
        {
            string strCurrentPath = "";
            using (ICMSMenuMasterService cMSMenuMasterService = new CMSMenuMasterService())
            {
                var data = cMSMenuMasterService.GetList().Where(x => x.MenuURL == strPath).FirstOrDefault();

                if (data != null)
                {
                    if (data.PageType == "1")
                    {
                        strCurrentPath = "/" + (data.MenuURL.ToString()) + "/d";
                    }
                    else
                    {
                        strCurrentPath = strPath;
                    }
                }
                else
                {
                    strCurrentPath = strPath;
                }
            }

            return strCurrentPath;
        }

        internal static string ValidateHomePage(string strPath)
        {
            string strCurrentPath = "";
            using (ICMSMenuMasterService cMSMenuMasterService = new CMSMenuMasterService())
            {
                var data = cMSMenuMasterService.GetList().Where(x => x.MenuURL == strPath).FirstOrDefault();

                if (data != null)
                {
                    if (data.PageType == "1")
                    {
                        strCurrentPath = "/" + (data.MenuURL.ToString()) + "/d";
                    }
                    else
                    {
                        strCurrentPath = strPath;
                    }
                }
                else
                {
                    strCurrentPath = strPath;
                }
            }

            return strCurrentPath;
        }

        internal static string GetHomePage(IUrlHelper url)
        {
            string strCurrentPath = "";
            using (ICMSMenuMasterService cMSMenuMasterService = new CMSMenuMasterService())
            {
                var data = cMSMenuMasterService.GetList().Where(x => x.IsHomePage).FirstOrDefault();

                if (data != null)
                {
                    if (data.PageType == "1")
                    {
                        strCurrentPath = (url.Content("~/Home/" + data.MenuURL.ToString()));
                        //strCurrentPath = "/" + (data.MenuURL.ToString()) + "/Index";
                    }
                    //else if(data.PageType=="0")
                    //{
                    //    strCurrentPath = (url.Content("~/" + data.MenuURL.ToString()));
                    //}
                }
                else
                {
                    strCurrentPath = url.Content("~/Account/Index");
                }
            }

            return strCurrentPath;
        }

        private static string GetParentIdString(IUrlHelper urlHelper, MenuRightsMasterModel mainMenu, List<MenuRightsMasterModel> lstList, string strCurrentPath)
        {
            bool isActive = false;
            if (mainMenu.MenuURL.ToString().ToLower() == strCurrentPath.ToLower())
            {
                isActive = true;
            }
            StringBuilder strMenu = new StringBuilder();
            List<MenuRightsMasterModel> lstSubList = lstList.Where(x => x.ParentId == mainMenu.Id).OrderBy(x => x.ParentId).ThenBy(n => n.MenuRank).ToList();
            if (lstSubList.Count() > 0)
            {
                strMenu.Append("<li class='has-sub nav-item'><a href='#'><span data-i18n='' class='menu-title'>" + mainMenu.Name + "</span></a><ul class='menu-content'>");
                foreach (var mainMenus in lstSubList)
                {
                    if (lstSubList.Count() > 0)
                    {
                        strMenu.Append(GetParentIdString(urlHelper, mainMenus, lstList, strCurrentPath));
                    }
                    else
                    {
                        string strPath = mainMenu.MenuURL;
                        strMenu.Append("<li " + (strPath == strCurrentPath ? " class=\"active\"" : "") + " ><a href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : urlHelper.Content("~" + strPath)) + "' >" + mainMenu.Name + "</a></li>");
                    }
                }
                strMenu.Append("</ul></li>");
            }
            else
            {
                string strPath = mainMenu.MenuURL;
                strMenu.Append("<li " + (strPath == strCurrentPath ? " class=\"active\"" : "") + " ><a href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : urlHelper.Content("~" + strPath)) + "' >" + mainMenu.Name + "</a></li>");
            }

            return strMenu.ToString();
        }

        public static void GetPageRights(long lgRoleId, HttpContext httpContext)
        {
            using (IMenuRightsMasterService menuRightsMasterService = new MenuRightsMasterService())
            {
                PageRightsModel pageRightsModel = new PageRightsModel();
                var menuList = menuRightsMasterService.GetListByRoleId(lgRoleId).Where(x => x.MenuURL == httpContext.Request.Path.Value).FirstOrDefault();
                if (menuList != null)
                {
                    pageRightsModel.Insert = menuList.Insert;
                    pageRightsModel.Update = menuList.Update;
                    pageRightsModel.Delete = menuList.Delete;
                    pageRightsModel.View = menuList.View;

                    SessionWrapper.Set<PageRightsModel>(httpContext.Session, "PageRights", pageRightsModel);

                }
            }
        }

        public static PageRightsModel GetPageRightsCheck(ISession httpContext)
        {
            return SessionWrapper.Get<PageRightsModel>(httpContext, "PageRights");
        }

        public static string CreateMainLayoutMenu(IUrlHelper urlHelper, long lgRoleId, string strCurrentPath)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string strMainMenu = "";
                using (IUserMasterService userMasterService = new UserMasterService())
                using (IAdminMenuMasterService adminMenuMasterService = new AdminMenuMasterService())
                using (IMenuResourceMasterService menuResourceMasterService = new MenuResourceMasterService())
                using (IRoleMasterService roleMasterService = new RoleMasterService())
                using (IMenuRightsMasterService menuRightsMasterService = new MenuRightsMasterService())
                {

                    if (lgRoleId == 0)
                    {
                        ErrorLogger.Error(" Functions \n\r CreateMainLayoutMenu \n\r Role " + lgRoleId, "RoleId As 0", "Functions", "CreateMainLayoutMenu", "", true);
                        return "";
                    }
                    else if (string.IsNullOrWhiteSpace(strCurrentPath))
                    {
                        ErrorLogger.Error(" Functions \n\r CreateMainLayoutMenu \n\r Role " + lgRoleId, "CurrentPath As " + strCurrentPath, "Functions", "CreateMainLayoutMenu", "", true);
                        return "";
                    }
                    else
                    {
                        List<MenuRightsMasterModel> lstMenuList = menuRightsMasterService.GetListByRoleId(lgRoleId).OrderBy(x => x.ParentId).ThenBy(n => n.MenuRank).ToList();

                        foreach (MenuRightsMasterModel menu in lstMenuList.Where(x => x.ParentId == 0).OrderBy(x => x.ParentId).ThenBy(n => n.MenuRank).ToList())
                        {
                            stringBuilder.Append(GetParentIdString(urlHelper, menu, lstMenuList, strCurrentPath));
                        }
                    }
                    strMainMenu = stringBuilder.ToString();
                    return strMainMenu;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(" Functions \n\r CreateMainLayoutMenu \n\r Role " + lgRoleId, "RoleId As " + lgRoleId, "Functions", "CreateMainLayoutMenu", "", true);
                DapperConnection.ErrorLogEntry(ex.ToString(), "Functions", "CreateMainLayoutMenu", "Web", "");
            }
            return "";
        }

        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input[0].ToString().ToUpper() + input.Substring(1);
            }
        }

        public static string CreateMainUserLayoutMenu(IUrlHelper urlHelper, string strCurrentPath, long lgid)
        {
            try
            {

                StringBuilder stringBuilder = new StringBuilder();
                string strMainMenu = "";
                if (lgid == 0)
                {
                    lgid = 1;
                }
                using (ICMSMenuResourceMasterService cMSMenuResourceMasterService = new CMSMenuResourceMasterService())
                {
                    // < li class="active"><a href = "#" >< i aria-hidden="true"class="fa fa-home"></i> </a></li>

                    if(lgid == 1)
                        stringBuilder.Append("<li class=\"mega-menu-item active\"><a href=\"\\Index\"><i aria-hidden=\"true\" class=\"fa fa-home\"></i></a></li>");
                    else
                        stringBuilder.Append("<li class=\"mega-menu-item active\"><a href=\"\\Index\"><i aria-hidden=\"true\" class=\"fa fa-home\"></i></a></li>");

                    List<CMSMenuResourceModel> lstMenuList = cMSMenuResourceMasterService.GetListFront(lgid).Where(z => z.col_menu_type != "2" && z.col_menu_type != "3").OrderBy(x => x.col_parent_id).ThenBy(n => n.MenuRank).ToList();

                    foreach (CMSMenuResourceModel menu in lstMenuList.Where(x => x.col_parent_id == 0).OrderBy(x => x.col_parent_id).ThenBy(n => n.MenuRank).ToList())
                    {
                        stringBuilder.Append(GetUserParentIdString(urlHelper, menu, lstMenuList, strCurrentPath));
                    }

                    strMainMenu = stringBuilder.ToString();
                    return strMainMenu;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private static string GetUserParentIdString(IUrlHelper urlHelper, CMSMenuResourceModel mainMenu, List<CMSMenuResourceModel> lstList, string strCurrentPath)
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
            bool isActive = false;
            if (mainMenu.MenuURL.ToString().ToLower() == strCurrentPath.ToLower())
            {
                isActive = true;
            }
            StringBuilder strMenu = new StringBuilder();
            List<CMSMenuResourceModel> lstSubList = lstList.Where(x => x.col_parent_id == mainMenu.Id).OrderBy(x => x.col_parent_id).ToList();
            if (lstSubList.Count() > 0)
            {

                strMenu.Append("<li class='mega-menu-item'><a href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : (mainMenu.MenuURL.ToLower().StartsWith("http") ? strPath : urlHelper.Content("~" + strPath))) + "'>" + mainMenu.MenuName + "<span class=\"arrow fa fa-angle-down\"></span> </a><ul class='mega-submenu'>");
                foreach (var mainMenus in lstSubList)
                {
                    if (lstSubList.Count() > 0)
                    {
                        strMenu.Append(GetUserParentIdString(urlHelper, mainMenus, lstList, strCurrentPath));
                    }
                    else
                    {
                        strMenu.Append("<li " + (strPath == strCurrentPath ? " class=\"active mega-menu-item\"" : "") + " ><a " + (mainMenu.IsRedirect ? " target=\"_blank\"" : "") + " href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : (mainMenu.MenuURL.ToLower().StartsWith("http") ? strPath : urlHelper.Content("~" + strPath))) + "' class='active'>" + mainMenu.MenuName + "</a></li>");
                    }

                }
                strMenu.Append("</ul></li>");
            }
            else
            {
                strMenu.Append("<li " + (strPath == strCurrentPath ? " class=\"active dropdown\"" : "") + " ><a " + (mainMenu.IsRedirect ? " target=\"_blank\"" : "") + " href='" + (string.IsNullOrWhiteSpace(strPath) || strPath == "#" ? "#" : (mainMenu.MenuURL.ToLower().StartsWith("http") ? strPath : urlHelper.Content("~" + strPath))) + "' class='active'>" + mainMenu.MenuName + "</a></li>");
            }

            return strMenu.ToString();
        }

        public static string GetAllNewsByType(IUrlHelper urlHelper, long LangId, string strCurrentPath, string Newstype)
        {
            try
            {
                string strnews = "";
                if (LangId == 0)
                {
                    LangId = 1;
                }
                using (INewsMasterService NewsMasterService = new NewsMasterService())
                {
                    List<NewsModel> lstNewsList = NewsMasterService.GetList(LangId).Where(x => x.NewsTypeId == Newstype).ToList();

                    if (lstNewsList.Count() <= 0)
                    {
                        lstNewsList = NewsMasterService.GetList(1).Where(x => x.NewsTypeId == Newstype).ToList();
                    }

                    strnews = lstNewsList[0].NewsDesc.ToString();
                }

                return strnews;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetAllNews(IUrlHelper urlHelper, long LangId, string strCurrentPath, string Newstype)
        {
            try
            {
                if (LangId == 0)
                {
                    LangId = 1;
                }
                StringBuilder stringBuilder = new StringBuilder();
                string strnews_R = "";

                using (INewsMasterService NewsMasterService = new NewsMasterService())
                {
                    List<NewsModel> lstNewsList = NewsMasterService.GetListFront(LangId).Where(x => x.NewsTypeId == Newstype).ToList();

                    if (lstNewsList.Count() <= 0)
                    {
                        lstNewsList = NewsMasterService.GetListFront(LangId).Where(x => x.NewsTypeId == Newstype).ToList();
                    }

                    foreach (var item in lstNewsList)
                    {
                        string strDate = "";
                        if (item.NewsStartDate != null)
                        {
                            item.NewsDate = item.NewsStartDate.Value.ToString("dd-MM-yyyy").Replace("-", "/");
                            strDate = item.NewsDate;
                        }
                        string strpath = urlHelper.Content("~/");

                        if (Newstype == "T")
                        {
                            strpath = strpath + "Tender/Index";
                        }
                        else if (Newstype == "I")
                        {
                            strpath = strpath + "News/Index";
                        }
                        else if (Newstype == "W")
                        {
                            strpath = strpath + "WhatsNew/Index?type=news";
                        }
                        else if (Newstype == "P")
                        {
                            strpath = strpath + "WhatsNew/Index?type=Past";
                        }
                        else
                        {
                            strpath = strpath + "WhatsNew/Index?type=Audio";
                        }

                        strnews_R =
                                    "<li><div class=\"about-icon-box\"><a class=\"content\"><h3><i class=\"fas fa-calendar\"></i> <span>" + strDate + "</span></h3> " +
                                    "<p><a href='" + strpath + "'>" + item.NewsDesc.Replace("<p>", "").Replace("</p>", "") + "</a> </p><p class=\"download\"><a href='" + strpath + "'><i class=\"fas fa-download\"></i> Download</a></p> </a></div></li>";

                        stringBuilder = stringBuilder.Append(strnews_R);
                    }
                }

                strnews_R = stringBuilder.ToString();
                return strnews_R;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public static string UpdateDate()
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string str_Date = "";

                using (ICMSMenuResourceMasterService MenuMasterService = new CMSMenuResourceMasterService())
                {
                    var data = MenuMasterService.UpdateSiteDate();

                    str_Date = "<span> Last Updated : " + data.UpdatedDate.ToString("dd/MM/yyyy") + "</span>&nbsp;&nbsp;&nbsp;&nbsp;";
                    stringBuilder = stringBuilder.Append(str_Date);

                }
                str_Date = stringBuilder.ToString();

                return str_Date;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public static string Publication(IUrlHelper urlHelper, string CateID)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string strMainMenu = "";
                using (ICategoryServices CategoryServices = new CategoryMasterService())
                {
                    List<CategoryModel> lstMenuList = CategoryServices.GetSubList().Where(x => x.CategoryID == Convert.ToInt64(CateID)).ToList();
                    int i = 1;

                    stringBuilder.Append(PublicationDetail(urlHelper, CateID));
                    stringBuilder.Append("<div class='col-lg-3 col-md-3 col-sm-12 col-12'>");
                    stringBuilder.Append("<div class='sidebar'>");
                    stringBuilder.Append("<div class='card category-widget'>");
                    stringBuilder.Append("<div class='card-header'>");
                    stringBuilder.Append("<h4 class='card-title'>Publication</h4>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("<div class='card-body'>");
                    stringBuilder.Append("<div class='categories nav nav-stacked flex-column nav-pills' id='v-pills-tab' role='tablist' aria-orientation='vertical'>");
                    stringBuilder.Append("<ul class='categories nav nav-pills nav-stacked flex-column'>");
                    foreach (CategoryModel menu in lstMenuList.Where(x => x.CategoryID == Convert.ToInt64(CateID)).OrderBy(x => x.Id).ToList())
                    {
                        if (i == 1)
                        {
                            stringBuilder.Append("<li><a class=\"show active\" data-bs-toggle=\"tab\" href=\"#tab" + i + "\"><i class=\"fas fa-hand-point-right\"></i>" + menu.SubCategoryName + "</a></li>");
                        }
                        else
                        {
                            stringBuilder.Append("<li><a data-bs-toggle=\"tab\" href=\"#tab" + i + "\"><i class=\"fas fa-hand-point-right\"></i>" + menu.SubCategoryName + "</a></li>");
                        }
                        i++;
                    }
                    stringBuilder.Append("</ul>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    strMainMenu = stringBuilder.ToString();
                    return strMainMenu;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return "";
        }

        public static string PublicationDetail(IUrlHelper urlHelper, string CateID)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string strnews_R = "";
            using (ICategoryServices CategoryServices = new CategoryMasterService())
            {
                var PrMasterList = CategoryServices.GetPRMasterList().Where(x => x.CategoryID == Convert.ToInt64(CateID)).ToList();
                List<CategoryModel> lstPrList = PrMasterList.GroupBy(x => x.SubCategoryID).Select(x => new CategoryModel { SubCategoryID = x.Key }).OrderBy(x => x.SubCategoryID).ToList();
                int i = 1;
                int j = 1;

                stringBuilder.Append("<div class='col-lg-9 col-md-9 col-sm-12 col-12'>");
                stringBuilder.Append("<div class='tab-content publication-resource-content'>");

                foreach (var item in lstPrList)
                {
                    if (i == 1)
                    {
                        stringBuilder.Append("<div class='tab-pane fade active show' id=tab" + i + ">");
                    }
                    else
                    {
                        stringBuilder.Append("<div class='tab-pane fade' id=tab" + i + ">");
                    }
                    stringBuilder.Append("<div class=tab-data>");
                    stringBuilder.Append("<ul class=p-0>");
                    var List = PrMasterList.Where(c => c.SubCategoryID == item.SubCategoryID);
                    foreach (var item1 in List)
                    {
                        if (item.SubCategoryID == item1.SubCategoryID)
                        {
                            stringBuilder.Append("<li>");
                            stringBuilder.Append("<h6><span>" + j + "</span>" + item1.Title + "</h6>");
                            stringBuilder.Append("<div class=icon-box>");
                            stringBuilder.Append("<a href=" + item1.FilePath + " target=_blank><i class='fa fa-file-pdf' aria-hidden=true></i></a>");
                            stringBuilder.Append("</div>");
                            stringBuilder.Append("</li>");
                        }
                        j++;
                    }
                    stringBuilder.Append("</ul>");
                    stringBuilder.Append("</div>");
                    stringBuilder.Append("</div>");
                    i++;
                    j = 1;
                }
                stringBuilder.Append("</div>");
                stringBuilder.Append("</div>");
            }
            strnews_R = stringBuilder.ToString();
            return strnews_R;
        }

        public static bool ValidControlValue(dynamic controlValue, ControlInputType type = ControlInputType.none)
        {
            bool allow = false;
            try
            {
                Regex regexGlobalValidation = new Regex(regGlobalValidation);
                Regex regexName = new Regex(regName);
                Regex regexMobileNo = new Regex(regMobileNo);
                Regex regexPincode = new Regex(regPincode);
                Regex regexNumber = new Regex(regNumber);
                Regex regexEmail = new Regex(@regEmail.Replace(@"\\", @"\"));
                Regex regexPassword = new Regex(@regPassword.Replace(@"\\", @"\"));
                string strControlValue = controlValue != null ? controlValue.ToString().Trim() : "";

                if (!string.IsNullOrEmpty(strControlValue) && regexGlobalValidation.IsMatch(strControlValue))
                {
                    if (type == ControlInputType.text)
                    {
                        if (regexName.IsMatch(strControlValue))
                        {
                            allow = true;
                        }
                    }
                    else if (type == ControlInputType.email)
                    {
                        if (regexEmail.IsMatch(strControlValue))
                        {
                            allow = true;
                        }
                    }
                    else if (type == ControlInputType.mobileno)
                    {
                        if (regexMobileNo.IsMatch(strControlValue))
                        {
                            allow = true;
                        }
                    }
                    else if (type == ControlInputType.password)
                    {
                        if (regexPassword.IsMatch(strControlValue))
                        {
                            allow = true;
                        }
                    }
                    else if (type == ControlInputType.pincode)
                    {
                        if (regexPincode.IsMatch(strControlValue))
                        {
                            allow = true;
                        }
                    }
                    else if (type == ControlInputType.dropdown)
                    {
                        long dropdownValue = 0;
                        long.TryParse(strControlValue, out dropdownValue);
                        if (dropdownValue > 0)
                        {
                            allow = true;
                        }
                    }
                    else if (type == ControlInputType.date)
                    {
                        try
                        {
                            DateTime dt = DateTime.ParseExact(strControlValue, dateFormat, CultureInfo.InvariantCulture);
                            allow = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else if (type == ControlInputType.time)
                    {
                        try
                        {
                            DateTime dt = DateTime.ParseExact(strControlValue, "hh:mm tt", CultureInfo.InvariantCulture);
                            allow = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        allow = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allow;
        }

        public static bool ValidLength(dynamic controlValue)
        {
            bool allow = false;
            try
            {
                if (controlValue != null && controlValue.Length > 0)
                {
                    allow = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return allow;
        }
        public static string Search(IUrlHelper url, string search, string lngId)
        {
            try
            {
                StringBuilder stringMainBuilder = new StringBuilder();
                string strserch_R = "";

                using (IGlobleSerchService SerchServices = new GlobleSerchService())
                {
                    List<GlobleSerchModel> lstGbList = SerchServices.GetList(search, Convert.ToInt64(lngId));

                    ListtoDataTableConverter converter = new ListtoDataTableConverter();
                    DataTable dt = converter.ToDataTable(lstGbList);
                    if (dt.Rows.Count > 0)
                    {
                        stringMainBuilder.Append("");
                        foreach (DataRow row in dt.Rows)
                        {
                            string pagename = row["PageName"].ToString() + "/" /*+ row["Id"].ToString()*/;
                            stringMainBuilder.Append("<div class='col-md-3'>");
                            stringMainBuilder.Append("    <div class='card '>");
                            stringMainBuilder.Append("        <div class='card-body'>");
                            stringMainBuilder.Append("            <h5 class='card-title'> <a href='" + url.Content("~/" + pagename) + "'> " + row["PageName"] + "</a></h5>");
                            stringMainBuilder.Append("            <p>");
                            stringMainBuilder.Append("               " + row["MetaDescription"].ToString());
                            stringMainBuilder.Append("            </p>");
                            stringMainBuilder.Append("        </div>");
                            stringMainBuilder.Append("    </div>");
                            stringMainBuilder.Append("</div>");
                        }
                    }
                    else
                    {
                        stringMainBuilder.Append("");
                        stringMainBuilder.Append("<div class='col-md-3'>");
                        stringMainBuilder.Append("    <div class='card '>");
                        stringMainBuilder.Append("        <div class='card-body'>");
                        stringMainBuilder.Append("            <h6 class='card-title'> <a href=''> Record not found !</a></h6>");
                        stringMainBuilder.Append("            <p>");
                        stringMainBuilder.Append("               ");
                        stringMainBuilder.Append("            </p>");
                        stringMainBuilder.Append("        </div>");
                        stringMainBuilder.Append("    </div>");
                        stringMainBuilder.Append("</div>");
                    }
                }
                strserch_R = stringMainBuilder.ToString();
                return strserch_R;
            }
            catch (Exception ex)
            {
                throw;
            }
            return "";
        }
        public static string CSSMasterDomain(IUrlHelper url)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string CSSCODE = "";
                using (ICssMasterService CSSMasterService = new CssMasterService())
                {
                    var data = CSSMasterService.CSSMasterSiteData();
                    foreach (var item in data)
                    {
                        string strPath = url.Content("~/CSSCustom/" + item.Title + ".css");
                        stringBuilder.Append("\r\n <link rel=\"stylesheet\" type=\"text/css\" href=" + strPath + ">");
                    }
                    stringBuilder = stringBuilder.Append(CSSCODE);
                }
                CSSCODE = stringBuilder.ToString();

                return CSSCODE;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public class ListtoDataTableConverter
        {
            public DataTable ToDataTable<T>(List<T> items)
            {
                DataTable dataTable = new DataTable(typeof(T).Name);
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name);
                }
                foreach (T item in items)
                {
                    var values = new object[Props.Length];
                    for (int i = 0; i < Props.Length; i++)
                    {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                    }
                    dataTable.Rows.Add(values);
                }
                //put a breakpoint here and check datatable
                return dataTable;
            }
        }
    }
}
