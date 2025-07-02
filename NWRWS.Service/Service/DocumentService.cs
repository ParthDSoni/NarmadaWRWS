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
    public class DocumentService: IDocumentServices
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public DocumentService()
        {
            dapperConnection = new DapperConnection();
        }


        #endregion

        public JsonResponseModel Delete(long Doc_Id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pDoc_Id", Doc_Id);
                dictionary.Add("pUsername", username);
                dapperConnection.GetListResult<AdminMenuMasterModel>("RemoveDocumentMaster", CommandType.StoredProcedure, dictionary).ToList();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveGoiLogoMaster", ex.ToString(), "RemoveGoiLogoMaster", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
        public DocumentModel Get(long Id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Doc_Id == Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllGoiLogoMasterLanguageId", ex.ToString(), "MinisterMasterService", "Get");
                return null;
            }
        }
        public List<DocumentModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pLanguageId", lgLangId);
                var data = dapperConnection.GetListResult<DocumentModel>("GetAllDocumentMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Doc_Id = (long)x.Doc_Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllGoiLogoMasterLanguageId", ex.ToString(), "GoiLogoService", "GetList");
                return null;
            }
        }

        public JsonResponseModel AddOrUpdate(DocumentModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                if (model.Doc_Id != 0)
                {
                    var dataModel = Get(model.Doc_Id);
                }
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pDoc_Id", model.Doc_Id);               
                dictionary.Add("pDoc_Name", model.Doc_Name);
                dictionary.Add("pFile_Name", model.File_Name);
                dictionary.Add("pDoc_Path", model.Doc_Path);
                dictionary.Add("pLanguageId", model.LanguageId);  
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pUsername", username);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateDocumentMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (model.Doc_Id == 0)
                {
                    jsonResponseModel.strMessage = "Record inserted successfully";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
                }
                else
                {
                    jsonResponseModel.strMessage = "Record updated successfully";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
                }
                model.Doc_Id = (long)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateMinisterMaster", ex.ToString(), "MinisterMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~DocumentService()
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

