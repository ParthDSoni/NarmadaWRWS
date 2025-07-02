using NWRWS.Model.Service;

namespace NWRWS.Webs.Models
{
    public class MenubarViewModel
    {
        public List<CMSMenuMasterModel> ParentMenus {get; set;}
        public List<CMSMenuMasterModel> ChildMenus {get; set;}
         
    }
}
