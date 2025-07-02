
using NWRWS.Common;
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;

namespace NWRWS.Services.Service
{
    public class AccountService : IAccountService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public AccountService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public bool LogInValidation(string strUserName, string strPassword, string userLogDetails, out SessionUserModel sessionUserModel, out string strErrorMessage)
        {
            bool isError = true;
            strErrorMessage = "";
            sessionUserModel = null;


            try
            {
                if (string.IsNullOrWhiteSpace(strUserName))
                {
                    strErrorMessage = "Please Enter UserName.";
                    isError = false;
                    return isError;

                }
                if (string.IsNullOrWhiteSpace(strPassword))
                {
                    strErrorMessage = "Please Enter Password.";
                    isError = false;
                    return isError;
                }

                strUserName = Functions.FrontDecrypt(strUserName);
                strPassword = (strPassword);

                strPassword = Functions.Encrypt(strPassword);

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("P_userusername", strUserName);
                dictionary.Add("P_userpassword", strPassword);

                var userData = dapperConnection.GetListResult<SessionUserModel>("GetLoginUsersMaster", CommandType.StoredProcedure, dictionary).ToList();
                if (userData != null)
                {
                    if (userData.Count() > 0)
                    {
                        sessionUserModel = userData.FirstOrDefault();
                        //db.InsertUserLogInLogDetails(userData.FirstOrDefault().Id, userLogDetails);
                    }
                    else
                    {
                        strErrorMessage = "No such username or password";
                        isError = false;
                    }
                }
                else
                {
                    strErrorMessage = "No such username or password";
                    isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");
                strErrorMessage = ex.Message;
                isError = false;
            }


            return isError;
        }

        class mdl
        {
            public bool AllowLogin { get; set; }
        }

        public async Task<JsonResponseModel> CheckUserAlreadyLogin(long Userid, string strIPAddress)
        {
            //bool isError = true;
            JsonResponseModel model = new JsonResponseModel();
            model.isError = true;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pUserId", Userid);
                dictionary.Add("pIPAddress", strIPAddress);

                var userData = dapperConnection.GetListResult<mdl>("CheckUserDetails", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (userData != null)
                {
                    model.isError = userData.AllowLogin;
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");
                model.isError = false;
            }
            return model;
        }
        class mdlogin
        {
            public bool DeviceAllowLogin { get; set; }
        }
        public async Task<JsonResponseModel> CheckUserAlreadyLoginOtherDevice(long Userid)
        {
            //bool isError = true;
            JsonResponseModel model = new JsonResponseModel();
            model.isError = true;
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pUserId", Userid);

                var userData = dapperConnection.GetListResult<mdlogin>("CheckUserDetailsByDevice", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (userData != null)
                {
                    model.isError = userData.DeviceAllowLogin;
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");
                model.isError = false;
            }
            return model;
        }
        public bool InertLogUserDetails(long Userid, string pLogType, string IpAddress, string userLogDetails, bool worngattempt)
        {
            bool isError = true;

            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pUserId", Userid);
                dictionary.Add("pLogType", pLogType);
                dictionary.Add("pIPAddress", IpAddress);
                dictionary.Add("pDetails", userLogDetails);
                dictionary.Add("pWorngattempt", worngattempt);

                dapperConnection.ExecuteWithoutResult("InsertLogUserDetails", CommandType.StoredProcedure, dictionary);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");
                isError = false;
            }
            return isError;
        }

        class mdlMail
        {
            public bool AllowLogin { get; set; }
        }
        public bool CheckForgotPasswordDetailsAlreadySend(string EmailId, string strIPAddress)
        {
            bool isError = true;

            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pEmailId", EmailId);
                dictionary.Add("pIPAddress", strIPAddress);

                var userData = dapperConnection.GetListResult<mdlMail>("Checkforgotpassworddetails", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (userData != null)
                {
                    isError = userData.AllowLogin;
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");
                isError = false;
            }
            return isError;
        }

        public bool InertLogForgotPasswordLogDetails(string EmailId, string pLogType, string IpAddress, string userLogDetails)
        {
            bool isError = true;

            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pEmailId", EmailId);
                dictionary.Add("pLogType", pLogType);
                dictionary.Add("pIPAddress", IpAddress);
                dictionary.Add("pDetails", userLogDetails);
                dapperConnection.ExecuteWithoutResult("Insertforgotpasswordlogdetails", CommandType.StoredProcedure, dictionary);

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");
                isError = false;
            }
            return isError;
        }
        public JsonResponseModel ExecuteQueryData(string QueryData)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                var rslt = dapperConnection.GetDataTable(QueryData, CommandType.Text, out string strError);
                jsonResponseModel.result = Functions.ConvertDataTableToHTML(rslt);
                jsonResponseModel.strMessage = "Record executed success";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into Removereportmaster", ex.ToString(), "reportmasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }

