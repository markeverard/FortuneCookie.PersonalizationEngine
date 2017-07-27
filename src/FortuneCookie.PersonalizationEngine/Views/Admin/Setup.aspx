<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="../Shared/PersonalizationEngine.Master" %>
<%@ Import Namespace="EPiServer.Shell.Navigation" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="epi-globalDocument">
        <%=Html.GlobalMenu() %>
        <div class="epi-padding">
            <div class="epi-contentContainer epi-fullWidth">
                <div>
                    <h1 class="EP-prefix">Personalization Engine</h1>
                    <ul class="EP-validationSummary">
                        <li>You must first configure <a href="<%=Url.Action("VisitorGroups", "Index") %>">Visitor Groups</a> before using the Personalization Engine.</li>
                    </ul>
                </div>

            </div>               
        </div>
    </div>
</asp:Content>
