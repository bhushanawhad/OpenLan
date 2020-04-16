<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ConfirmInvite.aspx.cs" Inherits="Site.Pages.ConfirmInvite" %>

<asp:Content ContentPlaceHolderID="Breadcrumbs" runat="server" />

<asp:Content ContentPlaceHolderID="Content" runat="server">
	<asp:PlaceHolder ID="PageContent" runat="server">
		<crm:CrmEntityDataSource ID="CurrentEntity" DataItem="<%$ CrmSiteMap: Current %>" runat="server" />
		<h1>
			<crm:Property DataSourceID="CurrentEntity" PropertyName="Adx_Title,Adx_name" EditType="text" runat="server" />
		</h1>
		<crm:Property DataSourceID="CurrentEntity" PropertyName="Adx_Copy" CssClass="copy" EditType="html" runat="server" />
	</asp:PlaceHolder>
</asp:Content>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<div id="register">
		<asp:panel id="RegistrationPanel" CssClass="form frame" runat="server">
			<div class="validation-summary">
				<asp:Label runat="server" ID="ConfirmMessage" Visible="false" Text="<%$ Snippet: Registration/Confirm, Please check your email for a confirmation link or an invitation code. %>"></asp:Label>
				<asp:Label runat="server" ID="UnregisteredMessage" Visible="false" Text="<%$ Snippet: Registration/Unregistered, This account is not eligible for transfer. Please sign in with a registered Windows Live ID account. %>"></asp:Label>
				<asp:Label runat="server" ID="InactiveMessage" Visible="false" Text="<%$ Snippet: Registration/Inactive, This account is pending registration. %>"></asp:Label>
				<asp:Label runat="server" ID="InvalidMessage" Visible="false" Text="<%$ Snippet: Registration/Invalid, The invitation code is invalid. Please enter the code and try again. %>"></asp:Label>
			</div>
			<asp:PlaceHolder ID="Registration" runat="server">
				<fieldset class="insert">
					<legend><crm:Snippet runat="server" SnippetName="Register/Heading" DefaultText="Register by invitation" /></legend>
					<div class="row">
						<div class="field required">
							<asp:Label runat="server" AssociatedControlID="InvitationCode"><crm:Snippet runat="server" SnippetName="Register/InvitationCode" DefaultText="Invitation Code" /></asp:Label>
							<asp:TextBox ID="InvitationCode" CssClass="text" runat="server" />
							<asp:RequiredFieldValidator ID="InvitationCodeFieldValidator" runat="server" 
								ControlToValidate="InvitationCode"
								ErrorMessage="This field is required" 
								Display="Dynamic" 
								CssClass="validation-message"
								ValidationGroup="Registration"/>
							<asp:CustomValidator ID="InvitationCodeCustomValidator" runat="server"
								OnServerValidate="InvitationCodeCustomValidator_ServerValidate"
								ControlToValidate="InvitationCode"
								ErrorMessage="Invalid code"
								Display="Dynamic"
								CssClass="validation-message"
								ValidationGroup="Registration"/>
						</div>
					</div>
					<div class="row">
						<asp:button ID="SubmitInvitationCodeButton" runat="server" ValidationGroup="Registration" CssClass="button" Text="Submit" />
					</div>
				</fieldset>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="Challenge" runat="server" Visible="false">
				<fieldset class="insert">
					<legend><asp:Label ID="ChallengeQuestion" runat="server" /></legend>
					<div class="row">
						<div class="field required">
							<asp:Label runat="server" AssociatedControlID="ChallengeAnswer"><crm:Snippet runat="server" SnippetName="Register/ChallengeAnswer" DefaultText="Answer" /></asp:Label>
							<asp:TextBox ID="ChallengeAnswer" CssClass="text" TextMode="Password" runat="server" />
							<asp:RequiredFieldValidator ID="ChallengeAnswerFieldValidator" runat="server" 
								ControlToValidate="ChallengeAnswer"
								ErrorMessage="This field is required" 
								Display="Dynamic" 
								CssClass="validation-message"
								ValidationGroup="Challenge"/>
							<asp:CustomValidator ID="ChallengeAnswerCustomValidator" runat="server"
								OnServerValidate="ChallengeAnswerCustomValidator_ServerValidate"
								ControlToValidate="ChallengeAnswer"
								ErrorMessage="Invalid answer"
								Display="Dynamic"
								CssClass="validation-message"
								ValidationGroup="Challenge"/>
						</div>
					</div>
					<div class="row">
						<asp:button id="SubmitChallengeAnswerButton" runat="server" ValidationGroup="Challenge" CssClass="button" Text="Submit" onclick="SubmitChallengeAnswerButton_Click" />
					</div>
				</fieldset>
			</asp:PlaceHolder>
			<asp:PlaceHolder ID="OpenRegistration" runat="server">
				<fieldset class="insert">
					<legend><crm:Snippet runat="server" SnippetName="Register/OpenRegistration" DefaultText="Sign-up for a new account" /></legend>
					<div class="row">
						<div class="field required">
							<asp:Label runat="server" AssociatedControlID="FirstName"><crm:Snippet runat="server" SnippetName="Register/FirstName" DefaultText="First Name" /></asp:Label>
							<asp:TextBox ID="FirstName" CssClass="text" runat="server" />
							<asp:RequiredFieldValidator ID="FirstNameFieldValidator" runat="server" 
								ControlToValidate="FirstName"
								ErrorMessage="This field is required" 
								Display="Dynamic" 
								CssClass="validation-message"
								ValidationGroup="OpenRegistration"/>
						</div>
					</div>
					<div class="row">
						<div class="field required">
							<asp:Label runat="server" AssociatedControlID="LastName"><crm:Snippet runat="server" SnippetName="Register/LastName" DefaultText="Last Name" /></asp:Label>
							<asp:TextBox ID="LastName" CssClass="text" runat="server" />
							<asp:RequiredFieldValidator ID="LastNameFieldValidator" runat="server" 
								ControlToValidate="LastName"
								ErrorMessage="This field is required" 
								Display="Dynamic" 
								CssClass="validation-message"
								ValidationGroup="OpenRegistration"/>
						</div>
					</div>
					<div class="row">
						<div class="field required">
							<asp:Label runat="server" AssociatedControlID="Email"><crm:Snippet runat="server" SnippetName="Register/Email" DefaultText="Email" /></asp:Label>
							<asp:TextBox ID="Email" CssClass="text" runat="server" />
							<asp:RequiredFieldValidator ID="EmailFieldValidator" runat="server" 
								ControlToValidate="Email"
								ErrorMessage="This field is required" 
								Display="Dynamic" 
								CssClass="validation-message"
								ValidationGroup="OpenRegistration"/>
						</div>
					</div>
					<div class="row">
						<asp:button ID="SubmitOpenRegistrationButton" runat="server" ValidationGroup="OpenRegistration" CssClass="button" Text="Sign-up" onclick="SubmitOpenRegistrationButton_Click" />
					</div>
				</fieldset>
			</asp:PlaceHolder>
		</asp:panel>
	</div>
</asp:Content>
