using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System.Data;
using System.Transactions;

namespace NWRWS.Services.Service
{
    public class DropDownService : IDropDownService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public DropDownService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public List<dynamic> GetAllCountries()
        {
            try
            {
                var data = dapperConnection.GetListResult<dynamic>("getallcountry", CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getallcountry", ex.ToString(), "DropDownService", "GetAllCountries");
                return null;
            }
        }

        public List<dynamic> GetAllStatesByCountry(long country)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pCountryId", country);
                var data = dapperConnection.GetListResult<dynamic>("GetAllStatesByCountry", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllStatesByCountry", ex.ToString(), "DropDownService", "GetAllStatesByCountry");
                return null;
            }
        }
        public List<dynamic> GetAllDistrictsByState(long state)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pStateId", state);
                var data = dapperConnection.GetListResult<dynamic>("getAllDistrictByStateid", CommandType.StoredProcedure, dictionary).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getAllDistrictByStateid", ex.ToString(), "DropDownService", "GetAllDistrictsByState");
                return null;
            }
        }

        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~DropDownService()
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
