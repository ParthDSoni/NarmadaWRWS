using CommonCMS.Webs.Common;
using CommonCMS.Webs.Models;

namespace CommonCMS.Webs.Service
{
    public class ConfigDetailsValue
    {
        private static T GetFromTable<T>(string key)
        {
            var configDetailsModel = new ConfigDetailsModel();
            DapperConnection dapperConnection = new DapperConnection();
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();
                dictionary.Add("ParameterNames", key);

                ConfigDetailsModel objConfigDetailsModel = dapperConnection.GetListResult<ConfigDetailsModel>("GetConfigdetailsByName", System.Data.CommandType.StoredProcedure, dictionary).FirstOrDefault();
                if (objConfigDetailsModel != null)
                {
                    object obj = (objConfigDetailsModel.ParameterValue == null ? "" : objConfigDetailsModel.ParameterValue).ToString();
                    return (T)obj;
                }
                else
                {
                    object obj = ("DataNotFound" + "|" + false).ToString();
                    return (T)obj;
                }
            }
        }

        #region Email SMTP

        public static string SMTPServer
        {
            get
            {
                return GetFromTable<string>("SMTPServer");
            }
        }

        public static string SMTPPort
        {
            get
            {
                return GetFromTable<string>("SMTPPort");
            }
        }

        public static string SMTPAccount
        {
            get
            {
                return GetFromTable<string>("SMTPAccount");
            }
        }

        public static string SMTPPassword
        {
            get
            {
                return GetFromTable<string>("SMTPPassword");
            }
        }

        public static string SMTPFromEmail
        {
            get
            {
                return GetFromTable<string>("SMTPFromEmail");
            }
        }

        public static string SMTPIsSecure
        {
            get
            {
                return GetFromTable<string>("SMTPIsSecure");
            }
        }

        public static string SMTPIsTest
        {
            get
            {
                return GetFromTable<string>("SMTPIsTest");
            }
        }

        public static string TestSMTPServer
        {
            get
            {
                return GetFromTable<string>("TestSMTPServer");
            }
        }

        public static string TestSMTPPort
        {
            get
            {
                return GetFromTable<string>("TestSMTPPort");
            }
        }

        public static string TestSMTPAccount
        {
            get
            {
                return GetFromTable<string>("TestSMTPAccount");
            }
        }

        public static string TestSMTPPassword
        {
            get
            {
                return GetFromTable<string>("TestSMTPPassword");
            }
        }

        public static string TestSMTPFromEmail
        {
            get
            {
                return GetFromTable<string>("TestSMTPFromEmail");
            }
        }

        public static string TestSMTPIsSecure
        {
            get
            {
                return GetFromTable<string>("TestSMTPIsSecure");
            }
        }

        #endregion

    }
}
