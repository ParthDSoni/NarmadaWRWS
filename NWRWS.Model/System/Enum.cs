using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWRWS.Model.System
{

    public class ListItem
    {
        public long regionId { get; set; }
        public int languageId { get; set; }
        public int CanalId { get; set; }
        public string Value { get; set; }
		
		public string UploadDocumentPath { get; set; }
		public string Description { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
       
    }

    public enum PopupMessageType
    {
        success,
        error,
        warning,
        info
    }

    public enum MenuType
    {
        ParentMenu,
        ChildMenu,
        InternalLinks,
        QuickLinks
    }


    public enum CMSMenuResType
    {
        StaticPage,
        DynamicPage
    }

    public enum CMSMenuType
    {
        ParentMenu,
        ChildMenu,
        InnerPage,
        QuickLinks
    }

    public enum FileType
    {
        [Description("png,jpeg,jpg,gif,tiff,jfif")]
        ImageType,
        [Description("mp4,mov,wmv,avi,avchd,flv,mkv,webm,swf,wma")]
        VideoType,
        [Description("png,jpeg,jpg,gif,tiff,mp4,mov,wmv,avi,avchd,flv,mkv,webm,swf,wma,jfif")]
        ImageVideoType,
        [Description("doc,docx,pdf,xls,xlsx,ppt,pptx,txt")]
        DocType,
        [Description("pdf")]
        PDFType,
        [Description("png,jpeg,jpg,doc,docx,pdf")]
        DocPdfType,
        [Description("png,jpeg,jpg,gif,tiff,doc,docx,pdf,xls,xlsx,ppt,pptx,txt,mpeg,mp3,apk,wmv,mpg,wma,mov,jfif,avi")]
        AllType
    }

    public enum TenderDocumentType
    {
        [Description ("Pre-Bid Document")]
        PreBid,

        [Description("Other Document")]
        Other
    }

    public enum CMSTemplateType
    {
        HomeLayout,
        FooterLayout,
        MenuLayout,
        InnerPageLayout,
        EventLayout,
        EventDetailsLayout,
        NewsLayout,
        NewsDetailsLayout,
        BlogLayout,
        BlogDetailsLayout,
        TenderLayout,
        TenderDetailsLayout,
        GallaryLayout,
        GallaryDetailsLayout,
        FeedBackLayout
    }
    public enum ControlInputType
    {
        none,
        text,
        number,
        email,
        password,
        mobileno,
        pincode,
        dropdown,
        date,
        time
    }
}
