using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Services.Service
{
    public class GlobleSerchService: IGlobleSerchService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public GlobleSerchService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)
        public List<GlobleSerchModel> GetList(string SearchText, long lgLangId)
        {
            try
            {               
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LanguageId", lgLangId);
                dictionary.Add("SearchText", SearchText);
                var data = dapperConnection.GetListResult<GlobleSerchModel>("getallmetadata", CommandType.StoredProcedure, dictionary).ToList();
               
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllNewsMasterLanguageId", ex.ToString(), "GlobleSerchService", "GetList");
                return null;
            }
        }

        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~GlobleSerchService()
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
