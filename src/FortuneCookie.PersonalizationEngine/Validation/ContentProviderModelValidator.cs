using System;
using System.Collections.Generic;
using EPiServer.Personalization.VisitorGroups;
using FortuneCookie.PersonalizationEngine.EditorModels;
using FortuneCookie.PersonalizationEngine.Models;
using FortuneCookie.PersonalizationEngine.Services;

namespace FortuneCookie.PersonalizationEngine.Validation
{
    internal class ContentProviderModelValidator
    {
        protected IContentProviderService ContentProviderService { get; set; }

        internal ContentProviderModelValidator()
        {
            ContentProviderService = new DdsContentProviderService();
        }

        /// <summary>
        /// Ensures VisitorGroupContentProviderModels are removed from the datastore if the underlying IContentProvider cannot be found
        /// </summary>
        internal void RemoveModelsWithUnknownContentProvider()
        {
            IEnumerable<VisitorGroupContentProviderModel> allSavedModels = ContentProviderService.GetAllModels();

            foreach (var contentProviderModel in allSavedModels)
            {
                try
                {
                    contentProviderModel.GetContentProvider();
                }
                catch (FormatException)
                {
                    ContentProviderService.Delete(contentProviderModel.Id.ExternalId);
                }
            }
        }

        /// <summary>
        /// Ensures VisitorGroupContentProviderModels are removed from the datastore, if the underlying VisitorGroup has been removed 
        /// </summary>
        internal void RemoveModelsWithUnknownVisitorGroup()
        {
            var groupStore = new VisitorGroupStore();
            IEnumerable<VisitorGroupContentProviderModel> allSavedModels = ContentProviderService.GetAllModels();

            foreach (var contentProviderModel in allSavedModels)
            {
                VisitorGroup group = groupStore.Load(contentProviderModel.VisitorGroupId);
                if (group == null)
                    ContentProviderService.Delete(contentProviderModel.Id.ExternalId);
            }
        }

        /// <summary>
        /// Ensures that all CriteriaEditorModels defined in VisitorGroupContentAttributes implement ICriteriaModel  
        /// </summary>
        internal void VerifyCriteriaEditorModels()
        {
            var attributeModels = new ContentProviderAttributeService().GetContentProviderList();

            foreach (var contentProviderAttribute in attributeModels)
            {
                Type type = Type.GetType(contentProviderAttribute.CriteriaEditModelTypeName);
                if (type == null)
                    throw new FormatException(string.Format("Type {0} does not exist", contentProviderAttribute.CriteriaEditModelTypeName));

                if (!typeof (ICriteriaModel).IsAssignableFrom(type))
                    throw new FormatException(string.Format("Editor Criteria type {0} must implement ICriteriaModel", contentProviderAttribute.CriteriaEditModelTypeName));
            }

        }
    }
}