using CommonCMS.Webs.Common;
using CommonCMS.Webs.IService;
using CommonCMS.Webs.Models;
using System.Data;

namespace CommonCMS.Webs.Service
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

                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    dictionary.Add("username", strUserName);
                    dictionary.Add("password", strPassword);

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
                            strErrorMessage = "User Details is not Found.";
                            isError = false;
                        }
                    }
                    else
                    {
                        strErrorMessage = "User Details is not Found.";
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

