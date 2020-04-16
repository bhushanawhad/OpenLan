<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="Site.Pages.eService.Profile" MasterPageFile="~/MasterPages/Default.master" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<crm:CrmEntityDataSource ID="CurrentEntity" DataItem="<%$ CrmSiteMap: Current %>" runat="server" />

	<asp:panel id="RegistrationPanel" runat="server">
		<div class="page-copy">
			<crm:Property DataSourceID="CurrentEntity" PropertyName="Adx_Copy" EditType="html" runat="server" />
		</div>
		<crm:Snippet ID="ProfileUpdatedMsg" runat="server" SnippetName="Profile/UpdatedMessage" DefaultText="Your profile has been updated successfully." Visible="false" EditType="html" />
		<div class="register-form">
		
		<asp:PlaceHolder ID="AccountInformation" runat="server">
		
			<h3><crm:Snippet runat="server" SnippetName="Profile/AccountInfo" DefaultText="Account Information" /></h3>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountName" DefaultText="Account Name:" />
				<asp:TextBox ID="AccountName" runat="server" />
				<asp:RequiredFieldValidator ID="AccountNameFieldValidator" runat="server" 
							ControlToValidate="AccountName"
							ErrorMessage="This field is required" 
							Display="Dynamic" 
							CssClass="validation-message" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/Website" DefaultText="Website:" />
				<asp:TextBox ID="AccountWebsite" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountAddress1" DefaultText="Address 1:" />
				<asp:TextBox ID="AccountAddress1" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountAddress2" DefaultText="Address 2:" />
				<asp:TextBox ID="AccountAddress2" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountAddress3" DefaultText="Address 3:" />
				<asp:TextBox ID="AccountAddress3" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountCity" DefaultText="City:" />
				<asp:TextBox ID="AccountCity" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountState" DefaultText="State/Province:" />
				<asp:TextBox ID="AccountProvince" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/AccountPostalCode" DefaultText="Zip/Postal Code:" />
				<asp:TextBox ID="AccountPostalCode" runat="server" />
			</div>
			<div class="row" style="margin-bottom: 1.5em;">
				<crm:Snippet runat="server" SnippetName="Profile/AccountCountry" DefaultText="Country:" />
				<asp:TextBox ID="AccountCountry" runat="server" />
			</div>
			
		</asp:PlaceHolder>
		
		<asp:PlaceHolder ID="SelfInformation" runat="server">
		
			<h3><crm:Snippet runat="server" SnippetName="Profile/MyInfo" DefaultText="My Information" /></h3>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/FirstName" DefaultText="First Name:" />
				<asp:TextBox ID="FirstName" runat="server" />
				<asp:RequiredFieldValidator ID="FirstNameFieldValidator" runat="server" 
							ControlToValidate="FirstName"
							ErrorMessage="This field is required" 
							Display="Dynamic" 
							CssClass="validation-message" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/LastName" DefaultText="Last Name:" />
				<asp:TextBox ID="LastName" runat="server" />
				<asp:RequiredFieldValidator ID="LastNameFieldValidator" runat="server" 
							ControlToValidate="LastName"
							ErrorMessage="This field is required" 
							Display="Dynamic" 
							CssClass="validation-message" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/Email" DefaultText="Email:" />
				<asp:TextBox ID="EmailAddress" runat="server" />
				<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
							ControlToValidate="EmailAddress"
							ErrorMessage="Email address is required" 
							Display="Dynamic" 
							CssClass="validation-message" />
				<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
							ControlToValidate="EmailAddress"
							ErrorMessage="Email address is invalid" 
							ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" 
							CssClass="validation-message" Display="Dynamic" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactPhone" DefaultText="Phone Number:" />
				<asp:TextBox ID="PhoneNumber" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactJobTitle" DefaultText="Title:" />
				<asp:TextBox ID="JobTitle" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactAddress1" DefaultText="Address 1:" />
				<asp:TextBox ID="ContactAddressLine1" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactAddress2" DefaultText="Address 2:" />
				<asp:TextBox ID="ContactAddressLine2" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactAddress3" DefaultText="Address 3:" />
				<asp:TextBox ID="ContactAddressLine3" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactCity" DefaultText="City:" />
				<asp:TextBox ID="ContactCity" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactState" DefaultText="State/Province:" />
				<asp:TextBox ID="ContactState" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactZip" DefaultText="Zip/Postal Code:" />
				<asp:TextBox ID="ContactZip" runat="server" />
			</div>
			<div class="row">
				<crm:Snippet runat="server" SnippetName="Profile/ContactCountry" DefaultText="Country:" />
				<asp:TextBox ID="ContactCountry" runat="server" />
			</div>
			<div class="row" style="margin-bottom: 1.5em;">
				<crm:Snippet runat="server" SnippetName="Profile/TimeZone" DefaultText="Timezone:" /><br />
				<asp:DropDownList runat="server" ID="TimeZoneSelection" />
			</div>
			<div class="marketing-preferences">
				<crm:Snippet runat="server" SnippetName="Profile/MarketingPref" DefaultText="How may we contact you?  Select all that apply:" /><br />
				<crm:Snippet runat="server" SnippetName="Profile/MarketEmail" DefaulTText="Email" />
				<asp:CheckBox runat="server" ID="marketEmail" />
				<crm:Snippet runat="server" SnippetName="Profile/MarketFax" DefaulTText="Fax" />
				<asp:CheckBox runat="server" ID="marketFax" />
				<crm:Snippet runat="server" SnippetName="Profile/MarketPhone" DefaulTText="Phone" />
				<asp:CheckBox runat="server" ID="marketPhone" />
				<crm:Snippet runat="server" SnippetName="Profile/MarketMail" DefaulTText="Mail" />
				<asp:CheckBox runat="server" ID="marketMail" />
			</div>
			
			<h3><crm:Snippet runat="server" SnippetName="Profile/EmailLists" DefaultText="Email Lists" /></h3>
			<ul class="link-list">
				<asp:Repeater ID="MarketingList" runat="server">
					<ItemTemplate>
						<li><asp:CheckBox ID="ListCheckbox" runat="server" Checked='<%# IsListChecked(Container.DataItem) %>' Text=' <%# Eval("listname") %>'/><asp:HiddenField ID="ListID" Value='<%# Eval("ListID") %>' runat="server" /></li>
						<li><%# Eval("purpose") %></li>
					</ItemTemplate>
				</asp:Repeater>
			</ul>
		</asp:PlaceHolder>		
		</div>
		<asp:button id="SubmitButton" CssClass="PostButton" runat="server" onclick="SubmitButton_Click" />
	</asp:panel>
</asp:Content>
