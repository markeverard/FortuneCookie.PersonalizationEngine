<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<FortuneCookie.PersonalizationEngine.Models.AdminExampleViewModel>" MasterPageFile="../Shared/PersonalizationEngine.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">

    <div class="epi-padding">
        <div class="epi-contentContainer">
            <div class="epi-contentArea">
                <h1 class="EP-prefix">Test Personalization Engine</h1>
                <p><span class="EP-systemInfo">
                    View the recommended content based on the selected Visitor Group combinations.
                </span></p>
            </div>

            <div class="epi-formArea">
            <% using (Html.BeginForm()) %>
            <% {%>

                <fieldset>
                    <legend>View as Visitor Group</legend> 
                    <ul>
                    <%
                       foreach (var item in Model.VisitorGroupItems)
                       {%>
                        <li>
                            <input type="checkbox" name="SelectedVisitorGroupIds" value="<%=item.Value %>" <%=item.Selected ? "checked=\"checked\"" : string.Empty %> />
                            <label><%=item.Text %></label>
                        </li>
                        <% }%>
                    </ul>
                    <br />

                    <div>
                        <%=Html.LabelFor(m => m.SelectedPageCountItem)%>
                        <%=Html.DropDownListFor(m => m.SelectedPageCountItem, Model.PageCountItems)%>
                    
                        <%=Html.LabelFor(m => m.SelectedLanguageBranchItem)%>
                        <%=Html.DropDownListFor(m => m.SelectedLanguageBranchItem, Model.LanguageBranchItems)%>
                    </div>

                    <div class="epi-buttonContainer" style="border:0;">
                        <span class="epi-cmsButton">
                            <input type="submit" name="Show" 
                                onmouseover="EPi.ToolButton.MouseDownHandler(this)" 
                                onmouseout="EPi.ToolButton.ResetMouseDownHandler(this)"
                                class="epi-cmsButton-text epi-cmsButton-tools epi-cmsButton-Add" value="Show" />
                        </span>
                    </div>

                </fieldset>
                <% }%>
            </div>

            <h3>Recommended Content</h3>
            <ul>
            <% foreach (var page in Model.RecommendedContent) %>
            <% { %>
                <li><a target="_blank" href="<%=page.LinkURL%>"><%=page.PageName %></a></li>
                <% } %>
            </ul>
            <% if (!Model.RecommendedContent.Any()) %>
            <% { %>
                <p>The personalisation engine recommended no content for your selected combination of Visitor groups.</p>
            <% } %>
        </div>
    </div>

</asp:Content>