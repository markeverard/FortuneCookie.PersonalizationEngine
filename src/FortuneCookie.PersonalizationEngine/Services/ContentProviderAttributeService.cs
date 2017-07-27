using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FortuneCookie.PersonalizationEngine.EditorModels;
using FortuneCookie.PersonalizationEngine.Models;
using FortuneCookie.PersonalizationEngine.Reflection;

namespace FortuneCookie.PersonalizationEngine.Services
{
    public class ContentProviderAttributeService
    {
        protected string ContentProviderCacheKey = "ContentProviderAttributeList";

        public IEnumerable<ContentProviderAttributeModel> GetContentProviderList()
        {
            IEnumerable<ContentProviderAttributeModel> contentProviderModels = GetCachedContentProviderList();
            if (contentProviderModels != null)
                return contentProviderModels;

            contentProviderModels = ScanForContentProviderList();
            SetCachedContentProviderList(contentProviderModels);
            return contentProviderModels;
        }

        private IEnumerable<ContentProviderAttributeModel> GetCachedContentProviderList()
        {
            return HttpRuntime.Cache[ContentProviderCacheKey] as IEnumerable<ContentProviderAttributeModel>;
        }

        private void SetCachedContentProviderList(IEnumerable<ContentProviderAttributeModel> contentProviderModels)
        {
            HttpRuntime.Cache[ContentProviderCacheKey] = contentProviderModels;
        }

        private IEnumerable<ContentProviderAttributeModel> ScanForContentProviderList()
        {
            List<Type> types = AttributedTypesUtility.GetTypesWithAttribute(typeof (VisitorGroupContentProviderAttribute));
            
            foreach (var type in types)
            {
                var typeAttribute =
                    AttributedTypesUtility.GetAttribute(type, typeof (VisitorGroupContentProviderAttribute)) 
                    as VisitorGroupContentProviderAttribute;

                if (typeAttribute == null)
                    continue;

                yield return new ContentProviderAttributeModel() { TypeName = type.FullName, DisplayName = typeAttribute.DisplayName, CriteriaEditModelTypeName = typeAttribute.CriteriaEditModel.FullName };
            }
        }

        /// <summary>
        /// Gets the CriteriaEditorModel type from the attribute specified on each IContentProvider.
        /// </summary>
        /// <param name="contentProviderTypeName">Name of the content provider type.</param>
        /// <returns></returns>
        public Type GetCriteriaEditorTypeFromAttribute(string contentProviderTypeName)
        {
            ContentProviderAttributeModel contentProviderAttribute = GetContentProviderAttribute(contentProviderTypeName);
            return contentProviderAttribute == null 
                ? typeof (DefaultCriteriaModel) 
                : Type.GetType(contentProviderAttribute.CriteriaEditModelTypeName);
        }

        /// <summary>
        /// Gets the ContentProvider attribute for the type specified by the contentProviderTypeName.
        /// </summary>
        /// <param name="contentProviderTypeName">Name of the content provider type.</param>
        /// <returns></returns>
        public ContentProviderAttributeModel GetContentProviderAttribute(string contentProviderTypeName)
        {
            return GetContentProviderList().Where(c => c.TypeName == contentProviderTypeName).SingleOrDefault();
        }
    }
}