using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FortuneCookie.PersonalizationEngine.Models;

namespace FortuneCookie.PersonalizationEngine.UI.CustomProperties
{
    /// <summary>
    /// Selected content providers class
    /// </summary>
    [Serializable]
    [DataContract]
    [KnownType(typeof(SelectedContentProviders))]
    public class SelectedContentProviders
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedContentProviders"/> class.
        /// </summary>
        public SelectedContentProviders()
        {
            ContentProviderIds = new List<Guid>();
            PageCount = 0;
        }

        [DataMember]
        public List<Guid> ContentProviderIds { get; set; }

        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets the selected content providers.
        /// </summary>
        /// <returns>List of selected IVisitorGroupContentProvider</returns>
        public IEnumerable<IVisitorGroupContentProvider> GetSelectedContentProviders()
        {
            if (ContentProviderIds == null)
                ContentProviderIds = new List<Guid>();

            var personalizationEngine = new PersonalizationEngine();
            return personalizationEngine.GetVisitorGroupContentProviders(ContentProviderIds.ToArray());
        }
    }
}