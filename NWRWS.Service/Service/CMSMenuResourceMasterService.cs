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
    public class CMSMenuResourceMasterService : ICMSMenuResourceMasterService
    {
        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        #region Constructor

        public CMSMenuResourceMasterService()
        {
            dapperConnection = new DapperConnection();
        }

        #endregion

        #region Public Method(s)

        public CMSMenuResourceModel Get(long id, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllMenuResourceMaster", ex.ToString(), "CMSMenuResourceMasterService", "Get");
                return null;
            }
        }
        public CMSMenuResourceModel GetMenuRes(long menuResid, long lgLangId = 1)
        {
            try
            {
                //throw new NotImplementedException();
                return GetList(lgLangId).Where(x => x.CMSMenuResId == menuResid).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "CMSMenuResourceMasterService", "Get");
                return null;
            }
        }

        public List<CMSMenuResourceModel> GetList(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<CMSMenuResourceModel>("GetAllCMSMenuResourceMasterLanguageId", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuResourceMasterLanguageId", ex.ToString(), "CMSMenuResourceMasterService", "GetList");
                return null;
            }
        }
        public List<CMSMenuResourceModel> GetListFront(long lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<CMSMenuResourceModel>("GetAllCMSMenuResourceMasterLanguageIdFront", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuResourceMasterLanguageId", ex.ToString(), "CMSMenuResourceMasterService", "GetList");
                return null;
            }
        }
        public List<CMSMenuResourceModel> GetListMaster()
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                //dictionary.Add("LangId", lgLangId);
                var data = dapperConnection.GetListResult<CMSMenuResourceModel>("GetAllCMSMenuResourceMasterRank", CommandType.StoredProcedure, dictionary).ToList();
                /*data.ForEach(x => {
                    x.Id = (long)x.Id;
                });*/
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllCMSMenuResourceMasterRank", ex.ToString(), "CMSMenuResourceMasterService", "GetList");
                return null;
            }
        }
        public List<CMSMenuResourceModel> GetParentList(long? lgLangId = 1)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("p_Languageid", lgLangId);
                var data = dapperConnection.GetListResult<CMSMenuResourceModel>("tbl_Menu_SelectAll", CommandType.StoredProcedure, dictionary).ToList();
                data.ForEach(x => {
                    x.Id = (long)x.Id;
                });
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into GetAllAdminMenuMaster", ex.ToString(), "CMSMenuResourceMasterService", "GetList");
                return null;
            }
        }

        public List<LanguageMasterModel> GetListLanguage()
        {
            try
            {
                return dapperConnection.GetListResult<LanguageMasterModel>("GetListLanguage", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into LanguageMasterModel", ex.ToString(), "CMSMenuResourceMasterService", "GetListLanguage");
                return null;
            }
        }
       
        
        //public List<ExamTypeMasterModel> GetExamType()
        //{
        //    try
        //    {
        //        return dapperConnection.GetListResult<ExamTypeMasterModel>("GetExamType", CommandType.StoredProcedure).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogger.Error("Error Into LanguageMasterModel", ex.ToString(), "CMSMenuResourceMasterService", "GetExamType");
        //        return null;
        //    }
        //}

        public JsonResponseModel Delete(long id, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("pId", id);
                dictionary.Add("Username", username);
                dapperConnection.GetListResult<CMSMenuResourceModel>("RemoveCMSMenuResourceMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                jsonResponseModel.strMessage = "Record removed successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into RemoveAdminMenuMaster", ex.ToString(), "CMSMenuResourceMasterService", "Delete");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
        public JsonResponseModel SwapSequance(long rank, string dir, string username, string type, long parentid)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();

            using (var transactionScope = new TransactionScope())
            {
                try
                {
                    var getDetails = GetListMaster();
                    CMSMenuResourceModel masterModel = getDetails.Where(x => x.MenuRank == rank).FirstOrDefault();
                    var cuurentLevelList = getDetails.OrderBy(x => x.MenuRank);
                    if (type == "ParentMenu")
                    {
                        cuurentLevelList = getDetails.Where(x => x.col_parent_id == 0).Where(x => x.col_menu_type == "0").Where(x => x.DeletedDate is null).OrderBy(x => x.MenuRank);
                    }
                    else
                    {
                        cuurentLevelList = getDetails.Where(x => x.col_parent_id == parentid).Where(x => x.DeletedDate is null).OrderBy(x => x.MenuRank);
                    }
                    long minValue = cuurentLevelList.Min(x => x.MenuRank);
                    long maxValue = cuurentLevelList.Max(x => x.MenuRank);

                    long updateRank = 0;

                    if (dir == "up" && (rank - 1) < minValue)
                    {
                        if (type == "ParentMenu")
                        {
                            jsonResponseModel.strMessage = "This Parent Menu already have min rank!";
                        }
                        else if (type == "ChildMenu")
                        {
                            jsonResponseModel.strMessage = "This Child Menu already have min rank!";
                        }
                        else
                        {
                            jsonResponseModel.strMessage = "This Menu already have min rank!";
                        }
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else if (dir == "down" && (rank + 1) > maxValue)
                    {
                        if (type == "ParentMenu")
                        {
                            jsonResponseModel.strMessage = "This Parent Menu already have max rank!";
                        }
                        else if (type == "ChildMenu")
                        {
                            jsonResponseModel.strMessage = "This Child Menu already have max rank!";
                        }
                        else
                        {
                            jsonResponseModel.strMessage = "This Menu already have max rank!";
                        }
                        jsonResponseModel.isError = true;
                        jsonResponseModel.type = PopupMessageType.error.ToString();
                    }
                    else
                    {
                        var indexList = cuurentLevelList.Select((x, i) => new
                        {
                            item = x,
                            index = i
                        }).ToList();

                        foreach (var cuurentLevel in indexList)
                        {
                            if (dir == "up" && cuurentLevel.item.MenuRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index - 1)).FirstOrDefault().item.MenuRank;
                                break;
                            }
                            else if (dir == "down" && cuurentLevel.item.MenuRank == rank)
                            {
                                updateRank = indexList.Where(x => x.index == (cuurentLevel.index + 1)).FirstOrDefault().item.MenuRank;
                                break;
                            }
                        }

                        CMSMenuResourceModel masterupdateRankModel = getDetails.Where(x => x.MenuRank == updateRank).FirstOrDefault();

                        if (masterModel != null && masterupdateRankModel != null)
                        {
                            masterModel.MenuRank = updateRank;
                            masterupdateRankModel.MenuRank = rank;
                            jsonResponseModel = UpdateSwap(masterModel, masterModel.CreatedBy);
                            jsonResponseModel = UpdateSwap(masterupdateRankModel, masterModel.CreatedBy);

                            jsonResponseModel.strMessage = "Data Swap Successfully";

                            transactionScope.Complete();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorLogger.Error("Error Into SwapSequance", ex.ToString(), "CMSMenuResourceMasterService", "SwapSequance");
                    jsonResponseModel.strMessage = ex.Message;
                    jsonResponseModel.isError = true;
                    jsonResponseModel.type = PopupMessageType.error.ToString();
                }
            }
            return jsonResponseModel;
        }
        public JsonResponseModel UpdateSwap(CMSMenuResourceModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("Id", model.Id);
                dictionary.Add("MenuRank", model.MenuRank);
                dictionary.Add("username", username);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateCMSMenuResourceMasterSwap", CommandType.StoredProcedure, dictionary).FirstOrDefault();


                jsonResponseModel.strMessage = "Record updated successfully";
                jsonResponseModel.isError = false;
                jsonResponseModel.type = PopupMessageType.success.ToString();


                model.Id = (long)data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into InsertOrUpdateCMSMenuResourceMasterSwap", ex.ToString(), "CMSMenuResourceMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }
        public JsonResponseModel AddOrUpdate(CMSMenuResourceModel model, string username)
        {
            JsonResponseModel jsonResponseModel = new JsonResponseModel();
            try
            {

                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("P_Id", model.Id);
                dictionary.Add("pLanguageId", model.LanguageId);
                dictionary.Add("pMenuName", model.MenuName);
                dictionary.Add("pMenuURL", model.MenuURL);
                dictionary.Add("pTooltip", model.Tooltip);
                dictionary.Add("pResourceType", model.ResourceType);
                dictionary.Add("pPageDescription", model.PageDescription);
                dictionary.Add("pTemplateId", model.TemplateId);
                dictionary.Add("pIsActive", model.IsActive);
                dictionary.Add("pIsRedirect", model.IsRedirect);
                dictionary.Add("pIsFullScreen", model.IsFullScreen);
                dictionary.Add("pcol_menu_type", model.col_menu_type);
                dictionary.Add("pcol_parent_id", model.col_parent_id);
                dictionary.Add("MenuRank", model.MenuRank);
                dictionary.Add("pMetaTitle", model.MetaTitle);
                dictionary.Add("pMetaDescription", model.MetaDescription);
                dictionary.Add("pUsername", username);

                var data = dapperConnection.GetListResult<long>("InsertOrUpdateCMSMenuResourceMaster", CommandType.StoredProcedure, dictionary).FirstOrDefault();

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
                ErrorLogger.Error("Error Into InsertOrUpdateCMSMenuResourceMaster", ex.ToString(), "CMSMenuResourceMasterService", "AddOrUpdate");
                jsonResponseModel.strMessage = ex.Message;
                jsonResponseModel.isError = true;
                jsonResponseModel.type = PopupMessageType.error.ToString();
            }
            return jsonResponseModel;
        }

        public CMSMenuResourceModel PageDeails(long Id)
        {
            try
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("p_pageid", Id);
                var data = dapperConnection.GetListResult<CMSMenuResourceModel>("GetPageDataFromPageName", CommandType.StoredProcedure, dictionary).FirstOrDefault();

                return data;

            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into SetHomePageInCMSMenu", ex.ToString(), "CMSMenuMasterService", "SetHomePageInCMSMenu");
                return null;
            }
        }

        public CMSMenuResourceModel UpdateSiteDate()
        {
            try
            {
                var data = dapperConnection.GetListResult<CMSMenuResourceModel>("PROC_Visitors_Select", CommandType.StoredProcedure).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into CMSMenuResourceModel", ex.ToString(), "CMSMenuResourceMasterService", "GetListLanguage");
                return null;
            }
        }
        #endregion

        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~CMSMenuResourceMasterService()
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

