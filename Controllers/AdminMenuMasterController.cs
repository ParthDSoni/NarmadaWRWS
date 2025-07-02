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
    public class AdminMenuMasterController : BaseController<AdminMenuMasterController>
    {
        private IAdminMenuMasterService objAdminMenuMasterService { get; set; }

        private IMenuResourceMasterService objMenuResourceMasterService { get; set; }

        public AdminMenuMasterController(IAdminMenuMasterService _adminMenuMasterService, IMenuResourceMasterService _menuResourceMasterService)
        {
            this.objAdminMenuMasterService = _adminMenuMasterService;
            this.objMenuResourceMasterService = _menuResourceMasterService;
        }

        [Route("AdminMenu")]
        public IActionResult AdminMenu()
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
            return View();
        }


        [Route("SaveAdminMenuData")]
        [HttpPost]
        public JsonResult SaveAdminMenuData(AdminMenuFromModel objModel)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    AdminMenuMasterModel adminMenuMasterModel = new AdminMenuMasterModel();
                    adminMenuMasterModel.Id = objModel.Id;
                    adminMenuMasterModel.MenuId = objModel.MenuId;
                    adminMenuMasterModel.Name = objModel.Name;
                    adminMenuMasterModel.ParentId = (long)objModel.ParentId;
                    adminMenuMasterModel.MenuType = objModel.MenuType;
                    adminMenuMasterModel.MenuRank = objModel.MenuRank;
                    adminMenuMasterModel.IsActive = objModel.IsActive;
                    adminMenuMasterModel.CreatedBy = UserModel.Username;

                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Insert && objModel.Id == 0)
                    {
                        objreturn = objAdminMenuMasterService.AddOrUpdate(adminMenuMasterModel);
                    }
                    else if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Update && objModel.Id != 0)
                    {
                        objreturn = objAdminMenuMasterService.AddOrUpdate(adminMenuMasterModel);
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

        [HttpPost]
        [Route("GetAdminMenuData")]
        public JsonResult GetAdminMenuData()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            try
            {
                var lsdata = objAdminMenuMasterService.GetList();
                lsdata.ForEach(x =>
                {
                    var mainMenu = Enum.GetValues(typeof(MenuType)).Cast<MenuType>().Select(d => new ListItem { Text = d.ToString(), Value = ((int)d).ToString() }).Where(y=> y.Value==x.MenuType).FirstOrDefault();
                    x.MenuType = mainMenu.Text;
                });
                return Json(new { draw = draw, recordsFiltered = lsdata.Count(), recordsTotal = lsdata.Count(), data = lsdata });
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);

                return Json("");
            }

        }



        [HttpPost]
        [Route("BindMenu")]
        public JsonResult BindMenu()
        {

            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.Add(new ListItem { Text = "-- Select Menu --" });
                lsdata.AddRange(objMenuResourceMasterService.GetList().Select(x => new ListItem { Text = x.MenuName, Value = x.Id.ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        [HttpPost]
        [Route("BindMenuType")]
        public JsonResult BindMenuType()
        {

            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.Add(new ListItem { Text = "-- Select Menu Type --" });
                lsdata.AddRange(Enum.GetValues(typeof(MenuType)).Cast<MenuType>().Select(d => new ListItem { Text = d.ToString(), Value = ((int)d).ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        [HttpPost]
        [Route("BindParentMenu")]
        public JsonResult BindParentMenu(long? lgId)
        {

            List<ListItem> lsdata = new List<ListItem>();
            try
            {
                lsdata.Add(new ListItem { Text = "-- Select Parent Menu --" });
                if (lgId.HasValue)
                {
                    if(lgId.Value > 0)
                    {
                        lsdata.AddRange(objAdminMenuMasterService.GetList().Where(x => x.Id != lgId).Select(x => new ListItem { Text = x.Name, Value = x.Id.ToString() }).ToList());
                    }
                    else
                    {
                        lsdata.AddRange(objAdminMenuMasterService.GetList().Select(x => new ListItem { Text = x.Name, Value = x.Id.ToString() }).ToList());
                    }
                }
                else
                {
                    lsdata.AddRange(objAdminMenuMasterService.GetList().Select(x => new ListItem { Text = x.Name, Value = x.Id.ToString() }).ToList());
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }



        [Route("GetAdminMenuDataDetails")]
        [HttpPost]
        public JsonResult GetAdminMenuDetails(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    objreturn.result = objAdminMenuMasterService.Get(lgid);
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

        [Route("DeleteAdminMenuData")]
        [HttpPost]
        public JsonResult DeleteAdminMenuData(string id)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(id), out long lgid))
                {
                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Delete)
                    {
                        objreturn = objAdminMenuMasterService.Delete(lgid, UserModel.Username);
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


        [Route("AdminMenuSwapDetails")]
        [HttpPost]
        public JsonResult AdminMenuSwapDetails(string rank, string dir)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                dir = CommonCMS.Common.Functions.FrontDecrypt(dir);
                if (long.TryParse(CommonCMS.Common.Functions.FrontDecrypt(rank), out long lgid))
                {
                    if (Common.Functions.GetPageRightsCheck(HttpContext.Session).Update)
                    {
                        objreturn = objAdminMenuMasterService.SwapSequance(lgid, dir,UserModel.Username);
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
                    objreturn.strMessage = "Record not Swap, Try again";
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

    }
}
