using System;
using EPiServer.Data;
using FortuneCookie.PersonalizationEngine.Services;

namespace FortuneCookie.PersonalizationEngine.Models
{
    public static class VisitorGroupContentProviderFactory
    {
        public static VisitorGroupContentProviderModel Create(string contentProviderTypeName, Guid visitorGroupId, string criteriaValue)
        {
            return new VisitorGroupContentProviderModel
                {
                    Id = Identity.NewIdentity(),
                    VisitorGroupId = visitorGroupId,
                    ContentProviderCriteria = criteriaValue,
                    ContentProviderTypeName = contentProviderTypeName,
                    ContentProviderTypeDisplayName = new ContentProviderAttributeService().GetContentProviderAttribute(contentProviderTypeName).DisplayName,
                    SortOrder = new DdsContentProviderService().GetMaxSortOrder() + 1
                };
        }
    }
}