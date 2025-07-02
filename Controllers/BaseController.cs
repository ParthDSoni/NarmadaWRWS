using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NWRWS.Webs.Controllers
{
    public abstract class BaseController<T> : Controller where T : BaseController<T>
    {
        protected IHttpClientFactory httpClientFactory { get; set; }

        private ILogger<T> _logger;
        protected ILogger<T> Logger => _logger ?? (_logger = HttpContext.RequestServices.GetService<ILogger<T>>());
        protected IRoleMasterService _objIRoleMasterService { get; set; }
        protected IPayScalServices _objPayScalServices { get; set; }
        protected IRoleMasterService objIRoleMasterService => _objIRoleMasterService ?? (_objIRoleMasterService = HttpContext.RequestServices.GetService<IRoleMasterService>());
        protected IPayScalServices objPayScalServices => _objPayScalServices ?? (_objPayScalServices = HttpContext.RequestServices.GetService<IPayScalServices>());
        public SessionUserModel UserModel
        {
            get { return SessionWrapper.Get<SessionUserModel>(this.HttpContext.Session, "UserDetails"); }
            set { SessionWrapper.Set<SessionUserModel>(this.HttpContext.Session, "UserDetails", value); }
        }

        public JsonResponseModel DataIssue
        {
            get { return SessionWrapper.Get<JsonResponseModel>(this.HttpContext.Session, "DataIssue"); }
            set { SessionWrapper.Set<JsonResponseModel>(this.HttpContext.Session, "DataIssue", value); }
        }


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            long LoginSessionCookie = SessionWrapper.Get<long>(this.HttpContext.Session, "LoginCookie");
            Request.Cookies.TryGetValue("LoginCookie", out string LoginCookie);
            string requestedURL = HttpContext.Request.Path.Value;
            if (LoginCookie == LoginSessionCookie.ToString())
            {
                if (UserModel != null)
                {
                    Common.Functions.GetPageRights(UserModel.RoleId, HttpContext);
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Logout" }, { "area", "" } });
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Account" }, { "action", "Logout" }, { "area", "" } });
            }
            base.OnActionExecuting(filterContext);
        }

        public BaseController()
        {
            if (this.HttpContext == null || this.HttpContext.Session == null || UserModel == null)
            {
                RedirectToAction("Account", "Logout");
            }
        }
        public BaseController(IHttpClientFactory _httpClientFactory)
        {
            if (this.HttpContext == null || this.HttpContext.Session == null || UserModel == null)
            {
                RedirectToAction("Account", "Logout");
            }

            this.httpClientFactory = _httpClientFactory;
        }
        public bool ValidControlValue(dynamic controlValue, ControlInputType type = ControlInputType.none)
        {
            return Common.Functions.ValidControlValue(controlValue, type);
        }
        public bool ValidLength(dynamic controlValue)
        {
            return Common.Functions.ValidLength(controlValue);
        }
    }
}
