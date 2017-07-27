using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiServer.Web.WebControls;
using FortuneCookie.PersonalizationEngine.EditorModels;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    [VisitorGroupContentProvider(DisplayName = "Page search content provider", 
                                Description="Performs a full-text page property search against the defined criteria", 
                                CriteriaEditModel = typeof(TextBoxCriteriaModel))]
    public class PageSearchContentProvider : CachedContentProviderBase
    {
        protected override IEnumerable<PageReference> GetContentReferences(string languageBranch)
        {
            var searchPageSource = new SearchDataSource
                                       {
                                           PageLink = PageReference.StartPage,
                                           OnlyWholeWords = true,
                                           SearchFiles = false,
                                           IncludeRootPage = true,
                                           LanguageBranches = languageBranch
                                       };

            searchPageSource.SelectParameters.Add(new Parameter("SearchQuery", TypeCode.String, ContentCriteria));
            searchPageSource.DataBind();

            return searchPageSource.Select(DataSourceSelectArguments.Empty)
                .OfType<PageData>()
                .Select(p => p.PageLink);
        }
    }
}
