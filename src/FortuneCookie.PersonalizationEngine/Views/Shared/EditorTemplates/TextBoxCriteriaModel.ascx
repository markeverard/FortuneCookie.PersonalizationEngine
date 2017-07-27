<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FortuneCookie.PersonalizationEngine.EditorModels.TextBoxCriteriaModel>" %>

<%=Html.LabelFor(c => Model.Criteria) %>
<%=Html.TextBoxFor(c => Model.Criteria) %>
