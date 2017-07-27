using System;
using FortuneCookie.PersonalizationEngine.ContentProviders;

namespace FortuneCookie.PersonalizationEngine.Models
{
    public interface IVisitorGroupContentProvider
    {
        Guid UniqueId { get; }
        Guid VisitorGroupId { get; set; }
        string ContentProviderTypeName { get; set; }
        string ContentProviderTypeDisplayName { get; set; }
        string ContentProviderCriteria { get; set; }
        IContentProvider GetContentProvider();
        string GetVisitorGroupName();
    }
}