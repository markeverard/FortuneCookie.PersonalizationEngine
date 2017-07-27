using System;
using FortuneCookie.PersonalizationEngine.EditorModels;

namespace FortuneCookie.PersonalizationEngine
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class VisitorGroupContentProviderAttribute : Attribute
    {
        public VisitorGroupContentProviderAttribute()
        {
            DisplayName = string.Empty;
            Description = string.Empty;
            CriteriaEditModel = typeof(DefaultCriteriaModel);
        }

        public string DisplayName { get; set; }
        public string Description { get; set; }

        public Type CriteriaEditModel { get; set; }
    }
}