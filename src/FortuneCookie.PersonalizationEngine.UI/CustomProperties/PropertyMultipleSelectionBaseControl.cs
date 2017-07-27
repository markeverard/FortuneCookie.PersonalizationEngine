using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.UI.WebControls;
using EPiServer.Web.PropertyControls;

namespace FortuneCookie.PersonalizationEngine.UI.CustomProperties
{
    /// <summary>
    /// Multiple selection property base control
    /// </summary>
    public abstract class PropertyMultipleSelectionBaseControl : PropertyDataControl
    {
        private ListBox _availableOptions;
        private ListBox _selectedOptions;
        private HiddenField _selectedValuesHiddenField;
        private List<string> _selectedValues = new List<string>();

        protected List<string> SelectedValues
        {
            get { return _selectedValues; }
            set { _selectedValues = value ?? new List<string>(); }
        }

        protected virtual string AvailableItemsText { get { return "Available Items"; }}
        protected virtual string SelectedItemsText { get { return "Selected Items"; }}
        protected virtual string AddButtonText { get { return "Add"; }}
        protected virtual string RemoveButtonText { get { return "Remove"; }}
        protected virtual string MoveUpText { get { return "Move Up"; }}
        protected virtual string MoveDownText { get { return "Move Down"; }}
        protected virtual int ListBoxWidth { get { return 450; }}
        protected virtual int ListBoxRows { get { return 15; }}
        protected virtual int? MinimumNumberOfSelectedValues { get { return null; }}
        protected virtual int? MaximumNumberOfSelectedValues { get { return null; }}
        protected virtual string MinimumNumberOfSelectedValuesErrorMessageFormatString { get { return "You must select at least {0} values"; }}
        protected virtual string MaximumNumberOfSelectedValuesErrorMessageFormatString { get { return "You must select no more than {0} values"; }}

