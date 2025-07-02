using NWRWS.Common;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using Microsoft.AspNetCore.Mvc;
using NWRWS.IService.Service;

namespace NWRWS.Webs.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class FeedBackController : Controller
    {
        #region Controller Variable
        private IFeedbackServices objFeedbackService { get; set; }
        #endregion

        #region Controller Constructor
        public FeedBackController(IFeedbackServices _objFeedbackService)
        {
            objFeedbackService = _objFeedbackService;
        }
        #endregion

        [Route("/FeedBack")]
        public IActionResult FeedBack()
        {
            return View();
        }

        #region Save Method
        [Route("/Admin/AddFeedback")]
        public JsonResult AddFeedback(Feedback objModel)
        {
            JsonResponseModel objreturn = new JsonResponseModel();
            try
            {
                if (ModelState.IsValid)
                {
                    string[] srtEmail = Functions.FrontDecrypt(objModel.hfEmail).Split("--exegil--");
                    objModel.Email = srtEmail[0];
                    objModel.Captcha = srtEmail[1];

                    if (Captcha.ValidateCaptchaCode(objModel.Captcha, HttpContext))
                    {
                        objreturn = objFeedbackService.AddFeedback(objModel);
                    }
                    else
                    {
                        objreturn.strMessage = "Captcha Is not Match";
                        objreturn.isError = true;
                        objreturn.type = PopupMessageType.error.ToString();
                    }
                }
                else
                {
                    string strMessage = ModelState.OrderBy(kvp => typeof(Feedback)
                                        .GetProperties()
                                        .Select(p => p.Name)
                                        .ToList()
                                        .IndexOf(kvp.Key))
                                        .SelectMany(ms => ms.Value.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .FirstOrDefault() ?? "Invalid Data";
                    objreturn.strMessage = strMessage;
                    objreturn.isError = true;
                    objreturn.type = PopupMessageType.error.ToString();
                    ModelState.Clear();
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

        #endregion
    }
}
