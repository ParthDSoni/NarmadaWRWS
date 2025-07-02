
namespace NWRWS.Model.System
{
    public class CommonModel
    {
    }

    public class SMTPModel
    {
        public string SMTPServer { get; set; }
        public string? SMTPPort { get; set; }
        public string SMTPAccount { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPFromEmail { get; set; }
        public string? SMTPIsSecure { get; set; }
        public bool? SMTPIsSecureFlag {
            get { return SMTPIsSecure!=null? (SMTPIsSecure=="1" ?true:false): null; }
        }

        public bool SMTPIsTest { get; set; }

        public string TestSMTPServer { get; set; }
        public string? TestSMTPPort { get; set; }
        public string TestSMTPAccount { get; set; }
        public string TestSMTPPassword { get; set; }
        public string TestSMTPFromEmail { get; set; }
        public string? TestSMTPIsSecure { get; set; }
        public bool? TestSMTPIsSecureFlag
        {
            get { return TestSMTPIsSecure != null ? (TestSMTPIsSecure == "1" ? true : false) : null; }
        }

    }

    public class JsonResponseModel
    {
        public bool isError { get; set; } = true;
        public string strMessage { get; set; } = "";
        public string type { get; set; } = PopupMessageType.error.ToString();
        public dynamic result { get; set; }
    }

}
