using System;
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
using System.Transactions;

namespace NWRWS.Services.Service
{
    public class DepartmentServices : IDepartmentServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public DepartmentServices()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public DepartmentMasterModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldepartmentmasterfront", ex.ToString(), "DepartmentServices", "Get");
                return null;
            }
        }

        public List<DepartmentMasterModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLangId", lgLangId);
                var data = dapperConnection.GetListResult<DepartmentMasterModel>("getalldepartmentmasterfront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.DepId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldepartmentmasterfront", ex.ToString(), "DepartmentServices", "GetList");
                return null;
            }
        }

        public List<DepartmentMasterModel> GetListF()
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
               
                var data = dapperConnection.GetListResult<DepartmentMasterModel>("getalldepartmentmasterfront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.DepId;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getalldepartmentmasterfront", ex.ToString(), "DepartmentServices", "GetList");
                return null;
            }
        }

     

        public JsonResponseModel AddOrUpdate(DepartmentMasterModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
               
                if (model.Id != 0)
                {
                    var dataModel = Get(model.Id);
                   
                }


                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pid", model.Id);
                dictionary.Add("pdepartmentname", model.DepartmentName);
                dictionary.Add("pLanguageId", model.LanguageId);
                dictionary.Add("pisactive", model.IsActive);
                dictionary.Add("pusername", username);

                var data = dapperConnection.GetListResult<long>("insertorupdateDepartmentmaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (model.Id == 0)
                {
                    jsonResponseModel.strMessage = "Record inserted success";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
                }
                else
                {
                    jsonResponseModel.strMessage = "Record updated success";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
                }
                model.Id = (long)data;


            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into insertorupdateDepartmentmaster", ex.ToString(), "DepartmentServices", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pid", id);
                dictionary.Add("pusername", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("removeDepartmentmaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record Removed success";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemovePublicationTypeMaster", ex.ToString(), "DepartmentServices", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }


      
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~DepartmentServices()
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
