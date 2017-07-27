using System;
using System.Collections.Generic;
using FortuneCookie.PersonalizationEngine.Models;

namespace FortuneCookie.PersonalizationEngine.Services
{
    public interface IContentProviderService
    {
        VisitorGroupContentProviderModel GetModel(Guid id);
        IEnumerable<VisitorGroupContentProviderModel> GetAllModels();
        void Add(VisitorGroupContentProviderModel model);
        void Update(VisitorGroupContentProviderModel model);
        void Delete(Guid id);
        void Reorder(Guid id, ReorderDirection direction);
        int GetMaxSortOrder();
    }

    public enum ReorderDirection
    {
        Up,
        Down
    }
}