        /// <summary>
        /// Gets the bindable data.
        /// </summary>
        /// <returns>Dictionary of string keys and values</returns>
        protected virtual Dictionary<string, string> GetBindableData()
        {
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (PropertyData != null && PropertyData.Value != null)
                SelectedValues = PropertyData.Value.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Applies changes for the posted data to the page's properties when the <see cref="P:EPiServer.Web.PropertyControls.PropertyDataControl.RenderType"/> property is set to <see cref="F:EPiServer.Core.RenderType.Edit"/>.
        /// </summary>
        public override void ApplyEditChanges()
        {
            SelectedValues = _selectedValuesHiddenField.Value.TrimEnd(new[] { ',' })
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            bool valid = true;

            if (MinimumNumberOfSelectedValues.HasValue && SelectedValues.Count < MinimumNumberOfSelectedValues.Value)
            {
                AddErrorValidator(string.Format(MinimumNumberOfSelectedValuesErrorMessageFormatString, MinimumNumberOfSelectedValues.Value));
                valid = false;
            }

            if (MaximumNumberOfSelectedValues.HasValue && SelectedValues.Count > MaximumNumberOfSelectedValues.Value)
            {
                AddErrorValidator(string.Format(MaximumNumberOfSelectedValuesErrorMessageFormatString, MaximumNumberOfSelectedValues.Value));
                valid = false;
            }

            if (!valid)
                return;

            SetValue(SelectedValues.Aggregate(string.Empty, (current, value) => current + (value + ",")).TrimEnd(new[] { ',' }));
        }

        /// <summary>
        /// Creates an edit interface for the property.
        /// </summary>
        public override void CreateEditControls()
        {
            AddJavascript();
            CreateHiddenSelectedValuesControl(this);

            Panel formPanel = new Panel { CssClass = "epiformArea" };
            Controls.Add(formPanel);

            Panel panel = new Panel { CssClass = "epi-floatLeft" };
            panel.Attributes.Add("style", "clear:both;margin-top:8px");
            formPanel.Controls.Add(panel);

            Dictionary<string, string> data = GetBindableData();
            CreateAvailableItemsControl(panel, data);

            panel = new Panel { CssClass = "epi-floatLeft" };
            panel.Attributes.Add("style", string.Format("text-align:center;clear:both;width:{0}px;margin-top:8px", ListBoxWidth));
            formPanel.Controls.Add(panel);

            ToolButton addItemButton = new ToolButton
            {
                ID = "AddItem",
                ToolTip = AddButtonText,
                Text = AddButtonText,
                SkinID = "Add"
            };

            panel.Controls.Add(addItemButton);

            AddLiteral(panel, "&nbsp;");

            ToolButton removeItemButton = new ToolButton
            {
                ID = "RemoveItem",
                ToolTip = RemoveButtonText,
                Text = RemoveButtonText,
                SkinID = "Delete"
            };
            panel.Controls.Add(removeItemButton);

            panel = new Panel { CssClass = "epi-floatLeft" };
            panel.Attributes.Add("style", "clear:left");
            formPanel.Controls.Add(panel);

            Panel subPanelContainer = CreateSelectedItemsControl(panel, data);

            string moveRightClick = string.Format("C7D3468_Move({0}, {1}, {2}, {3})",
                                                  _availableOptions.ClientID,
                                                  _selectedOptions.ClientID,
                                                  _selectedOptions.ClientID,
                                                  _selectedValuesHiddenField.ClientID);

            _availableOptions.Attributes.Add("ondblclick", moveRightClick);
            addItemButton.OnClientClick = moveRightClick + ";return false;";

            string moveLeftClick = string.Format("C7D3468_Move({0}, {1}, {2}, {3})",
                                                 _selectedOptions.ClientID,
                                                 _availableOptions.ClientID,
                                                 _selectedOptions.ClientID,
                                                 _selectedValuesHiddenField.ClientID);

            _selectedOptions.Attributes.Add("ondblclick", moveLeftClick);
            removeItemButton.OnClientClick = moveLeftClick + ";return false;";

            Panel subPanel = new Panel { CssClass = "epi-floatLeft" };
            subPanelContainer.Controls.Add(subPanel);

            ImageButton moveUp = new ImageButton
            {
                ID = "MoveUp",
                ToolTip = MoveUpText,
                ImageUrl = EPiServer.Web.PageExtensions.ThemeUtility.GetImageThemeUrl(Page, "Tools/Up.gif")
            };

            subPanel.Controls.Add(moveUp);

            ImageButton moveDown = new ImageButton
            {
                ID = "MoveDowwn",
                ToolTip = MoveDownText,
                ImageUrl = EPiServer.Web.PageExtensions.ThemeUtility.GetImageThemeUrl(Page, "Tools/Down.gif")
            };
            subPanel.Controls.Add(moveDown);

            moveUp.OnClientClick = string.Format("C7D3468_MoveUpAndDown({0}, true, {1});return false;",
                                                 _selectedOptions.ClientID,
                                                 _selectedValuesHiddenField.ClientID);

            moveDown.OnClientClick = string.Format("C7D3468_MoveUpAndDown({0}, false, {1});return false;",
                                                   _selectedOptions.ClientID,
                                                   _selectedValuesHiddenField.ClientID);
        }

        /// <summary>
        /// Adds a label.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="text">The text.</param>
        private static void AddLabel(Control parent, string text)
        {
            Label label = new Label { Text = text };
            label.Attributes.Add("style", "float:left");
            parent.Controls.Add(label);
        }

        /// <summary>
        /// Adds a literal.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="text">The text.</param>
        private static void AddLiteral(Control parent, string text)
        {
            Literal label = new Literal { Text = text };
            parent.Controls.Add(label);
        }

        /// <summary>
        /// Adds the javascript.
        /// </summary>
        private void AddJavascript()
        {
            if (Page.ClientScript.IsClientScriptBlockRegistered(typeof(Page), "C7D3468_Move"))
                return;

            const string script = @"
                function C7D3468_Move(oSelectFrom, oSelectTo, oSelectToUpdateFrom, oToUpdate)
                {
	                while (oSelectFrom.selectedIndex >= 0)
                    {
                        var nIndex = oSelectFrom.selectedIndex;
                        var newOption = new Option(oSelectFrom.options[nIndex].text, oSelectFrom.options[nIndex].value);

		                try
		                {
		                    // IE way
		                    oSelectTo.add(newOption);
		                }
		                catch(ex)
		                {
		                    // DOM level 2
		                    oSelectTo.add(newOption,null);
		                }

		                oSelectFrom.remove(nIndex);
	                }
                    
                    if (nIndex > (oSelectFrom.options.length - 1))
                        nIndex = oSelectFrom.options.length - 1;

                    if (nIndex >= 0)
                        oSelectFrom.selectedIndex = nIndex;

                    C7D3468_Update(oSelectToUpdateFrom, oToUpdate);
                }
                function C7D3468_Update(oSelectToUpdateFrom, oToUpdate)
                {
                    var selections = '';

                    for (i = 0; i < oSelectToUpdateFrom.options.length; i++)
                        selections += oSelectToUpdateFrom.options[i].value + ',';

                    oToUpdate.value = selections;
                }
                function C7D3468_MoveUpAndDown(oSelectBox, up, oToUpdate) {
                    if (oSelectBox.length < 2)
                        return;

                    var idx = oSelectBox.selectedIndex;
                    var nxidx = idx + (up ? -1 : 1);

                    if (nxidx < 0)
                        nxidx = oSelectBox.length - 1;
                    if (nxidx>=oSelectBox.length) 
                        nxidx = 0;
                        
                    var oldVal = oSelectBox[idx].value;
                    var oldText = oSelectBox[idx].text;
                    oSelectBox[idx].value = oSelectBox[nxidx].value;
                    oSelectBox[idx].text = oSelectBox[nxidx].text;
                    oSelectBox[nxidx].value = oldVal;
                    oSelectBox[nxidx].text = oldText;
                    oSelectBox.selectedIndex = nxidx;
                    C7D3468_Update(oSelectBox, oToUpdate);
                }";

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "C7D3468_Move", script, true);
        }

