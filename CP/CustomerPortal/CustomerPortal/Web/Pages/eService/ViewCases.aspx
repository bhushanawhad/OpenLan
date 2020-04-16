<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewCases.aspx.cs" Inherits="Site.Pages.eService.ViewCases" MasterPageFile="~/MasterPages/Default.master" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<div id="service-cases">
		<asp:Label Font-Bold="true" runat="server">View: </asp:Label>
		<asp:DropDownList ID="CustomerFilter" AutoPostBack="true" runat="server" CssClass="case-filter">
			<asp:ListItem>My</asp:ListItem>
			<asp:ListItem>My Company's</asp:ListItem>
			<asp:ListItem>All</asp:ListItem>
		</asp:DropDownList>
		<asp:DropDownList ID="StatusDropDown" AutoPostBack="true" runat="server" CssClass="case-filter">
			<asp:ListItem>Active</asp:ListItem>
			<asp:ListItem>Closed</asp:ListItem>
		</asp:DropDownList>
		<asp:GridView ID="CaseList" runat="server" CssClass="cases" OnRowDataBound="CaseList_OnRowDataBound" >
		</asp:GridView>
		<crm:CrmHyperLink ID="CreateCaseLink" SiteMarkerName="Create Case" runat="server"><crm:Snippet runat="server" SnippetName="cases/openNew" DefaultText="Open a New Case" /></crm:CrmHyperLink>
	</div>
</asp:Content>
