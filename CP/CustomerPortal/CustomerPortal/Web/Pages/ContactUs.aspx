<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ContactUs.aspx.cs" Inherits="Site.Pages.ContactUs" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">

	<asp:Panel ID="ContactForm" CssClass="contact-us insert" runat="server" Visible="true">
		<crm:CrmDataSource ID="WebFormDataSource" runat="server" />
		<crm:CrmEntityFormView runat="server" ID="FormView" DataSourceID="WebFormDataSource" EntityName="lead" SavedQueryName="Lead Web Form" OnItemInserted="OnItemInserted" ValidationGroup="ContactUs">
			<InsertItemTemplate>
				<fieldset>
					<div class="row">
						<div class="cell captcha-cell">
							<script type="text/javascript" src="http://challenge.asirra.com/js/AsirraClientSide.js"></script>
						</div>
					</div>
				</fieldset>
				<div class="actions">
					<asp:Button ID="SubmitButton" Text='<%$ Snippet: ContactUs/Submit, Submit %>' CssClass="button" CommandName="Insert" ValidationGroup="ContactUs" runat="server" />
				</div>
			</InsertItemTemplate>
		</crm:CrmEntityFormView>
	</asp:Panel>
	
	<asp:Panel ID="ConfirmationMessage" runat="server" Visible="false">
		<crm:Snippet runat="server" SnippetName="ContactUs/ConfirmationMsg" EditType="html" />
	</asp:Panel>
	
</asp:Content>
