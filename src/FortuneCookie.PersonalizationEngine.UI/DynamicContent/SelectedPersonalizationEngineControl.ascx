<%@ Control Language="C#" Inherits="FortuneCookie.PersonalizationEngine.UI.DynamicContent.SelectedPersonalizationEngineControl" %>
<%@ Register TagPrefix="EPiServer" Namespace="EPiServer.Web.WebControls" %>

<EPiServer:PageList ID="PageListControl" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
            <EPiServer:Property PropertyName="PageLink" runat="server" />
        </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</EPiServer:PageList>