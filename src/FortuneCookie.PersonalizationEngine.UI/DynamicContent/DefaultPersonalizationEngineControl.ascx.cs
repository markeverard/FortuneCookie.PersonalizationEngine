using System;
using EPiServer;
using EPiServer.DynamicContent;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Web.WebControls;

namespace FortuneCookie.PersonalizationEngine.UI.DynamicContent
{
    [DynamicContentPlugIn(
        DisplayName = "Personalization Engine - Default",
        Description = "Displays the full list of results from the defined Personalization Engine rules",
        ViewUrl = "~/Templates/PersonalizationEngine/DynamicContent/DefaultPersonalizationEngineControl.ascx")]
    public class DefaultPersonalizationEngineControl : UserControlBase
    {
        protected PageList PageListControl;

        public int PageCount { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            BindPersonalizationEngineResults();
        }

        private void BindPersonalizationEngineResults()
        {
            var engine = new PersonalizationEngine();
            IEnumerable<PageData> pages = engine.GetRecommendedContent(Page.User, CurrentPage.LanguageBranch, PageCount);
            
            PageListControl.DataSource = pages;
            PageListControl.DataBind();
        }


        public void BindData(IEnumerable<PageData> pages)
        {
            PageListControl.DataSource = pages;
            PageListControl.DataBind();
        }
    }
}
