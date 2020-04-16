<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="Site.Pages.Error" %>
<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<asp:Panel ID="FederationError" Visible="false" runat="server">
		<h2>Details</h2>
		<asp:GridView ID="ErrorDetails" runat="server" AutoGenerateColumns="true" />
		<h2>Errors</h2>
		<asp:GridView ID="ErrorList" runat="server" AutoGenerateColumns="true" />
	</asp:Panel>
</asp:Content>
