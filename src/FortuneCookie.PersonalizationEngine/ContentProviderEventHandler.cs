using System;
using System.Collections.Generic;
using EPiServer.Core;

namespace FortuneCookie.PersonalizationEngine
{
    public delegate void OnContentProviderGetContentHandler(ContentProviderEventArgs e);

    public class ContentProviderEventArgs : EventArgs
    {
        public Type ContentProviderType { get; set; }
        public IEnumerable<PageData> ContentProviderPages { get; set; }
    }
}
