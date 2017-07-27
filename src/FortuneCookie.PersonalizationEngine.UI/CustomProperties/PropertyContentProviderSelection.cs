using System;
using EPiServer.Core;
using EPiServer.PlugIn;

namespace FortuneCookie.PersonalizationEngine.UI.CustomProperties
{
    /// <summary>
    /// Content Provider selection property
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn(DisplayName = "Content Provider Selection Property", Description = "Content Provider Selection Property")]
    public class PropertyContentProviderSelection : PropertyData
    {
        private SelectedContentProviders _selectedContentProviders;

        public PropertyContentProviderSelection()
        {
        }

        public PropertyContentProviderSelection(SelectedContentProviders selectedContentProviders)
        {
            _selectedContentProviders = selectedContentProviders;
        }

        public override Type PropertyValueType { get { return typeof(SelectedContentProviders); }}
        public override PropertyDataType Type { get { return PropertyDataType.LongString; }}

        public override object Value
        {
            get { return _selectedContentProviders; }
            set
            {
                SetPropertyValue(value, delegate
                {
                    if (value == null)
                        SelectedContentProviders = new SelectedContentProviders();
                    else if (value is string)
                        SelectedContentProviders = (SelectedContentProviders)Parse((string)value).Value;
                    else if (value is SelectedContentProviders)
                        SelectedContentProviders = (SelectedContentProviders)value;
                });
            }
        }

        public virtual SelectedContentProviders SelectedContentProviders
        {
            get { return _selectedContentProviders; }
            set
            {
                ThrowIfReadOnly();

                if ((EqualTo(_selectedContentProviders, value)) && !IsNull) 
                    return;

                _selectedContentProviders = value;
                Modified();
            }
        }

        public override string ToString()
        {
            return Value == null ? string.Empty : SerializationHelper.SerializeObject(Value);
        }

        /// <summary>
        /// Parses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Current property type</returns>
        public static PropertyContentProviderSelection Parse(string value)
        {
            return new PropertyContentProviderSelection(SerializationHelper.DeserializeObject<SelectedContentProviders>(value));
        }

        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyContentProviderSelectionControl();
        }

        /// <summary>
        /// Parses the object string value to a PropertyData object
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>PropertyData type</returns>
        public override PropertyData ParseToObject(string value)
        {
            return Parse(value);
        }

        /// <summary>
        /// Parses a string representation to the current value type
        /// </summary>
        /// <param name="value">The value.</param>
        public override void ParseToSelf(string value)
        {
            Value = Parse(value).Value;
        }

        /// <summary>
        /// Sets the default value.
        /// </summary>
        protected override void SetDefaultValue()
        {
            ThrowIfReadOnly();
            _selectedContentProviders = new SelectedContentProviders();
        }

        private static bool EqualTo(SelectedContentProviders entity1, SelectedContentProviders entity2)
        {
            if (entity1 == null || entity2 == null)
                return false;

            return string.Equals(SerializationHelper.SerializeObject(entity1), SerializationHelper.SerializeObject(entity2));
        }

        public override void LoadData(object value)
        {
            Value = value;
        }

        public override object SaveData(PropertyDataCollection properties)
        {
            return SerializationHelper.SerializeObject(Value);
        }
    }
}