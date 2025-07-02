using System.ComponentModel.DataAnnotations;

namespace NWRWS.Webs.Models
{
    public class ForgetPasswordModel
    {
        [Required]
        [RegularExpression(@"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", ErrorMessage = "Please enter valid emailid.")]
        public string EmailId { get; set; }
    }
}
