<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="KbArticle.aspx.cs" Inherits="Site.Pages.eService.KbArticle" %>

<asp:Content runat="server" ContentPlaceHolderID="Content" >
	<div runat="server" id="CrmPageCopy" class="page-copy" />
	<asp:Label runat="server" ID="EmailSentMessage" Text="Email this Article:" /><br />
	<asp:RequiredFieldValidator runat="server" ID="EmailValidator" ControlToValidate="toEmailAddresses" Display="Dynamic" ErrorMessage="Email Address is Required" />
	<asp:RegularExpressionValidator runat="server" ControlToValidate="toEmailAddresses" ErrorMessage="Email address is invalid" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" Display="Dynamic" /><br />
	<asp:TextBox runat="server" ID="toEmailAddresses" CssClass="emailAddressInput" ToolTip="Email address" />
	<asp:Button runat="server" Text="<%$ Snippet: KnowledgeBase/Send %>" OnClick="SendKBArticle_Click" CssClass="emailBtn" /><br />
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentBottom" runat="server" />
