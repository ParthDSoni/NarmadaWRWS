using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using NWRWS.Model.Service;
using NWRWS.IService.Service;
using NWRWS.Common;
using NWRWS.Model.System;
using System.Transactions;
using System.Data;

namespace NWRWS.Common
{
    public class GetWebAPIData : IWebAPIDataService
    {

        #region Constants
        public static DapperConnection dapperConnection;
        #endregion

        public GetWebAPIData()
        {
            dapperConnection = new DapperConnection();
        }

        #region Assembly Details

        #region Bind Assembly District Data
        public List<AssemblyDistrictDetails> GetAssemblyDistrict()
        {
            List<AssemblyDistrictDetails> AssemblyDistrictList = new List<AssemblyDistrictDetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/getdistrict/CEODISTDATA,CEO2017,DEO").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    AssemblyDistrictList = JsonConvert.DeserializeObject<List<AssemblyDistrictDetails>>(DistList);
                }
            }
            return AssemblyDistrictList;
        }
        #endregion

        #region Bind Assembly Constituency Data
        public List<AssemblyDetails> GetAssemblyDetails()
        {
            List<AssemblyDetails> AssemblyDetailsList = new List<AssemblyDetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/acdetails/CEOACDATA,CEO2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    AssemblyDetailsList = JsonConvert.DeserializeObject<List<AssemblyDetails>>(DistList);
                }
            }
            return AssemblyDetailsList;
        }
        #endregion

        #region "Get District Election Officer Details"
        public List<DEODetails> GetDEODetail()
        {
            List<DEODetails> DEODetailsList = new List<DEODetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/getdistrict/CEODISTDATA,CEO2017,DEO").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    DEODetailsList = JsonConvert.DeserializeObject<List<DEODetails>>(DistList);
                }
            }
            return DEODetailsList;
        }
        #endregion

        #region "Get Deputy District Election Officer Details"
        public List<DYDEODetails> GetDYDEODetail()
        {
            List<DYDEODetails> DYDEODetailsList = new List<DYDEODetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/getdistrict/CEODISTDATA,CEO2017,DYDEO").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    DYDEODetailsList = JsonConvert.DeserializeObject<List<DYDEODetails>>(DistList);
                }
            }
            return DYDEODetailsList;
        }
        #endregion

        #region "Get Electoral Registration Officer Details"
        public List<ERODetails> GetERODetail()
        {
            List<ERODetails> ERODetailsList = new List<ERODetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/ERO_AERODetails/CEOEROAERODATA,CEO2017,ERO").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ERODetailsList = JsonConvert.DeserializeObject<List<ERODetails>>(DistList);
                }
            }
            return ERODetailsList;
        }
        #endregion

        #region "Get Assistant Electoral Registration Officer Details"
        public List<ERODetails> GetAERODetail()
        {
            List<ERODetails> AERODetailsList = new List<ERODetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/ERO_AERODetails/CEOEROAERODATA,CEO2017,AERO").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    AERODetailsList = JsonConvert.DeserializeObject<List<ERODetails>>(DistList);
                }
            }
            return AERODetailsList;
        }
        #endregion

        #endregion

        #region Parliamentary Details
        #region Bind Parliamentary District Data
        public List<ParliamentaryDistrictDetails> GetParliamentaryDistrict()
        {
            List<ParliamentaryDistrictDetails> ParliamentaryDistrictList = new List<ParliamentaryDistrictDetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/PCOfficer/CEOPCOFFICERDATA,CEO2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ParliamentaryDistrictList = JsonConvert.DeserializeObject<List<ParliamentaryDistrictDetails>>(DistList);
                }
            }
            return ParliamentaryDistrictList;
        }
        #endregion

        #region Bind Parliamentary Constituency Data
        public List<ParliamentaryDetails> GetParliamentaryDetails()
        {
            List<ParliamentaryDetails> ParliamentaryDetailsList = new List<ParliamentaryDetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/pcdetail/CEOACDATA,CEO2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ParliamentaryDetailsList = JsonConvert.DeserializeObject<List<ParliamentaryDetails>>(DistList);
                }
            }
            return ParliamentaryDetailsList;
        }
        #endregion

        #region "Get District Returning Officer Details"
        public List<RODetails> GetRODetail()
        {
            List<RODetails> RODetailsList = new List<RODetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/CEOweb/api/PCOfficer/CEOPCOFFICERDATA,CEO2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    RODetailsList = JsonConvert.DeserializeObject<List<RODetails>>(DistList);
                }
            }
            return RODetailsList;
        }
        #endregion
        #endregion

        #region Matdar Sahayata Kendra Details

        #region Bind MSK District Data
        public List<MSKDistrictDetails> GetMSKDistrict()
        {
            List<MSKDistrictDetails> MSKDistrictList = new List<MSKDistrictDetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/MatSahaytaKendra/CEOMSKDATA,CEO2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList1 = Res.Content.ReadAsStringAsync().Result;
                    MSKDistrictList = JsonConvert.DeserializeObject<List<MSKDistrictDetails>>(DistList1);
                }
            }
            return MSKDistrictList;
        }

        #endregion

        #region Bind MSK Data
        public List<MSKDetails> GetMSKDetails()
        {
            List<MSKDetails> MSKDetailsList = new List<MSKDetails>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/MatSahaytaKendra/CEOMSKDATA,CEO2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    MSKDetailsList = JsonConvert.DeserializeObject<List<MSKDetails>>(DistList);
                }
            }
            return MSKDetailsList;
        }
        #endregion

        public List<DirectoryDesignation> GetDirectoryDesignation()
        {
            try
            {
                return dapperConnection.GetListResult<DirectoryDesignation>("GetListDirectoryDesignation", CommandType.StoredProcedure).ToList();
            }
            catch (Exception ex)
            {
                ErrorLogger.Error("Error Into directorydesignation", ex.ToString(), "GetWebAPIData", "GetListDirectoryDesignation");
                return null;
            }
        }

        #endregion

        #region LoksabhaGeneralElection-2014 Details

        #region Bind LoksabhaGeneralElection-2014 Data
        public List<LocksabhaCandaffidavit> GetLoksabhaGeneralElection2014Data()
        {
            List<LocksabhaCandaffidavit> LoksabhaGeneralElection2014List = new List<LocksabhaCandaffidavit>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/pccandidateaffi/Loksabha%20General%20Election%20-%202014").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    LoksabhaGeneralElection2014List = JsonConvert.DeserializeObject<List<LocksabhaCandaffidavit>>(DistList);
                }
            }
            return LoksabhaGeneralElection2014List;
        }
        #endregion

        #endregion

        #region Get GENERAL ASSEMBLY ELECTION-2012 Data
        public List<CandidatesAffidavit2> GetGENERALASSEMBLYELECTION2012Data()
        {
            List<CandidatesAffidavit2> GENERALASSEMBLYELECTION2012List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/General%20Assembly%20Election-2012").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    GENERALASSEMBLYELECTION2012List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return GENERALASSEMBLYELECTION2012List;
        }
        #endregion

        #region Get GENERAL ASSEMBLY ELECTION-2017 Data
        public List<CandidatesAffidavit2> GetGENERALASSEMBLYELECTION2017Data()
        {
            List<CandidatesAffidavit2> GENERALASSEMBLYELECTION2017List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/General%20Assembly%20Election-2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    GENERALASSEMBLYELECTION2017List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return GENERALASSEMBLYELECTION2017List;
        }
        #endregion

        #region Get BYE ELECTION - 2013 Data
        public List<CandidatesAffidavit2> GetBindBYEELECTION2013Data()
        {
            List<CandidatesAffidavit2> BindBYEELECTION2013List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/Bye%20Election%20-%202013").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    BindBYEELECTION2013List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return BindBYEELECTION2013List;
        }
        #endregion

        #region Get BYE ELECTION - November-December - 2013 Data
        public List<CandidatesAffidavit2> GetBYEELECTIONNovDec2013()
        {
            List<CandidatesAffidavit2> BYEELECTIONNovDec2013List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/Bye%20Election%20-%20Dec%20-2013").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    BYEELECTIONNovDec2013List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return BYEELECTIONNovDec2013List;
        }
        #endregion

        #region Get Bye Election - April - 2014 Data
        public List<CandidatesAffidavit2> GetByeElectionApril2014()
        {
            List<CandidatesAffidavit2> ByeElectionApril2014List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/Bye%20Election%20-%20April%20-2014").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionApril2014List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return ByeElectionApril2014List;
        }
        #endregion

        #region Get Bye Election September 2014 Data
        public List<CandidatesAffidavit2> GetByeElectionSeptember2014()
        {
            List<CandidatesAffidavit2> ByeElectionSeptember2014List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/Bye%20Election%20-%20September%20-2014").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionSeptember2014List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return ByeElectionSeptember2014List;
        }
        #endregion

        #region Get Bye Election - January 2016 Data
        public List<CandidatesAffidavit2> GetByeElectionJanuary2016()
        {
            List<CandidatesAffidavit2> ByeElectionJanuary2016List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/Bye%20Election%20-%20January%20-2016").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionJanuary2016List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return ByeElectionJanuary2016List;
        }
        #endregion

        #region Get Bye Election - December 2018 Data
        public List<CandidatesAffidavit2> GetByeElectionDecember2018()
        {
            List<CandidatesAffidavit2> ByeElectionDecember2018List = new List<CandidatesAffidavit2>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavit/Bye%20Election%20-%20Dec%20-2018").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionDecember2018List = JsonConvert.DeserializeObject<List<CandidatesAffidavit2>>(DistList);
                }
            }
            return ByeElectionDecember2018List;
        }
        #endregion

        #region step 1 Details

        #region Bind LoksabhaGeneralElection-2014 step1 Data
        public List<CandidatesAffidavit3> GetLoksabhaGeneralElection2014Datastep1(int distid)
        {
            List<CandidatesAffidavit3> LoksabhaGeneralElection2014List1 = new List<CandidatesAffidavit3>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/pccandidateaffi1/" + distid + ",Loksabha%20General%20Election%20-%202014").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    LoksabhaGeneralElection2014List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit3>>(DistList);
                }
            }
            return LoksabhaGeneralElection2014List1;
        }
        #endregion

        #region Bind GENERAL ASSEMBLY ELECTION-2012 step1 Data
        public List<CandidatesAffidavit4> GetGENERALASSEMBLYELECTION2012step1(int Acno)
        {
            List<CandidatesAffidavit4> GENERALASSEMBLYELECTION2012List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",General%20Assembly%20Election-2012").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    GENERALASSEMBLYELECTION2012List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return GENERALASSEMBLYELECTION2012List1;
        }
        #endregion

        #region Bind GENERAL ASSEMBLY ELECTION-2017 step1 Data
        public List<CandidatesAffidavit4> GetGENERALASSEMBLYELECTION2017step1(int Acno)
        {
            List<CandidatesAffidavit4> GENERALASSEMBLYELECTION2017List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",General%20Assembly%20Election-2017").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    GENERALASSEMBLYELECTION2017List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return GENERALASSEMBLYELECTION2017List1;
        }
        #endregion

        #region Bind BYE ELECTION - May-June - 2013 step1 Data
        public List<CandidatesAffidavit4> GetBYEELECTIONMayJune2013step1(int Acno)
        {
            List<CandidatesAffidavit4> BYEELECTIONMayJune2013List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",Bye%20Election%20-%202013").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    BYEELECTIONMayJune2013List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return BYEELECTIONMayJune2013List1;
        }
        #endregion

        #region Bind BYE ELECTION - November-December - 2013 step1 Data
        public List<CandidatesAffidavit4> GetBYEELECTIONNovemberDecember2013step1(int Acno)
        {
            List<CandidatesAffidavit4> BYEELECTIONNovemberDecember2013List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",Bye%20Election%20-%20Dec%20-2013").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    BYEELECTIONNovemberDecember2013List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return BYEELECTIONNovemberDecember2013List1;
        }
        #endregion

        #region Bind Bye Election - April - 2014 step1 Data
        public List<CandidatesAffidavit4> GetByeElectionApril2014step1(int Acno)
        {
            List<CandidatesAffidavit4> ByeElectionApril2014List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",Bye%20Election%20-%20April%20-2014").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionApril2014List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return ByeElectionApril2014List1;
        }
        #endregion

        #region Bind Bye Election September 2014 step1 Data
        public List<CandidatesAffidavit4> GetByeElectionSeptember2014step1(int Acno)
        {
            List<CandidatesAffidavit4> ByeElectionSeptember2014List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",Bye%20Election%20-%20September%20-2014").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionSeptember2014List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return ByeElectionSeptember2014List1;
        }
        #endregion

        #region Bind Bye Election - January 2016 step1 Data
        public List<CandidatesAffidavit4> GetByeElectionJanuary2016step1(int Acno)
        {
            List<CandidatesAffidavit4> ByeElectionJanuary2016List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",Bye%20Election%20-%20January%20-2016").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionJanuary2016List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return ByeElectionJanuary2016List1;
        }
        #endregion

        #region Bind Bye Election - December 2018 step1 Data
        public List<CandidatesAffidavit4> GetByeElectionDecember2018step1(int Acno)
        {
            List<CandidatesAffidavit4> ByeElectionDecember2018List1 = new List<CandidatesAffidavit4>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync("https://erms.gujarat.gov.in/ceoweb/api/candidatesaffidavite1/" + Acno + ",Bye%20Election%20-%20Dec%20-2018").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var DistList = Res.Content.ReadAsStringAsync().Result;
                    ByeElectionDecember2018List1 = JsonConvert.DeserializeObject<List<CandidatesAffidavit4>>(DistList);
                }
            }
            return ByeElectionDecember2018List1;
        }
        #endregion

        #endregion


        #region Disposing Method(s)

        private bool disposed;

        /// <summary>
        /// Destructor
        /// </summary>
        ~GetWebAPIData()
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
