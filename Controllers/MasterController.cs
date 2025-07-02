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
    public class MasterController : BaseController<MasterController>
    {

        private IRoleMasterService objIRoleMasterService { get; set; }

        public MasterController(IRoleMasterService _roleMasterService)
        {
            this.objIRoleMasterService = _roleMasterService;
        }

        #region Role Master

        [Route("RoleMaster")]
        public IActionResult RoleMaster()
        {
            try
            {
                Common.Functions.GetPageRights(UserModel.RoleId, HttpContext);
                var model = Common.Functions.GetPageRightsCheck(HttpContext.Session);
                if (model == null)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        [Route("SaveRoleData")]
        [HttpPost]
        public JsonResult SaveRoleData(RoleMasterFormModel objModel)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    RoleMasterModel roleMasterModel = new RoleMasterModel();
                    roleMasterModel.Id = objModel.Id;
                    roleMasterModel.RoleName = objModel.RoleName;
                    roleMasterModel.IsActive = objModel.IsActive;
                    roleMasterModel.CreatedBy = UserModel.Username;

                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Insert && objModel.Id==0)
                    {
                        objreturn = objIRoleMasterService.AddOrUpdate(roleMasterModel);
                    }
                    else if(Common.Functions.GetPageRightsCheck(HttpContext.Session).Update && objModel.Id!=0)
                    {
                        objreturn = objIRoleMasterService.AddOrUpdate(roleMasterModel);
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

        //[IgnoreAntiforgeryToken]
        [HttpPost]
        [Route("GetRoleData")]
        public JsonResult GetRoleData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            List<RoleMasterModel> lsdata = new List<RoleMasterModel>();
            try
            {
                lsdata = objIRoleMasterService.GetList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(new { draw = draw, recordsFiltered = lsdata.Count(), recordsTotal = lsdata.Count(), data = lsdata });
        }

        [Route("GetRoleDataDetails")]
        [HttpPost]
        public JsonResult GetRoleDetails(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            RoleMasterModel lsdata = new RoleMasterModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                   objreturn.result= lsdata = objIRoleMasterService.Get(lgid);
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

        [Route("DeleteRoleData")]
        [HttpPost]
        public JsonResult DeleteRoleData(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    if(Common.Functions.GetPageRightsCheck(HttpContext.Session).Delete)
                    {
                        objreturn = objIRoleMasterService.Delete(lgid, UserModel.Username);
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
