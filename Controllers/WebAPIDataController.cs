using NWRWS.Common;
using NWRWS.IService.Service;
using NWRWS.Model.Service;
using NWRWS.Model.System;
using NWRWS.Services.Service;
using NWRWS.Webs.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Globalization;
using System.Web;

namespace NWRWS.Webs.Controllers
{
   
    public class WebAPIDataController : Controller
    {
        #region Controller Variable

        private IWebAPIDataService GetWebAPIData { get; set; }

        public long LanguageId
        {
            get
            {
                long Lang = 1;
                if (SessionWrapper.Get<long>(this.HttpContext.Session, "LanguageId") == null || SessionWrapper.Get<long>(this.HttpContext.Session, "LanguageId") == 0)
                {
                    SessionWrapper.Set<long>(this.HttpContext.Session, "LanguageId", 1);
                    Lang = 1;
                }
                else
                {
                    Lang = SessionWrapper.Get<long>(this.HttpContext.Session, "LanguageId");
                }
                return Lang;
            }
            set { SessionWrapper.Set<long>(this.HttpContext.Session, "LanguageId", value); }
        }


        #endregion

        #region Controller Constructor

        public WebAPIDataController(IWebAPIDataService _WebAPIDataService) 
        {
            GetWebAPIData = _WebAPIDataService;
        }

        #endregion

        #region Controller Method

        #region Bind Assembly data

        #region Bind AC District

