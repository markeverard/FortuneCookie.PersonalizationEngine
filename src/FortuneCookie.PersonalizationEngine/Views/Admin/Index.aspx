<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FortuneCookie.PersonalizationEngine.Models.AdminViewModel>" MasterPageFile="../Shared/PersonalizationEngine.Master" %>
<%@ Import Namespace="EPiServer.Shell.Navigation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">    
    <style type="text/css">
        
        .validation-summary-errors {
                background-color: #FFFDBD;
                background-image: url("/App_Themes/Default/Images/Tools/Warning.gif");
                background-position: 0.8em center;
                background-repeat: no-repeat;
                border: 1px solid #878787;
                margin: 0.5em 0;
                overflow: hidden;
                padding: 0.4em 0;
                width: 99.7%;
          }
        .validation-summary-errors ul li { margin-left: 3em;}
        
    </style>
    
    <script type="text/javascript">

        function getCriteriaEditorTemplate(contentProviderType) {

            dojo.byId("criteriaTemplate").innerHTML = "<label>Loading....</label><img src='<%=EPiServer.Web.PageExtensions.ThemeUtility.GetImageThemeUrl(Page, "General/AjaxLoader.gif") %>' />";
            dojo.xhrPost({
                url: '<%= Url.Action("CriteriaEditorTemplate") %>',
                handleAs: 'text',
                preventCache: true,
                content: { contentProviderType: contentProviderType },
                error: function(error) { alert(error); },
                load: function(data) {
                    dojo.byId("criteriaTemplate").innerHTML = data;
                }
            });
            return false;
        }

        function removeValidationSummaryErrors()
        {
            dojo.query(".validation-summary-errors").orphan();
        }

        function toggleElement(elementId, show) {
            var style = show ? "block" : "none";
            var node = dojo.byId(elementId);
            node.style.display = style;
        }

        dojo.addOnLoad(function () {
            var contentProviderDropDown = dojo.byId("ContentProviderTypeName");
            dojo.connect(contentProviderDropDown, "onchange", function () {
                removeValidationSummaryErrors();
                getCriteriaEditorTemplate(this.options[this.selectedIndex].value);
                return false;
            });

            var contentProviderAdd = dojo.byId("Add");
            dojo.connect(contentProviderAdd, "click", function () {
                toggleElement("contentProviderForm", true);
                toggleElement("addButtonRow", false);
                return false;
            });

            var contentProviderCancel = dojo.byId("Cancel");
            dojo.connect(contentProviderCancel, "click", function () {
                toggleElement("contentProviderForm", false);
                toggleElement("addButtonRow", true);
                return false;
            });
        });
    
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="epi-globalDocument">
    <%=Html.GlobalMenu() %>
        <div class="epi-padding">
            <div class="epi-contentContainer epi-fullWidth">
            <div class="epi-contentArea">
                <h1 class="EP-prefix">Personalization Engine</h1>
                    <p><span class="EP-systemInfo">
                        The Personalization engine delivers targeted content to visitors matching defined Visitor Groups.
                    </span></p>
            </div>
                
            <div class="epi-formArea"> 
                <div id="addButtonRow" class="epitoolbuttonrow" style="<%=Model.FormPanelVisibility(!ViewData.ModelState.IsValid) %>">
                    <span class="epi-cmsButton">
                        <input type="button" name="Add" id="Add"
                            onmouseover="EPi.ToolButton.MouseDownHandler(this)" 
                            onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)"
                            class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Add" value="Add" />
                    </span>
                    <span class="epi-cmsButton">
                        <input type="button" value="Test" onclick="window.open('<%= Url.Action("Example") %>', 'example', 'menubar=0,resizable=1,width=600,height=700');"
                            onmouseover="EPi.ToolButton.MouseDownHandler(this)" 
                            onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" 
                            class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-EditMode" />
                    </span>                        
                    <span class="epi-cmsButton">
                        <input type="button" value="Refresh cache" onclick="document.location='<%= Url.Action("RefreshCache") %>';"
                            onmouseover="EPi.ToolButton.MouseDownHandler(this)" 
                            onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)" 
                            class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Refresh" />
                    </span>
                </div>

              
                <% using (Html.BeginForm("Index", "Admin", FormMethod.Post, new { id = "contentProviderForm", style=Model.FormPanelVisibility(ViewData.ModelState.IsValid) } )) %>
                <%
                   {%>
                    <fieldset>
                        <legend>Add new rule</legend> 
                            
                        <div class="epi-size10">                        
                            <div>
                                <%=Html.LabelFor(m => m.VisitorGroupId)%>
                                <%=Html.DropDownListFor(m => m.VisitorGroupId,
                                                              Model.VisitorGroupList)%>
                            </div>
                        </div>

                        <div class="epi-size10">                        
                            <div>
                                <%=Html.LabelFor(m => m.ContentProviderTypeName)%>
                                <%=Html.DropDownListFor(m => m.ContentProviderTypeName,
                                                Model.ContentProviderList)%>
                            </div>
                        </div>

                        <div class="epi-size10">                        
                            <div id="criteriaTemplate">
                                <% Html.RenderAction("CriteriaEditorTemplate", "Admin", new { contentProviderType = Model.ContentProviderList.SelectedValue });%>
                            </div>
                        </div>
                          <%= Html.ValidationSummary() %>

                        <div class="epi-buttonContainer" style="border:0;">
                            <span class="epi-cmsButton">
                                <input type="submit" name="Save" value="Save" class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Save" />
                            </span>
                            <span class="epi-cmsButton">
                                <input type="button" name="Cancel" id="Cancel"
                                    onmouseover="EPi.ToolButton.MouseDownHandler(this)" 
                                    onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)"
                                    class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Cancel" value="Cancel" />
                            </span>
                        </div>
                    </fieldset>
               <%} %>
                <table class="epi-default">
                    <tbody>
                        <tr>
                            <th class="episize50">#</th>
                            <th class="episize300">Visitor Group</th>
                            <th>Content Provider</th>
                            <th class="episize200">Criteria</th>
                            <th class="episize100">Action</th>
                        </tr>
                        <% for (var i=0; i < Model.ContentProviders.Count(); i++)
                            { %>
                            <tr>
                                <td><%=(i + 1).ToString() %></td>
                                <td><%= Model.ContentProviders.ElementAt(i).GetVisitorGroupName()%></td>
                                <td><%= Model.ContentProviders.ElementAt(i).ContentProviderTypeDisplayName%></td>
                                <td><%= Model.ContentProviders.ElementAt(i).ContentProviderCriteria%></td>
                                <td>
                                    <a title="Move up" href="<%= Url.Action("ReorderUp", new { id = Model.ContentProviders.ElementAt(i).Id.ExternalId }) %>">
                                        <img alt="Move up" src="<%=EPiServer.Web.PageExtensions.ThemeUtility.GetImageThemeUrl(Page, "Tools/Up.gif") %>" />
                                    </a>
                                    <a title="Move down" href="<%= Url.Action("ReorderDown", new { id = Model.ContentProviders.ElementAt(i).Id.ExternalId }) %>">
                                        <img alt="Down" src="<%=EPiServer.Web.PageExtensions.ThemeUtility.GetImageThemeUrl(Page, "Tools/Down.gif") %>" /> 
                                    </a>
                                    <a title="Delete" href="<%= Url.Action("Delete", new { id = Model.ContentProviders.ElementAt(i).Id.ExternalId }) %>">
                                        <img alt="Delete" src="<%= EPiServer.Web.PageExtensions.ThemeUtility.GetImageThemeUrl(Page, "Tools/Delete.gif") %>" />
                                    </a>
                                </td>
                            </tr>
                        <% } %>
                    </tbody>
                </table>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
