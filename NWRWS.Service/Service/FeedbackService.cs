using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Services.Service
{
    public class FeedbackService :IFeedbackServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public FeedbackService()
        {
            dapperConnection = new DapperConnection();
        }


        #endregion

      
        public JsonResponseModel AddFeedback(Feedback model)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
               
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pFName", model.FName);
                dictionary.Add("pLName", model.LName);
                dictionary.Add("pEmail", model.Email);
                dictionary.Add("pPhoneNo", model.PhoneNo);
                dictionary.Add("pZip", model.Zip);
                dictionary.Add("pSubject", model.Subject);
                dictionary.Add("pCountry", model.Country);
                dictionary.Add("pState", model.State);
                dictionary.Add("pStateName", model.StateName);
                dictionary.Add("pCity", model.City);
                dictionary.Add("pCityName", model.CityName);
                dictionary.Add("pAddress", model.Address);
                dictionary.Add("pFeedbackDetails", model.FeedbackDetails);

                var data = dapperConnection.GetListResult<long>("InsertFeedbackDetails", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (model.Id == 0)
                {
                    jsonResponseModel.strMessage = "Thank you for your valuable feedback";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
                }                
                model.Id = (long)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertFeedbackDetails", ex.ToString(), "FeedbackService", "Add");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public List<Feedback> GetList()
        {
            try
            {
                var data = dapperConnection.GetListResult<Feedback>("GetFeedbackdata", CommandType.StoredProcedure).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetFeedbackdata", ex.ToString(), "FeedbackService", "GetList");
                return null;
            }
        }

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~FeedbackService()
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

