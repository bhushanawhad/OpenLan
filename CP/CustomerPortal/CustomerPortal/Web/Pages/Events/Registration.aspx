<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Default.master" CodeBehind="Registration.aspx.cs" Inherits="Site.Pages.Events.Registration" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<div id="event-registration" class="insert">
		<asp:Panel runat="server" ID="RegForm">
			<p><span class="required-marker">*</span> Indicates a required field</p>
		
			<div class="row">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="FirstName">
						<span class="required-marker">*</span>
						First Name
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="FirstName" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="1" ID="FirstName" />
				</div>
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="LastName">
						<span class="required-marker">*</span>
						Last Name
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="LastName" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="2" ID="LastName" />
				</div>
			</div>
			
			<div class="row">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="EMail">
						<span class="required-marker">*</span>
						E-Mail
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="EMail" Display="Dynamic" />
						<asp:RegularExpressionValidator CssClass="validator" runat="server" ControlToValidate="EMail" Text="Invailid Email Adress" ValidationExpression="^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" Display="Dynamic" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="3" ID="EMail" />
				</div>
			
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="Phone">
						<span class="required-marker">*</span>
						Phone
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="Phone" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="4" ID="Phone" />
				</div>
			</div>
		
			<div class="row">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="CompanyName" Text="Organization" />
					<asp:TextBox runat="server" CssClass="text" TabIndex="5" ID="CompanyName" />
				</div>
			
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="JobTitle" Text="Job Title" />
					<asp:TextBox runat="server" CssClass="text" TabIndex="6" ID="JobTitle" />
				</div>
			</div>
		
			<div class="row full">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="Address1">
						<span class="required-marker">*</span>
						Street Address 1
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="Address1" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="7" ID="Address1" />
				</div>
			</div>
		
			<div class="row full">	
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="Address2" Text="Street Address 2" />
					<asp:TextBox runat="server" CssClass="text" TabIndex="8" ID="Address2" />
				</div>
			</div>
		
			<div class="row full">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="Address3" Text="Street Address 3" />
					<asp:TextBox runat="server" CssClass="text" TabIndex="9" ID="Address3" />
				</div>
			</div>
		
			<div class="row">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="City">
						<span class="required-marker">*</span>
						City
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="City" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="10" ID="City" />
				</div>
			
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="State">
						<span class="required-marker">*</span>
						State/Province
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="State" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="11" ID="State" />
				</div>
			</div>
		
			<div class="row">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="PostalCode" Text="Postal Code" />
					<asp:TextBox runat="server" CssClass="text" TabIndex="12" ID="PostalCode" />
				</div>
			
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="Country">
						<span class="required-marker">*</span>
						Country
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="Country" />
					</asp:Label>
					<asp:TextBox runat="server" CssClass="text" TabIndex="13" ID="Country" />
				</div>
			</div>
		
			<div class="row full">
				<div class="field">
					<crm:CrmMetadataDataSource ID="CommunicationSource" runat="server" AttributeName="msa_preferredmethodofcommunication" EntityName="campaignresponse" />
					<asp:Label runat="server" AssociatedControlID="CommunicationMethod">
						<span class="required-marker">*</span>
						Preferred Method of Communication
						<asp:RequiredFieldValidator CssClass="validator" runat="server" ErrorMessage="Required" ControlToValidate="CommunicationMethod" />
					</asp:Label>
					<asp:DropDownList runat="server" CssClass="field-wide" TabIndex="14" ID="CommunicationMethod" DataSourceID="CommunicationSource" DataTextField="OptionLabel" DataValueField="OptionValue" AppendDataBoundItems="True">
						<asp:ListItem Text="" Value="" Selected="True" />
					</asp:DropDownList>
				</div>
			</div>
		
			<div class="row full">
				<div class="field">
					<asp:Label runat="server" AssociatedControlID="Notes" Text="Notes" />
					<asp:TextBox runat="server" CssClass="text" ID="Notes" TabIndex="15" TextMode="MultiLine" />
				</div>
			</div>
		
			<div class="row">
				<div class="cell captcha-cell">
					<script type="text/javascript" src="http://challenge.asirra.com/js/AsirraClientSide.js"></script>
				</div>
			</div>
		
			<div class="actions">
				<asp:Button runat="server" ID="SubmitButton" CssClass="register-button" TabIndex="17" Text='<%$ Snippet: Events/Register, Register Now %>' CausesValidation="true" OnClick="Register_Click" />
			</div>
		</asp:Panel>
		
		<asp:Panel runat="server" ID="ConfirmationMsg" Visible="false">
			<crm:Snippet runat="server" SnippetName="EventRegistration/ConfirmationMsg" DefaultText="Thank you. Your registration for this event has been received." EditType="html" />
			<div class="row">
				<asp:HyperLink ID="EventExportLink" CssClass="vevent" Text='<%$ Snippet: Events/ExportLink, Save event to calendar %>' runat="server" />
			</div>
		</asp:Panel>
	</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="Sidebar" runat="server">
	<div id="event-details" class="section first">
		<h2><asp:Label ID="EventName" runat="server" /></h2>
		
		<div class="event-topic"><asp:Label CssClass="detail" ID="EventTopic" runat="server" /></div>
		<div class="event-datetime"><asp:Label CssClass="detail" ID="EventDateTime" runat="server" /></div>
		<div class="event-address"><asp:Label CssClass="detail" ID="Address" runat="server" /></div>
		<div class="event-cost"><asp:Label CssClass="detail" ID="Cost" runat="server" /></div>
		<div class="event-location"><asp:Label CssClass="detail" ID="EventLocation" runat="server" /></div>

		<div class="event-brochure">
			<asp:HyperLink ID="EventBrochure" Text="Event Brochure" runat="server" />
		</div>

		<div class="event-contact">Contact Information: <asp:Label CssClass="detail" ID="EventContact" runat="server" /></div>
	</div>
</asp:Content>
