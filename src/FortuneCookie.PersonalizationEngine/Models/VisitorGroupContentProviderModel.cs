using System;
using EPiServer.Data;
using EPiServer.Data.Dynamic;
using EPiServer.Personalization.VisitorGroups;
using FortuneCookie.PersonalizationEngine.ContentProviders;

namespace FortuneCookie.PersonalizationEngine.Models
{
    [EPiServerDataStore(AutomaticallyCreateStore = true, AutomaticallyRemapStore = true)]
    public class VisitorGroupContentProviderModel : IDynamicData, IVisitorGroupContentProvider
    {
        public VisitorGroupContentProviderModel()
        {
            GroupStore = new VisitorGroupStore();
        }

        protected IVisitorGroupRepository GroupStore { get; set; }
        
        public Identity Id { get; set; }
        public Guid UniqueId { get { return Id.ExternalId; } }

        public Guid VisitorGroupId { get; set; }
        public string ContentProviderTypeName { get; set; }
        public string ContentProviderTypeDisplayName { get; set; }
        public string ContentProviderCriteria { get; set; }
        
        public int SortOrder { get; set; }

        public IContentProvider GetContentProvider()
        {
            var contentProviderType = Type.GetType(ContentProviderTypeName);
            var contentproviderInstance = Activator.CreateInstance(contentProviderType) as IContentProvider;
            if (contentproviderInstance == null)
                throw new FormatException(string.Format("Could not create IContentProvider instance for {0}", contentProviderType));

            contentproviderInstance.VisitorGroupId = VisitorGroupId;
            contentproviderInstance.ContentCriteria = ContentProviderCriteria;

            return contentproviderInstance;
        }

        public Type GetContentProviderType()
        {
            return Type.GetType(ContentProviderTypeName);
        }

        public string GetVisitorGroupName()
        {
            var group = GroupStore.Load(VisitorGroupId);
            if (group == null)
                throw new NullReferenceException(string.Format("Visitor group does not exist - id = {0}", VisitorGroupId));

            return group.Name;
        }
    }
}