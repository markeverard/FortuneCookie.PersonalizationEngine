using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Configuration;
using EPiServer.Core;
using EPiServer.Web.WebControls;
using FortuneCookie.PersonalizationEngine.EditorModels;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    [VisitorGroupContentProvider(DisplayName = "Search referrer page search content provider",
                                Description = "Performs a full-text page property search against the search term found in the user's http header", 
                                CriteriaEditModel = typeof(SearchReferrerCriteriaModel))]
    public class SearchReferrerContentProvider : CachedContentProviderBase
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

        public override string ContentCriteria
        {
            get
            {
                var context = HttpContext.Current;
                if (context.Request.UrlReferrer != null)
                {
                    Match m = Regex.Match(context.Request.UrlReferrer.Query, SearchPatternRegex);
                    return HttpUtility.UrlDecode(m.Groups["query"].Value.Replace('+', ' '));
                }
                return string.Empty;
            }
        }

        protected string SearchPatternRegex
        {
            get
            {
                return EPiServerSection.Instance.VisitorGroup.SearchKeyWordCriteria.Pattern;
            }
        }
    }
}