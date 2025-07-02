using CommonCMS.Webs.Common;
using CommonCMS.IService.Service;
using CommonCMS.Model.Service;
using CommonCMS.Model.System;
using CommonCMS.Webs.Models;
using Microsoft.AspNetCore.Mvc;
using CommonCMS.Common;

namespace CommonCMS.Webs.Controllers
{
    public class UserDetailMasterController : BaseController<UserDetailMasterController>
    {
        private IUserMasterService objUserMasterService { get; set; }
        private IRoleMasterService objRoleMasterService { get; set; }

        public UserDetailMasterController(IUserMasterService _userMasterService, IRoleMasterService _roleMasterService)
        {
            this.objUserMasterService = _userMasterService;
            this.objRoleMasterService = _roleMasterService;
        }


        #region User Master

        [Route("UserMaster")]
        public IActionResult UserMaster()
        {
            try
            {
                UserMasterFormModel userMasterFormModel = new UserMasterFormModel();
                Common.Functions.GetPageRights(UserModel.RoleId, HttpContext);
                var model = Common.Functions.GetPageRightsCheck(HttpContext.Session);
                if (model == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                userMasterFormModel.RoleList = objRoleMasterService.GetList().Select(x => new ListItem { Text = x.RoleName, Value = x.Id.ToString() }).ToList();
                return View(userMasterFormModel);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        [Route("SaveUserData")]
        [HttpPost]
        public JsonResult SaveUserData(UserMasterFrontModel objModel)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    UserMasterModel userMasterModel = new UserMasterModel();
                    userMasterModel.Id = objModel.Id;
                    userMasterModel.FirstName = objModel.FirstName;
                    userMasterModel.LastName = objModel.LastName;
                    userMasterModel.Username = CommonCMS.Common.Functions.FrontDecrypt(objModel.Username);
                    userMasterModel.UserPassword = CommonCMS.Common.Functions.Encrypt(CommonCMS.Common.Functions.FrontDecrypt(objModel.UserPassword));
                    userMasterModel.RoleId = objModel.RoleId;
                    userMasterModel.Email = objModel.Email;
                    userMasterModel.PhoneNo = objModel.PhoneNo;
                    userMasterModel.IsActive = objModel.IsActive;
                    userMasterModel.CreatedBy = UserModel.Username;

                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Insert && objModel.Id == 0)
                    {
                        objreturn = objUserMasterService.AddOrUpdate(userMasterModel);
                    }
                    else if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Update && objModel.Id != 0)
                    {
                        objreturn = objUserMasterService.AddOrUpdate(userMasterModel);
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

        [Route("GetUserData")]
        [HttpPost]
        public JsonResult GetUserData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            List<UserMasterModel> lsdata = new List<UserMasterModel>();
            try
            {
                lsdata = objUserMasterService.GetList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(new { draw = draw, recordsFiltered = lsdata.Count(), recordsTotal = lsdata.Count(), data = lsdata });
        }

        [Route("GetUserDataDetails")]
        [HttpPost]
        public JsonResult GetUserDataDetails(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            UserMasterModel lsdata = new UserMasterModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    objreturn.result = lsdata = objUserMasterService.Get(lgid);
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

        [Route("DeleteUserData")]
        [HttpPost]
        public JsonResult DeleteUserData(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Delete)
                    {
                        objreturn = objUserMasterService.Delete(lgid, UserModel.Username);
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
