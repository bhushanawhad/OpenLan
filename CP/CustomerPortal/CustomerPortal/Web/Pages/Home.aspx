<%@ Page Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="True" CodeBehind="Home.aspx.cs" Inherits="Site.Pages.Home" %>

<asp:Content ContentPlaceHolderID="Breadcrumbs" runat="server" />

<asp:Content ContentPlaceHolderID="Content" runat="server">
	<crm:CrmEntityDataSource ID="CurrentEntity" DataItem="<%$ CrmSiteMap: Current %>" runat="server" />
	<crm:Property DataSourceID="CurrentEntity" PropertyName="Adx_Copy" CssClass="copy" EditType="html" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server" />
