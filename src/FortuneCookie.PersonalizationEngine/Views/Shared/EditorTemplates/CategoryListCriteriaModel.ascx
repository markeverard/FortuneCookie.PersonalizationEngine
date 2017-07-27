<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FortuneCookie.PersonalizationEngine.EditorModels.CategoryListCriteriaModel>" %>

<%= Html.LabelFor(m => m.Criteria)%>
<%= Html.DropDownListFor(m => m.Criteria, Model.CategorySelectListItems)%>