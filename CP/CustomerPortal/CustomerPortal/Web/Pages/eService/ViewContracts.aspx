<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewContracts.aspx.cs" Inherits="Site.Pages.eService.ViewContracts" MasterPageFile="~/MasterPages/Default.master" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<div id="service-cases">
		<asp:Label Font-Bold="true" runat="server">View: </asp:Label>
<%--		<asp:DropDownList ID="CustomerFilter" AutoPostBack="true" runat="server" CssClass="case-filter">
			<asp:ListItem>My</asp:ListItem>
			<asp:ListItem>My Company's</asp:ListItem>
			<asp:ListItem>All</asp:ListItem>
		</asp:DropDownList>--%>
		<asp:DropDownList ID="StatusDropDown" AutoPostBack="true" runat="server" CssClass="case-filter">
			<asp:ListItem>Active</asp:ListItem>
			<asp:ListItem>In Active</asp:ListItem>
		</asp:DropDownList>
		<asp:GridView ID="ContractList" runat="server" CssClass="cases" >
		</asp:GridView>
		<%--<crm:CrmHyperLink ID="CreateCaseLink" SiteMarkerName="Create Case" runat="server"><crm:Snippet runat="server" SnippetName="cases/openNew" DefaultText="Open a New Case" /></crm:CrmHyperLink>--%>
	</div>
</asp:Content>
