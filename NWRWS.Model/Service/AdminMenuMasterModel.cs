namespace NWRWS.Model.Service
{
    public class AdminMenuMasterModel
    {
        public long Id { get; set; }
        public long MenuId { get; set; }
        public string Name { get; set; }
        public string MenuType { get; set; }
        public long? ParentId { get; set; }
        public string ParentName { get; set; }
        public long MenuRank { get; set; }
        public string MenuURL { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public string CreatedBy { get; set; }
    }
    public class SearchFormModel
    {
        public string serch { get; set; }
    }
}
