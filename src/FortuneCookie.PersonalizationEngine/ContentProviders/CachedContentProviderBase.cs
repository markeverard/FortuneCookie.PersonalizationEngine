using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using EPiServer;
using EPiServer.Core;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    public abstract class CachedContentProviderBase : IContentProvider
    {
        private const string CacheDependencyKey = "FortuneCookie.PersonalizationEngine";
        
        public string CacheKeyName(string languageBranch)
        {
            return string.Format("{0}_{1}_{2}", GetType().FullName, ContentCriteria, languageBranch);
        }

        public IEnumerable<PageData> GetContent(string languageBranch)
        {
            string key = CacheKeyName(languageBranch);
            IEnumerable<PageReference> pageReferences = GetCachedContentReferences(key);
            
            if (pageReferences == null)
            {
                pageReferences = GetContentReferences(languageBranch);
                SetCachedContentReferences(key, pageReferences);
            }

            return pageReferences.Select(pageReference => DataFactory.Instance.GetPage(pageReference, new LanguageSelector(languageBranch)));        
        }

        private IEnumerable<PageReference> GetCachedContentReferences(string cacheKey)
        {
            return HttpRuntime.Cache[cacheKey] as IEnumerable<PageReference>;
        }

        private void SetCachedContentReferences(string cacheKey, IEnumerable<PageReference> contentReferences)
        {
            var dependency = new CacheDependency(null, new string[] {CacheDependencyKey});
            var cacheSlidingExpiration = new TimeSpan(0, 20, 0);
            HttpRuntime.Cache.Add(cacheKey, contentReferences, dependency, Cache.NoAbsoluteExpiration, cacheSlidingExpiration, CacheItemPriority.Normal, null);
        }

        /// <summary>
        /// Initialises the content provider cache - clears all ContentProvider page references caches.
        /// </summary>
        public static void InitialiseContentProviderCache()
        {
            HttpRuntime.Cache[CacheDependencyKey] = Guid.NewGuid();
        }

        protected abstract IEnumerable<PageReference> GetContentReferences(string languageBranch);
        
        public Guid VisitorGroupId { get; set; }
        public virtual string ContentCriteria { get; set; }
    }
}