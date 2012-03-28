<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    if (Request.IsAuthenticated) {
%>
        Bienvenue <b><%: Page.User.Identity.Name %></b>!
        [ <%: Html.ActionLink("Fermer la session", "LogOff", "Account") %> ]
<%
    }
    else {
%> 
        [ <%: Html.ActionLink("Ouvrir une session", "LogOn", "Account") %> ]
<%
    }
%>
