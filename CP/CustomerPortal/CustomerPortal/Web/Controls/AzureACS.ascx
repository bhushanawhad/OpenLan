<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AzureACS.ascx.cs" Inherits="Site.Controls.AzureAcs" %>
<div id="acs" class="acs-login">
	<div id="SignInContent" class="SignInContent">
		<div id="LeftArea" class="LeftArea" style="display:none;">
			<div class="Header"><crm:Snippet runat="server" SnippetName="ACS Login Title Text" DefaultText="Sign in using your account on:" Editable="true" EditType="text"></crm:Snippet></div>
			<div id="IdentityProvidersList"></div><br />
			<div id="MoreOptions" style="display:none;"><a href="" onclick="ShowDefaultSigninPage(); return false;"><crm:Snippet runat="server" SnippetName="ACS Login Show More Link Text" DefaultText="Show more options" Editable="true" EditType="text"></crm:Snippet></a></div>
		</div>
	</div>
</div>