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
    public class LanguageService : ILanguageService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor
        public LanguageService()
        {
            dapperConnection = new DapperConnection();
        }
        #endregion

        #region Methods
        public LanguageMasterModel Get(long id)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList().Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLanguage", ex.ToString(), "LanguageService", "Get");
                return null;
            }
        }
        public List<LanguageMasterModel> GetList()
        {
            try
            {
                var data = dapperConnection.GetListResult<LanguageMasterModel>("getallCmsmasterlanguageid", CommandType.StoredProcedure).ToList();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetLanguagedata", ex.ToString(), "getallCmsmasterlanguageid", "GetList");
                return null;
            }
        }
        public List<LanguageMasterModel> GetListById(long id)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pid", id);
                var data = dapperConnection.GetListResult<LanguageMasterModel>("getlanguagebyid", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x =>
                {
                    x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into getlanguagebyid", ex.ToString(), "LanguageService", "GetList");
                return null;
            }
        }
        public JsonResponseModel AddorUpdate(LanguageMasterModel model, string username)
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
                dictionary.Add("pname", model.Name);
                dictionary.Add("pUsername", username);
                dictionary.Add("pisvisible", model.IsVisible);

                var data = dapperConnection.GetListResult<long>("insertorupdateCmsLanguagemaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                if (model.Id == 0)
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
                model.Id = (long)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into insertorupdateCmsLanguagemaster", ex.ToString(), "LanguageService", "AddorUpdate");
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
              dapperConnection.GetListResult<JsonResponseModel>("removecmsLanguagemaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                    jsonResponseModel.strMessage = "Record removed successfully";
                    jsonResponseModel.isError = false;
                    jsonResponseModel.type = PopupMessageType.success.ToString();
               
            
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into removecmsLanguagemaster", ex.ToString(), "LanguageService", "Delete");
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
~LanguageService()
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
