using System;
using EPiServer;
using EPiServer.DynamicContent;
using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.Web.WebControls;
using FortuneCookie.PersonalizationEngine.UI.CustomProperties;

namespace FortuneCookie.PersonalizationEngine.UI.DynamicContent
{
    [DynamicContentPlugIn(
        DisplayName = "Personalization Engine - Selected rules",
        Description = "Displays the results from the editor-selected Personalization Engine rules",
        ViewUrl = "~/Templates/PersonalizationEngine/DynamicContent/SelectedPersonalizationEngineControl.ascx")]
    public class SelectedPersonalizationEngineControl : UserControlBase
    {
        protected PageList PageListControl;
        public PropertyContentProviderSelection ContentProviders { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            IEnumerable<PageData> pageResults = GetPersonalizationEngineResults();
            BindResults(pageResults);
        }

        protected IEnumerable<PageData> GetPersonalizationEngineResults()
        {
            var engine = new PersonalizationEngine();
            
            var selectedContentProviders = ContentProviders.Value as SelectedContentProviders;
            if (selectedContentProviders == null)
                throw new FormatException("PropertyContentProviderSelection.Value cannot be cast as SelectedContentProviders");

            return engine.GetRecommendedContent(Page.User, CurrentPage.LanguageBranch, selectedContentProviders.PageCount, selectedContentProviders.GetSelectedContentProviders());
        }

        public void BindResults(IEnumerable<PageData> pages)
        {
            PageListControl.DataSource = pages;
            PageListControl.DataBind();
        }
    }
}
