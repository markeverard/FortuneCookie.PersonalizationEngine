using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using EPiServer.Core;

namespace FortuneCookie.PersonalizationEngine.Models
{
    public class AdminExampleViewModel
    {
        public AdminExampleViewModel()
        {
            RecommendedContent = new List<PageData>();
            SelectedVisitorGroupIds = new string[0];
            SelectedLanguageBranchItem = string.Empty;
            SelectedPageCountItem = string.Empty;
        }

        public string[] SelectedVisitorGroupIds { get; set; }
        public IEnumerable<SelectListItem> VisitorGroupItems { get; set; }

        [DisplayName("Display up to: ")]
        public string SelectedPageCountItem { get; set; }
        public IEnumerable<SelectListItem> PageCountItems { get; set; }
        public int PageCount
        {
            get
            {
                int pageCount = 10;
                int.TryParse(SelectedPageCountItem, out pageCount);
                return pageCount;
            }
        }
        

        [DisplayName("from language branch")]
        public string SelectedLanguageBranchItem { get; set; }
        public IEnumerable<SelectListItem> LanguageBranchItems { get; set; }
  
        public IEnumerable<PageData> RecommendedContent { get; set; }
    }
}