        [Route("/BindAssemblyDistrict")]
        public JsonResult BindAssemblyDistrict()
        {

            List<ListItem> lsdata = new List<ListItem>();

            try
            {
                if (LanguageId == 2)
                {
                  lsdata.AddRange(GetWebAPIData.GetAssemblyDistrict().Select(x => new ListItem { Text = x.DIST_NO.ToString().PadLeft(2, '0') + " - " + x.GUJ_DIST_NAME, Value = x.DIST_NO.ToString() }).ToList());
                }
                else
                {
                    lsdata.AddRange(GetWebAPIData.GetAssemblyDistrict().Select(x => new ListItem { Text = x.DIST_NO.ToString().PadLeft(2, '0') + " - " + x.ENG_DIST_NAME, Value = x.DIST_NO.ToString() }).ToList());
                }

            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        #endregion

        #region Bind Assembly Constituency Data
        [Route("/BindAssemblyDetails")]
        public JsonResult BindAssemblyDetails(int Dist_id)
        {

            List<AssemblyDetails> lsdata = new List<AssemblyDetails>();
            //  List<AssemblyDistrictDetails> Assemblydistrictdata = GetWebAPIData.GetAssemblyDistrict().ToList();

            try
            {
                if (Dist_id == 0)
                {
                    lsdata.AddRange(GetWebAPIData.GetAssemblyDetails().ToList());
                }
                else
                {
                    lsdata.AddRange(GetWebAPIData.GetAssemblyDetails().Where(x => x.DIST_NO == Dist_id).ToList());
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region "Get District Election Officer Details"
        [Route("/BindDEODetail")]
        public JsonResult BindDEODetail(int Dist_id)
        {

            List<DEODetails> DEODetaillsdata = new List<DEODetails>();

            try
            {

                DEODetaillsdata.AddRange(GetWebAPIData.GetDEODetail().Where(x => x.DIST_NO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(DEODetaillsdata);
        }
        #endregion

        #region "Get Deputy District Election Officer Details"
        [Route("/BindDYDEODetail")]
        public JsonResult BindDYDEODetail(int Dist_id)
        {

            List<DYDEODetails> DYDEODetaillsdata = new List<DYDEODetails>();

            try
            {

                DYDEODetaillsdata.AddRange(GetWebAPIData.GetDYDEODetail().Where(x => x.DIST_NO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(DYDEODetaillsdata);
        }
        #endregion

        #region "Get Electoral Registration Officer Details"
        [Route("/BindERODetail")]
        public JsonResult BindERODetail(int Dist_id)
        {

            List<ERODetails> ERODetaillsdata = new List<ERODetails>();

            try
            {

                ERODetaillsdata.AddRange(GetWebAPIData.GetERODetail().Where(x => x.DIST_NO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(ERODetaillsdata);
        }
        #endregion

        #region "Get Assistant Electoral Registration Officer Details"
        [Route("/BindAERODetail")]
        public JsonResult BindAERODetail(int Dist_id)
        {

            List<ERODetails> AERODetaillsdata = new List<ERODetails>();

            try
            {

                AERODetaillsdata.AddRange(GetWebAPIData.GetAERODetail().Where(x => x.DIST_NO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(AERODetaillsdata);
        }
        #endregion

        #endregion

        #region Bind Parliamentary data

        #region Bind PC District

        [Route("/BindParliamentaryDistrict")]
        public JsonResult BindParliamentaryDistrict()
        {

            List<ListItem> lsdata = new List<ListItem>();

            try
            {
               
                if (LanguageId == 2)
                {
                    lsdata.AddRange(GetWebAPIData.GetParliamentaryDistrict().Select(x => new ListItem { Text = x.PC_NO.ToString().PadLeft(2, '0') + " - " + x.PC_NAME_V1, Value = x.PC_NO.ToString() }).ToList());
                }
                else
                {
                    lsdata.AddRange(GetWebAPIData.GetParliamentaryDistrict().Select(x => new ListItem { Text = x.PC_NO.ToString().PadLeft(2, '0') + " - " + x.ENG_PC_NAME, Value = x.PC_NO.ToString() }).ToList());
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        #endregion

        #region Bind Parliamentary Constituency Data
        [Route("/BindParliamentaryDetails")]
        public JsonResult BindParliamentaryDetails(int Dist_id)
        {

            List<ParliamentaryDetails> lsdata = new List<ParliamentaryDetails>();
            //  List<AssemblyDistrictDetails> Assemblydistrictdata = GetWebAPIData.GetAssemblyDistrict().ToList();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetParliamentaryDetails().Where(x => x.PC_NO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region "Get District Returning Officer Details"
        [Route("/BindRODetail")]
        public JsonResult BindRODetail(int Dist_id)
        {

            List<RODetails> RODetaillsdata = new List<RODetails>();

            try
            {

                RODetaillsdata.AddRange(GetWebAPIData.GetRODetail().Where(x => x.PC_NO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(RODetaillsdata);
        }
        #endregion
        #endregion

        #region Bind MSK data

        #region Bind MSK District

        [Route("/BindMSKDistrict")]
        public JsonResult BindMSKDistrict()
        {

            List<ListItem> lsdata = new List<ListItem>();

            try
            {
                if (LanguageId == 2)
                {
                    var data = GetWebAPIData.GetMSKDistrict().ToList().Select(x => new { Text = x.DISTRICTNO.ToString().PadLeft(2, '0') + " - " + x.GUJ_DISTRICTNAME, Value = x.DISTRICTNO.ToString() }).Distinct();
                    lsdata.AddRange(data.Select(x => new ListItem { Text = x.Text, Value = x.Value }).ToList());
                }
                else
                {
                    var data = GetWebAPIData.GetMSKDistrict().ToList().Select(x => new { Text = x.DISTRICTNO.ToString().PadLeft(2, '0') + " - " + x.DISTRICTNAME, Value = x.DISTRICTNO.ToString() }).Distinct();
                    lsdata.AddRange(data.Select(x => new ListItem { Text = x.Text, Value = x.Value }).ToList());
                }
                
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        #endregion

        #region Bind MSK Data
        [Route("/BindMSKDetails")]
        public JsonResult BindMSKDetails(int Dist_id)
        {

            List<MSKDetails> lsdata = new List<MSKDetails>();
            //  List<AssemblyDistrictDetails> Assemblydistrictdata = GetWebAPIData.GetAssemblyDistrict().ToList();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetMSKDetails().Where(x => x.DISTRICTNO == Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #endregion

        #region Bind Directory Data
        #region Bind Directory District

        [Route("/BindDistrict")]
        public JsonResult BindDistrict()
        {
            List<ListItem> lsdata = new List<ListItem>();
          
            try
            {
             
                lsdata.AddRange(GetWebAPIData.GetAssemblyDistrict().Select(x => new ListItem { Text = x.DIST_NO.ToString().PadLeft(2, '0') + " - " + x.ENG_DIST_NAME, Value = x.DIST_NO.ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        #endregion

        #region Bind Directory Designation

        [Route("/BindDesignation")]
        public JsonResult BindDesignation()
        {
            List<ListItem> lsdata = new List<ListItem>();

            try
            {
                lsdata.AddRange(GetWebAPIData.GetDirectoryDesignation().Select(x => new ListItem { Text = x.DesignationName, Value = x.DesignationID.ToString() }).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }

        #endregion

        #endregion

        #region Bind LoksabhaGeneralElection-2014 Data
        [Route("/BindLoksabhaGeneralElection2014")]
        public JsonResult BindLoksabhaGeneralElection2014()
        {

            List<LocksabhaCandaffidavit> lsdata = new List<LocksabhaCandaffidavit>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetLoksabhaGeneralElection2014Data().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind GENERAL ASSEMBLY ELECTION-2012 Data
        [Route("/BindGENERALASSEMBLYELECTION2012")]
        public JsonResult BindGENERALASSEMBLYELECTION2012()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetGENERALASSEMBLYELECTION2012Data().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind GENERAL ASSEMBLY ELECTION-2017 Data
        [Route("/BindGENERALASSEMBLYELECTION2017")]
        public JsonResult BindGENERALASSEMBLYELECTION2017()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetGENERALASSEMBLYELECTION2017Data().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind BYE ELECTION - 2013 Data
        [Route("/BindBYEELECTION2013")]
        public JsonResult BindBYEELECTION2013()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetBindBYEELECTION2013Data().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bye Election - November-December - 2013 Data
        [Route("/BindBYEELECTIONNovDec2013")]
        public JsonResult BindBYEELECTIONNovDec2013()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetBYEELECTIONNovDec2013().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bye Election - April - 2014 Data
        [Route("/BindByeElectionApril2014")]
        public JsonResult BindByeElectionApril2014()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionApril2014().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bye Election September - 2014 Data
        [Route("/BindByeElectionSeptember2014")]
        public JsonResult BindByeElectionSeptember2014()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionSeptember2014().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bye Election - January 2016 Data
        [Route("/BindByeElectionJanuary2016")]
        public JsonResult BindByeElectionJanuary2016()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionJanuary2016().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bye Election - December 2018 Data
        [Route("/BindByeElectionDecember2018")]
        public JsonResult BindByeElectionDecember2018()
        {
            List<CandidatesAffidavit2> lsdata = new List<CandidatesAffidavit2>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionDecember2018().ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind LoksabhaGeneralElection-2014 step-1 Data
        [Route("/BindLoksabhaGeneralElection2014step1")]
        public JsonResult BindLoksabhaGeneralElection2014step1(int Dist_id)
        {

            List<CandidatesAffidavit3> lsdata = new List<CandidatesAffidavit3>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetLoksabhaGeneralElection2014Datastep1(Dist_id).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind GENERAL ASSEMBLY ELECTION-2012 step1 Data
        [Route("/BindGENERALASSEMBLYELECTION2012step1")]
        public JsonResult BindGENERALASSEMBLYELECTION2012step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetGENERALASSEMBLYELECTION2012step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind GENERAL ASSEMBLY ELECTION-2017 step1 Data
        [Route("/BindGENERALASSEMBLYELECTION2017step1")]
        public JsonResult BindGENERALASSEMBLYELECTION2017step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetGENERALASSEMBLYELECTION2017step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind BYE ELECTION - May-June - 2013 step1 Data
        [Route("/BindBYEELECTIONMayJune2013step1")]
        public JsonResult BindBYEELECTIONMayJune2013step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetBYEELECTIONMayJune2013step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind BYE ELECTION - November-December - 2013 step1 Data
        [Route("/BindBYEELECTIONNovemberDecember2013step1")]
        public JsonResult BindBYEELECTIONNovemberDecember2013step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetBYEELECTIONNovemberDecember2013step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind Bye Election - April - 2014 step1 Data
        [Route("/BindByeElectionApril2014step1")]
        public JsonResult BindByeElectionApril2014step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionApril2014step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind Bye Election September 2014 step1 Data
        [Route("/BindByeElectionSeptember2014step1")]
        public JsonResult BindByeElectionSeptember2014step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionSeptember2014step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind Bye Election - January 2016 step1 Data
        [Route("/BindByeElectionJanuary2016step1")]
        public JsonResult BindByeElectionJanuary2016step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionJanuary2016step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #region Bind Bye Election - December 2018 step1 Data
        [Route("/BindByeElectionDecember2018step1")]
        public JsonResult BindByeElectionDecember2018step1(int AC_No)
        {

            List<CandidatesAffidavit4> lsdata = new List<CandidatesAffidavit4>();

            try
            {

                lsdata.AddRange(GetWebAPIData.GetByeElectionDecember2018step1(AC_No).ToList());
            }
            catch (Exception ex)
            {
                ErrorLogger.Error(ex.Message, ex.ToString(), ControllerContext.ActionDescriptor.ControllerName, ControllerContext.ActionDescriptor.ActionName, ControllerContext.HttpContext.Request.Method);
            }

            return Json(lsdata);
        }
        #endregion

        #endregion

    }
}
