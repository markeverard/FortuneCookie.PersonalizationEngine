using System.Collections.Generic;
using EPiServer.Core;

namespace FortuneCookie.PersonalizationEngine
{
    public class PageDataComparer : IEqualityComparer<PageData>
    {
        public bool Equals(PageData x, PageData y)
        {
            if (x == null || y == null)
                return false;

            return (x.PageLink.ID == y.PageLink.ID)
                   && (x.PageLink.WorkID == y.PageLink.WorkID);
        }

        public int GetHashCode(PageData obj)
        {
            return obj.PageLink.ID.GetHashCode() + obj.PageLink.WorkID.GetHashCode();
        }
    }
}