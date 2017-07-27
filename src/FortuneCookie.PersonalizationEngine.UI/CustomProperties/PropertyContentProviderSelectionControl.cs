using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace FortuneCookie.PersonalizationEngine.UI.CustomProperties
{
    public class PropertyContentProviderSelectionControl : PropertyMultipleSelectionBaseControl
    {
        private TextBox _pageCountTextBox;

        protected override string AvailableItemsText { get { return "Available Content Providers"; }}
        protected override string SelectedItemsText { get { return "Selected Content Providers"; }}
        protected override int ListBoxWidth { get { return 550; }}

        /// <summary>
        /// Applies changes for the posted data to the page's properties when the <see cref="P:EPiServer.Web.PropertyControls.PropertyDataControl.RenderType"/> property is set to <see cref="F:EPiServer.Core.RenderType.Edit"/>.
        /// </summary>
        public override void ApplyEditChanges()
        {
            base.ApplyEditChanges();

            int pageCount;

            if (!int.TryParse(_pageCountTextBox.Text, out pageCount) || pageCount < 0)
            {
                AddErrorValidator("You must enter a valid page count");
                return;
            }

            SelectedContentProviders selectedContentProviders = new SelectedContentProviders
                                                                    {
                                                                        PageCount = pageCount,
                                                                        ContentProviderIds = SelectedValues.Select(current => new Guid(current)).ToList()
                                                                    };

            SerializationHelper.SerializeObject(selectedContentProviders);
            SetValue(selectedContentProviders);
        }

        /// <summary>
        /// Creates an edit interface for the property.
        /// </summary>
        public override void CreateEditControls()
        {
            SelectedContentProviders selectedContentProviders = PropertyData.Value as SelectedContentProviders ??
                                                                new SelectedContentProviders();

            if (selectedContentProviders.ContentProviderIds == null)
                selectedContentProviders.ContentProviderIds = new List<Guid>();

            SelectedValues = selectedContentProviders.ContentProviderIds.Select(current => current.ToString()).ToList();

            // add page count
            Label label = new Label { Text = "Page Count:&nbsp;" };
            Controls.Add(label);

            _pageCountTextBox = new TextBox { ID = "PageCount", Text = selectedContentProviders.PageCount.ToString() };
            Controls.Add(_pageCountTextBox);

            // add base multiple selection editing controls
            base.CreateEditControls();
        }

        /// <summary>
        /// Gets the bindable data.
        /// </summary>
        /// <returns>
        /// Dictionary of string keys and values
        /// </returns>
        protected override Dictionary<string, string> GetBindableData()
        {
            var contentProviders = new PersonalizationEngine().GetVisitorGroupContentProviders();
            return contentProviders.ToDictionary(contentProvider => contentProvider.UniqueId.ToString(),
                                                 contentProvider => string.Format("{0}, {1}, {2}", contentProvider.GetVisitorGroupName(),
                                                                                  contentProvider.ContentProviderTypeDisplayName,
                                                                                  Page.Server.HtmlEncode(contentProvider.ContentProviderCriteria)).TrimEnd(' ', ','));
        }
    }
}