        /// <summary>
        /// Creates the hidden selected values control.
        /// </summary>
        /// <param name="parent">The parent.</param>
        private void CreateHiddenSelectedValuesControl(Control parent)
        {
            _selectedValuesHiddenField = new HiddenField
                                             {
                                                 ID = "SelectedValuesHiddenField",
                                                 Value = SelectedValues.Aggregate(string.Empty, (current, value) => current + (value + ","))
                                             };
            parent.Controls.Add(_selectedValuesHiddenField);

            if (Page.Request.Form[_selectedValuesHiddenField.UniqueID] != null)
                _selectedValuesHiddenField.Value = Page.Request.Form[_selectedValuesHiddenField.UniqueID];

            SelectedValues = _selectedValuesHiddenField.Value.TrimEnd(new[] { ',' })
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        /// <summary>
        /// Creates the available items control.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="data">The data.</param>
        private void CreateAvailableItemsControl(Control parent, Dictionary<string, string> data)
        {
            AddLabel(parent, AvailableItemsText);
            AddLiteral(parent, "<br />");

            _availableOptions = new ListBox
            {
                ID = "AvailableItems",
                SkinID = "Size300",
                Rows = ListBoxRows,
                SelectionMode = ListSelectionMode.Multiple
            };
            _availableOptions.Attributes.Add("style", string.Format("width:{0}px", ListBoxWidth));

            foreach (KeyValuePair<string, string> item in data.Where(item => !SelectedValues.Any(current => string.Equals(current, item.Key))))
                _availableOptions.Items.Add(new ListItem(item.Value, item.Key));

            parent.Controls.Add(_availableOptions);
        }

        /// <summary>
        /// Creates the selected items control.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="data">The data.</param>
        /// <returns>The container panel</returns>
        private Panel CreateSelectedItemsControl(Control parent, Dictionary<string, string> data)
        {
            AddLabel(parent, SelectedItemsText);
            AddLiteral(parent, "<br />");

            Panel subPanelContainer = new Panel { CssClass = "epi-floatLeft" };
            parent.Controls.Add(subPanelContainer);

            Panel subPanel = new Panel { CssClass = "epi-floatLeft" };
            subPanelContainer.Controls.Add(subPanel);

            _selectedOptions = new ListBox
            {
                ID = "SelectedItems",
                SkinID = "Size300",
                Rows = ListBoxRows,
                SelectionMode = ListSelectionMode.Multiple
            };
            _selectedOptions.Attributes.Add("style", string.Format("width:{0}px", ListBoxWidth));

            foreach (string value in SelectedValues.Where(data.ContainsKey))
                _selectedOptions.Items.Add(new ListItem(data[value], value));

            subPanel.Controls.Add(_selectedOptions);
            return subPanelContainer;
        }
    }
}
