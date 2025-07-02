
namespace NWRWS.Model.Service
{
    //Document Model
    public class DocumentModel
    {
        public long Doc_Id { get; set; }       
        public string? Doc_Name { get; set; }
        public string? File_Name { get; set; }
        public string? Doc_Path { get; set; }
        public long LanguageId { get; set; }        
        public bool IsActive { get; set; }
    }
}