            return jsonResponseModel;
        }

        public int GetIdbyUserName(string strUserName, out int idMessage)
        {
            bool isError = true;
            idMessage = 0;

            try
            {
                strUserName = Functions.FrontDecrypt(strUserName);

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Puserusername", strUserName);

                var userData = dapperConnection.GetListResult<SessionUserModel>("GetUserIdByUserName", CommandType.StoredProcedure, dictionary).ToList();
                if (userData != null)
                {
                    if (userData.Count() > 0)
                    {
                        idMessage = (int)userData[0].Id;
                        //db.InsertUserLogInLogDetails(userData.FirstOrDefault().Id, userLogDetails);
                    }
                    else
                    {
                        idMessage = 0;
                        isError = false;
                    }
                }
                else
                {
                    idMessage = 0;
                    isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");

                isError = false;
            }


            return idMessage;
        }
        public int GetAttemptsCount(string username, string ipAddress ,out int attempts)
        {
            bool isError = true;
            attempts = 0;

            try
            {
                username = Functions.FrontDecrypt(username);
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pUserName", username);
                dictionary.Add("pipAddress", ipAddress);

                var userData = dapperConnection.GetListResult<AttmptsModel>("getWrongAttemptCount", CommandType.StoredProcedure, dictionary).FirstOrDefault();
                if (userData != null)
                {
                    attempts = userData.AttemptsLogin;
                }
                else
                {
                    attempts = 0;
                    isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLoginUsersMaster", ex.ToString(), "AccountService", "LogInValidation");

                isError = false;
            }


            return attempts;
        }
        //public bool ChangePassword(long userId, string strPassword, out string strErrorMessage)
        //{
        //    bool isError = true;
        //    strErrorMessage = "";
        //    using (UserMasterDataContext db = new UserMasterDataContext(SqlConnectionString))
        //    {
        //        try
        //        {
        //            if (string.IsNullOrWhiteSpace(strPassword))
        //            {
        //                strErrorMessage = "Please Enter Password.";
        //                isError = false;
        //                return isError;
        //            }

        //            db.ChangePasswordUser(userId, Functions.Encode(strPassword));
        //            strErrorMessage = "Password Changes Success.";
        //        }
        //        catch (Exception ex)
        //        {
        //            using (System.Web.Data.Comman.Static.NLogger log = new System.Web.Data.Comman.Static.NLogger())
        //            {
        //                log.Log(" strErrorMessage : " + ex.Message + " ### Trace : " + ex.StackTrace, LogType.Info);
        //            }
        //            strErrorMessage = ex.Message;
        //            isError = false;
        //        }
        //    }
        //    return isError;
        //}

        //public ValidateUserNameEmailMobileNoResult ValidateUsernameEmailMobile(string strUserName, string strEmail, string strMobile)
        //{
        //    using (UserMasterDataContext db = new UserMasterDataContext(SqlConnectionString))
        //    {
        //        ValidateUserNameEmailMobileNoResult data = db.ValidateUserNameEmailMobileNo(strUserName, strMobile, strEmail).FirstOrDefault();
        //        return data;
        //    }
        //}

        //public UserMasterModel Get(long id)
        //{
        //    //throw new NotImplementedException();
        //    return GetList().Where(x => x.Id == id).FirstOrDefault();
        //}

        //public List<UserMasterModel> GetList()
        //{
        //    using (UserMasterDataContext db = new UserMasterDataContext(SqlConnectionString))
        //    {
        //        return db.GetAllUserList().Select(x => new UserMasterModel
        //        {
        //            Id = x.Id,
        //            FirstName = x.FirstName,
        //            MiddleName = x.MiddleName,
        //            LastName = x.LastName,
        //            MobileNo = x.MobileNo,
        //            Email = x.Email,
        //            RoleId = x.RoleId,
        //            RoleName = ((Role)x.RoleId).ToString(),
        //            //StateId = x.StateId,
        //            //CityVillageId = x.CityVillageId,
        //            //StateName = x.StateName,
        //            //CityVillageName = x.CityVillageName,
        //            //DistictName = x.DistictName,
        //            //DistrictId = x.DistrictId,
        //            Title = x.Title,
        //            UserName = x.UserName,
        //            UserPassword = x.UserPassword,
        //        }).ToList();
        //    }
        //}

        //public List<ListItem> GetListForDropDown()
        //{

        //    using (UserMasterDataContext db = new UserMasterDataContext(SqlConnectionString))
        //    {
        //        return db.GetAllUserList().Select(x => new ListItem
        //        {
        //            Value = x.Id.ToString(),
        //            Text = x.FirstName + " " + x.MiddleName + " " + x.LastName + " (" + ((Role)x.RoleId).ToString() + ")",
        //            //Text = x.FirstName+ " "+  x.MiddleName + " " + x.LastName,
        //        }).ToList();
        //    }
        //}

        //public bool Delete(long id, out string strMessage)
        //{
        //    bool isError = true;
        //    strMessage = "";
        //    try
        //    {
        //        using (UserMasterDataContext db = new UserMasterDataContext(SqlConnectionString))
        //        {
        //            var data = db.RemoveUserMaster(id, CommonHelper.UserDetails.UserName);


        //            strMessage = "Record Removed success";
        //            isError = false;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        strMessage = ex.Message;
        //        isError = true;
        //    }
        //    return isError;
        //}

        //public bool AddOrUpdate(UserMasterModel model, out string strMessage)
        //{
        //    bool isError = true;
        //    strMessage = "";
        //    try
        //    {
        //        using (UserMasterDataContext db = new UserMasterDataContext(SqlConnectionString))
        //        {
        //            var data = db.InsertOrUpdateUserMaster(model.Id, model.UserName, model.UserPassword, model.Title, model.FirstName, model.MiddleName, model.LastName, model.Email,
        //                model.MobileNo, model.RoleId, CommonHelper.UserDetails.UserName);

        //            if (model.Id == 0)
        //            {
        //                strMessage = "Record inserted success";
        //                isError = false;
        //            }
        //            else
        //            {
        //                strMessage = "Record updated success";
        //                isError = false;
        //            }
        //            model.Id = (long)data.FirstOrDefault().RecId;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        using (System.Web.Data.Comman.Static.NLogger log = new System.Web.Data.Comman.Static.NLogger())
        //        {
        //            log.Log(" strErrorMessage : " + ex.Message + " ### Trace : " + ex.StackTrace, LogType.Info);
        //        }
        //        strMessage = ex.Message;
        //        isError = true;
        //    }
        //    return isError;
        //}

        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~AccountService()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The dispose method that implements IDisposable.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// The virtual dispose method that allows
        /// classes inherithed from this one to dispose their resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources here.
                }

                // Dispose unmanaged resources here.
            }

            disposed = true;
        }

        #endregion

    }
}

