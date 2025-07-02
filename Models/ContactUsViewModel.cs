namespace NWRWS.Webs.Models
{
    public class ContactUsViewModel
    {
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string Address { get; set; }
    }
    public class CalanderFormModel
    {
        public long? hfIdCalander { get; set; }
        public string? hfAddNo { get; set; }
    }
    public class UpcomingExamsFormModel
    {
        public long? hfIdCalander { get; set; }
        public string? hfAddNo { get; set; }
    }
}
