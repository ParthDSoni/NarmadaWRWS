using NWRWS.Common;
using NWRWS.IService.System;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Services.System
{
    public class CouchDBMasterService : ICouchDBMasterService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public CouchDBMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public CouchDBModel Get()
        {
            try
            {
                var data = dapperConnection.GetListResult<CouchDBModel>("GetCouchDBSettings", CommandType.StoredProcedure).FirstOrDefault();
                //throw new NotImplementedException();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetCouchDBSettings", ex.ToString(), "CouchDBMasterService", "Get");
                return null;
            }
        }

        public bool InsertOrUpdate(CouchDBModel objSMTPModel, out string strErrorMessage)
        {
            bool isError = true;
            strErrorMessage = "";

            try
            {

                #region Validation
                if (string.IsNullOrWhiteSpace(objSMTPModel.CouchDBURL))
                {
                    strErrorMessage = "Please Enter CouchDBURL.";
                    isError = false;
                    return isError;
                }

                if (string.IsNullOrWhiteSpace(objSMTPModel.CouchDBDbName))
                {
                    strErrorMessage = "Please Enter CouchDBDbName.";
                    isError = false;
                    return isError;
                }

                if (string.IsNullOrWhiteSpace(objSMTPModel.CouchDBUser))
                {
                    strErrorMessage = "Please Enter CouchDBUser.";
                    isError = false;
                    return isError;
                }
                #endregion

                Dictionary<string, object> dictionary = new Dictionary<string, object>();

                dictionary.Add("CouchDBURL", objSMTPModel.CouchDBURL);
                dictionary.Add("CouchDBDbName", objSMTPModel.CouchDBDbName);
                dictionary.Add("CouchDBUser", objSMTPModel.CouchDBUser);
                dictionary.Add("AllowCouchDBStore", objSMTPModel.AllowCouchDBStore);

                var userData = dapperConnection.ExecuteWithoutResult("InsertOrUpdateCouchDBSetting", CommandType.StoredProcedure, dictionary);
                if (userData != false)
                {
                    strErrorMessage = "CouchDB Setting Updated Successfully.";
                    isError = true;
                }
                else
                {
                    strErrorMessage = "CouchDB Setting not Updated.";
                    isError = false;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateCouchDBSetting", ex.ToString(), "CouchDBMasterService", "InsertOrUpdate");
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
        ~CouchDBMasterService()
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
