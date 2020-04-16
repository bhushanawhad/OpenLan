<%@ Page Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="True" CodeBehind="SiteMap.aspx.cs" Inherits="Site.Pages.SiteMap" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<div class="site-map insert">
		<asp:SiteMapDataSource ID="SiteMapDataSource" runat="server" />
		<asp:TreeView DataSourceID="SiteMapDataSource" runat="server" />
	</div>
</asp:Content>
