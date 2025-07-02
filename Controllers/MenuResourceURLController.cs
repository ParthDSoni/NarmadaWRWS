using CommonCMS.Webs.Common;
using CommonCMS.IService.Service;
using CommonCMS.Model.Service;
using CommonCMS.Model.System;
using CommonCMS.Webs.Models;
using Microsoft.AspNetCore.Mvc;
using CommonCMS.Common;

namespace CommonCMS.Webs.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class MenuResourceURLController : BaseController<MenuResourceURLController>
    {

        private IMenuResourceMasterService objIMenuResourceMasterService { get; set; }

        public MenuResourceURLController(IMenuResourceMasterService _menuResourceMasterService)
        {
            this.objIMenuResourceMasterService = _menuResourceMasterService;
        }

        #region Menu Resource Master

        [Route("MenuResource")]
        public IActionResult MenuResource()
        {
            try
            {
                Common.Functions.GetPageRights(UserModel.RoleId, HttpContext);
                var model = Common.Functions.GetPageRightsCheck(HttpContext.Session);
                if (model == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
        }

        [Route("SaveMenuResouceData")]
        [HttpPost]
        public JsonResult SaveMenuResouceData(MenuResourceFormModel objModel)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    MenuResourceMasterModel menuResourceMasterModel = new MenuResourceMasterModel();
                    menuResourceMasterModel.Id = objModel.Id;
                    menuResourceMasterModel.MenuName = objModel.MenuName;
                    menuResourceMasterModel.MenuURL = objModel.MenuURL;
                    menuResourceMasterModel.IsActive = objModel.IsActive;
                    menuResourceMasterModel.CreatedBy = UserModel.Username;

                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Insert && objModel.Id == 0)
                    {
                        objreturn = objIMenuResourceMasterService.AddOrUpdate(menuResourceMasterModel);
                    }
                    else if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Update && objModel.Id != 0)
                    {
                        objreturn = objIMenuResourceMasterService.AddOrUpdate(menuResourceMasterModel);
                    }
                    else
                    {
                        objreturn.strMessage = "You Don't have Rights perform this action.";
                        objreturn.isError = true;
                        objreturn.type = PopupMessageType.error.ToString();
                    }

                }
                else
                {
                    objreturn.strMessage = "Form Input is not valid";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
                objreturn.strMessage = "Record not saved, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(objreturn);
        }

        [Route("GetMenuResouceData")]
        [HttpPost]
        public JsonResult GetMenuResouceData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            List<MenuResourceMasterModel> lsdata = new List<MenuResourceMasterModel>();
            try
            {
                lsdata = objIMenuResourceMasterService.GetList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(new { draw = draw, recordsFiltered = lsdata.Count(), recordsTotal = lsdata.Count(), data = lsdata });
        }

        [Route("GetMenuResouceDataDetails")]
        [HttpPost]
        public JsonResult GetMenuResouceDataDetails(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            MenuResourceMasterModel lsdata = new MenuResourceMasterModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    objreturn.result = lsdata = objIMenuResourceMasterService.Get(lgid);
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

        [Route("DeleteMenuResouceData")]
        [HttpPost]
        public JsonResult DeleteMenuResouceData(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Delete)
                    {
                        objreturn = objIMenuResourceMasterService.Delete(lgid, UserModel.Username);
                    }
                    else
                    {
                        objreturn.strMessage = "You Don't have Rights perform this action.";
                        objreturn.isError = true;
                        objreturn.type = PopupMessageType.error.ToString();
                    }
                }
                else
                {
                    objreturn.strMessage = "Record not deleted, Try again";
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
                objreturn.strMessage = "Record not deleted, Try again";
                objreturn.isError = true;
                objreturn.type = PopupMessageType.error.ToString();
            }
            return Json(objreturn);
        }

        #endregion

    }
}
