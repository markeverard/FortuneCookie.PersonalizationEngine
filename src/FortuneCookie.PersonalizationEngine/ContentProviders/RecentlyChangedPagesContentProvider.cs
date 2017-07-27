using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Cms.Shell.UI.Models.RecentlyChangedPages;
using EPiServer.Core;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    [VisitorGroupContentProvider(DisplayName = "Recently changed pages content provider",
                                Description = "Performs a RecentlyChangedPagesFinder search to return up to 50 recently changed pages")]
    public class RecentlyChangedPagesContentProvider : CachedContentProviderBase
    {
        protected override IEnumerable<PageReference> GetContentReferences(string languageBranch)
        {
            RecentlyChangedPagesFinder finder = new RecentlyChangedPagesFinder();

            IEnumerable<RecentlyChangedPage> recentPages = finder.Find(50)
                    .Where(p => p.Status == VersionStatus.Published.ToString());
             
            var pageReferenceList = new List<PageReference>();

            foreach (RecentlyChangedPage recentlyChangedpage in recentPages)
            {
                int pageId;
                if (!int.TryParse(recentlyChangedpage.ID, out pageId))
                    continue;

                var pageReference = new PageReference(pageId);
                if (PageReference.IsNullOrEmpty(pageReference))
                    continue;

                var page = DataFactory.Instance.GetPage(pageReference, new LanguageSelector(languageBranch));
                if (page == null)
                    continue;

                pageReferenceList.Add(pageReference);
            }

            return pageReferenceList;
        }
    }
}