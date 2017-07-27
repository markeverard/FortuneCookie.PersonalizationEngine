using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using FortuneCookie.PersonalizationEngine.EditorModels;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    [VisitorGroupContentProvider(DisplayName = "Pages in category content provider",
                                Description = "Performs a FindPagesWithCriteria search against the selected Category criteria",
                                CriteriaEditModel = typeof(CategoryListCriteriaModel))]
    public class PagesInCategoryContentProvider : CachedContentProviderBase
    {
        protected override IEnumerable<PageReference> GetContentReferences(string languageBranch)
        {
            var criterias = new PropertyCriteriaCollection() { PageIsInCategoryCriteria };
            PageDataCollection pages = DataFactory.Instance.FindPagesWithCriteria(PageReference.StartPage, criterias, languageBranch);
            return pages.Select(p => p.PageLink);
        }

        private PropertyCriteria PageIsInCategoryCriteria
        {
            get
            {
                return new PropertyCriteria
                    {
                        Name = "PageCategory",
                        Type = PropertyDataType.Category,
                        Condition = CompareCondition.Equal,
                        Value = ContentCriteria,
                        Required = true
                    };
            }
        }
    }
}