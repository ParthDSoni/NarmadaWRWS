
using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.IService.System;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;

namespace NWRWS.Services.System
{
    public class SMTPMasterService : ISMTPMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public SMTPMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public SMTPModel Get()
        {
            try
            {
                var data = dapperConnection.GetListResult<SMTPModel>("GetSMTPSettings", CommandType.StoredProcedure).FirstOrDefault();
                //throw new NotImplementedException();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetSMTPSettings", ex.ToString(), "SMTPMasterService", "Get");
                return null;
            }
        }

        public bool InsertOrUpdate(SMTPModel objSMTPModel, out string strErrorMessage)
        {
            bool isError = true;
            strErrorMessage = "";

            try
            {

                #region Validation
                if (string.IsNullOrWhiteSpace(objSMTPModel.SMTPServer))
                {
                    strErrorMessage = "Please Enter SMTPServer.";
                    isError = false;
                    return isError;
                }

                //if (string.IsNullOrWhiteSpace(objSMTPModel.SMTPAccount))
                //{
                //    strErrorMessage = "Please Enter SMTPAccount.";
                //    isError = false;
                //    return isError;
                //}

                //if (string.IsNullOrWhiteSpace(objSMTPModel.SMTPPassword))
                //{
                //    strErrorMessage = "Please Enter SMTPPassword.";
                //    isError = false;
                //    return isError;
                //}

                if (string.IsNullOrWhiteSpace(objSMTPModel.SMTPFromEmail))
                {
                    strErrorMessage = "Please Enter SMTPFromEmail.";
                    isError = false;
                    return isError;
                }

                if (string.IsNullOrWhiteSpace(objSMTPModel.TestSMTPServer))
                {
                    strErrorMessage = "Please Enter TestSMTPServer.";
                    isError = false;
                    return isError;
                }

                if (string.IsNullOrWhiteSpace(objSMTPModel.TestSMTPAccount))
                {
                    strErrorMessage = "Please Enter TestSMTPAccount.";
                    isError = false;
                    return isError;
                }

                if (string.IsNullOrWhiteSpace(objSMTPModel.TestSMTPPassword))
                {
                    strErrorMessage = "Please Enter TestSMTPPassword.";
                    isError = false;
                    return isError;
                }

                if (string.IsNullOrWhiteSpace(objSMTPModel.TestSMTPFromEmail))
                {
                    strErrorMessage = "Please Enter TestSMTPFromEmail.";
                    isError = false;
                    return isError;
                }

                #endregion

                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("SMTPServer", objSMTPModel.SMTPServer);
                dictionary.Add("SMTPPort", objSMTPModel.SMTPPort);
                dictionary.Add("SMTPAccount", objSMTPModel.SMTPAccount);
                dictionary.Add("SMTPPassword", objSMTPModel.SMTPPassword);
                dictionary.Add("SMTPFromEmail", objSMTPModel.SMTPFromEmail);
                dictionary.Add("SMTPIsSecure", objSMTPModel.SMTPIsSecure);

                dictionary.Add("TestSMTPServer", objSMTPModel.TestSMTPServer);
                dictionary.Add("TestSMTPPort", objSMTPModel.TestSMTPPort);
                dictionary.Add("TestSMTPAccount", objSMTPModel.TestSMTPAccount);
                dictionary.Add("TestSMTPPassword", objSMTPModel.TestSMTPPassword);
                dictionary.Add("TestSMTPFromEmail", objSMTPModel.TestSMTPFromEmail);
                dictionary.Add("TestSMTPIsSecure", objSMTPModel.TestSMTPIsSecure);


                var userData = dapperConnection.ExecuteWithoutResult("InsertOrUpdateSMTPSetting", CommandType.StoredProcedure, dictionary);
                if (userData != false)
                {
                    strErrorMessage = "SMTP Setting Updated Successfully.";
                    isError = true;
                }
                else
                {
                    strErrorMessage = "SMTP Setting not Updated.";
                    isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateSMTPSetting", ex.ToString(), "SMTPMasterService", "InsertOrUpdate");
                strErrorMessage = ex.Message;
                isError = false;
            }


            return isError;
        }

        public bool UpdateSMTPEnvironment(string strSMTPIsTest, out string strErrorMessage)
        {
            bool isError = true;
            strErrorMessage = "";

            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("SMTPIsTest", strSMTPIsTest);

                var userData = dapperConnection.ExecuteWithoutResult("UpdateSMTPEnvironment", CommandType.StoredProcedure, dictionary);
                if (userData != false)
                {
                    strErrorMessage = "SMTP Environment Setting Updated Successfully.";
                    isError = false;
                }
                else
                {
                    strErrorMessage = "SMTP Environment Setting not Updated.";
                    isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into UpdateSMTPEnvironment", ex.ToString(), "SMTPMasterService", "UpdateSMTPEnv");
                strErrorMessage = ex.Message;
                isError = false;
            }


            return isError;

        }
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~SMTPMasterService()
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
