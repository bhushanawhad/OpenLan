<%@ Page Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="ProductRegistration.aspx.cs" Inherits="Site.Pages.eService.ProductRegistration" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentBottom">
	<asp:Panel ID="ProductRegistrationFormPanel" runat="server" CssClass="register-product insert">
		<div class="row">
			<asp:Label runat="server" ID="RegistrationMessage" />
		</div>
		<asp:Panel runat="server" ID="RegForm">
			<div class="row">
				<asp:Label runat="server" CssClass="cell-info" Text="Product" AssociatedControlID="Product" /><br />
				<asp:DropDownList runat="server" ID="Product" />
			</div>
			<crm:CrmDataSource ID="WebFormDataSource" runat="server" />
			<crm:CrmEntityFormView ID="createOpp" runat="server" DataSourceID="WebFormDataSource"
				EntityName="msa_productregistration" SavedQueryName="Product Registration Web View"
				ValidationGroup="RegisterProduct" OnItemInserting="OnItemInserting" OnItemInserted="OnItemInserted">
				<InsertItemTemplate>
					<div class="actions">
						<asp:Button Text='<%$ Snippet: RegisterProduct/RegisterButton, Register %>' CssClass="button"
							CommandName="Insert" CausesValidation="true" ValidationGroup="RegisterProduct"
							runat="server" />
					</div>
				</InsertItemTemplate>
			</crm:CrmEntityFormView>
		</asp:Panel>
	</asp:Panel>
	<asp:Panel ID="NoProductsMessage" runat="server" Visible="false">
		<p><strong>There are no products available to be registered.</strong></p>
	</asp:Panel>
</asp:Content>
