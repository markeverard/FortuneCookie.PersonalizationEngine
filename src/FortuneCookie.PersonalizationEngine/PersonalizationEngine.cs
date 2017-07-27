using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using EPiServer.Core;
using EPiServer.Personalization.VisitorGroups;
using FortuneCookie.PersonalizationEngine.ContentProviders;
using FortuneCookie.PersonalizationEngine.Models;
using FortuneCookie.PersonalizationEngine.Services;

namespace FortuneCookie.PersonalizationEngine
{
    /// <summary>
    /// A set of methods for supplying personalized content.
    /// </summary>
    public class PersonalizationEngine
    {
        public PersonalizationEngine()
        {
            ContentProviderService = new DdsContentProviderService();
        }

        protected IContentProviderService ContentProviderService { get; set; }
        protected List<PageData> ContentPages { get; set; }

        private ContentProviderEventArgs _eventArgs;
        public static event OnContentProviderGetContentHandler OnContentProviderGetContent;

        /// <summary>
        /// Gets the recommended content based on the current IPrincipals Visitor Groups.
        /// </summary>
        /// <param name="principal">The IPrincipal.</param>
        /// <param name="languageBranch">The language branch.</param>
        /// <param name="pageCount">Number of pages to return.</param>
        /// <param name="visitorGroupContentProviders">A collection of VisitorGroupContentProviders</param>
        /// <returns></returns>
        public IEnumerable<PageData> GetRecommendedContent(IPrincipal principal, string languageBranch, int pageCount, IEnumerable<IVisitorGroupContentProvider> visitorGroupContentProviders)
        {
            InitializeContentPageList();

            foreach (var visitorGroupContentProvider in visitorGroupContentProviders)
            {
                if (ContentPages.Count() > pageCount)
                    break;

                IContentProvider contentProvider = visitorGroupContentProvider.GetContentProvider();
                if (!VisitorIsInGroup(principal, contentProvider))
                    continue;

                IEnumerable<PageData> providerPages = contentProvider.GetContent(languageBranch);
                RaiseContentProviderGetPagesEvent(providerPages, visitorGroupContentProvider.ContentProviderTypeName);
                                
                AddDistinctContentProviderPages(_eventArgs.ContentProviderPages);
            }

            return ContentPages.Take(pageCount).ToList();
        }

        private void RaiseContentProviderGetPagesEvent(IEnumerable<PageData> providerPages, string contentProviderTypeName)
        {
            _eventArgs = new ContentProviderEventArgs
                             {
                                 ContentProviderPages = providerPages,
                                 ContentProviderType = Type.GetType(contentProviderTypeName)
                             };
            OnContentProviderGetContent(_eventArgs);
        }

        public IEnumerable<PageData> GetRecommendedContent(IPrincipal principal, string languageBranch, int pageCount)
        {
            return GetRecommendedContent(principal, languageBranch, pageCount, GetVisitorGroupContentProviders());
        }

        /// <summary>
        /// Gets all visitor group content providers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IVisitorGroupContentProvider> GetVisitorGroupContentProviders()
        {
            return ContentProviderService.GetAllModels().Cast<IVisitorGroupContentProvider>();
        }

        /// <summary>
        /// Gets a subset of the visitor group content providers where they match the input Guids
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public IEnumerable<IVisitorGroupContentProvider> GetVisitorGroupContentProviders(Guid[] ids)
        {
            var allVisitorGroupContentProviders = GetVisitorGroupContentProviders();
            return allVisitorGroupContentProviders.Where(c => ids.Contains(c.UniqueId));
        }

        private void InitializeContentPageList()
        {
            ContentPages = new List<PageData>();
        }

        private void AddDistinctContentProviderPages(IEnumerable<PageData> providerPages)
        {
            IEnumerable<PageData> distinctPages = providerPages
                .Where(providerPage => !ContentPages.Contains(providerPage, new PageDataComparer()));

            foreach (var providerPage in distinctPages)
                ContentPages.Add(providerPage);
        }

        private bool VisitorIsInGroup(IPrincipal principal, IContentProvider contentProvider)
        {
            VisitorGroup group = new VisitorGroupStore().Load(contentProvider.VisitorGroupId);
            return new VisitorGroupHelper().IsPrincipalInGroup(principal, group.Name);
        }
    }
}
