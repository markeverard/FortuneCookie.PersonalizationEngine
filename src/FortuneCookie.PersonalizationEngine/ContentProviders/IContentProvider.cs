using System;
using System.Collections.Generic;
using EPiServer.Core;

namespace FortuneCookie.PersonalizationEngine.ContentProviders
{
    public interface IContentProvider
    {
        IEnumerable<PageData> GetContent(string languageBranch);
        Guid VisitorGroupId { get; set; }
        string ContentCriteria { get; set; }
    }
}