using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using FortuneCookie.PersonalizationEngine.EditorModels;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    [VisitorGroupContentProvider(DisplayName = "Pages with pagename content provider", 
                                Description = "Performs a FindPagesWithCriteria search against the PageName property using the defined criteria",
                                CriteriaEditModel = typeof(TextBoxCriteriaModel))]
    public class PagesWithPageNameContentProvider : CachedContentProviderBase
    {
        protected override IEnumerable<PageReference> GetContentReferences(string languageBranch)
        {
            var criterias = new PropertyCriteriaCollection() { PageHasPageNameCriteria };
            PageDataCollection pages = DataFactory.Instance.FindPagesWithCriteria(PageReference.StartPage, criterias, languageBranch);
            return pages.Select(p => p.PageLink);
        }

        private PropertyCriteria PageHasPageNameCriteria
        {
            get
            {
                return new PropertyCriteria
                {
                    Name = "PageName",
                    Type = PropertyDataType.String,
                    Condition = CompareCondition.Contained,
                    Value = ContentCriteria,
                    Required = true
                };
            }
        }
    }
}