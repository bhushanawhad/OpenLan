<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCase.aspx.cs" Inherits="Site.Pages.eService.CreateCase" MasterPageFile="~/MasterPages/Default.master" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">

	<crm:CrmMetadataDataSource ID="PrioritySource" runat="server" AttributeName="prioritycode" EntityName="incident"/>
	<div style="display:none"><asp:DropDownList ID="PriorityCode" runat="server" DataSourceID="PrioritySource" DataTextField="OptionLabel" DataValueField="OptionValue"></asp:DropDownList></div>
		
	<div id="service-cases">

		<fieldset>
		
			<div>
				<asp:ValidationSummary ValidationGroup="CreateCase" runat="server" />
			</div>
				
			<ul>
				<li>
					<asp:Label AssociatedControlID="TitleTextBox" runat="server"><crm:Snippet runat="server" SnippetName="cases/createCase/title" DefaultText="Title" />
						<asp:RequiredFieldValidator ControlToValidate="TitleTextBox" ErrorMessage="You must provide a title for your new case" ValidationGroup="CreateCase" runat="server">*</asp:RequiredFieldValidator>
					</asp:Label>
					<asp:TextBox ID="TitleTextBox" MaxLength="100" runat="server" />
				</li>
					
				<li>
					<crm:CrmMetadataDataSource ID="CaseTypeSource" runat="server"
							AttributeName="casetypecode"
							EntityName="incident"
							SortExpression="Value=DESC" />
								
					<asp:Label AssociatedControlID="CaseType" runat="server"><crm:Snippet runat="server" SnippetName="cases/createCase/caseType" DefaultText="Case Type" /></asp:Label>
					<asp:DropDownList ID="CaseType" runat="server"
						DataSourceID="CaseTypeSource"
						DataTextField="OptionLabel"
						DataValueField="OptionValue" />
				</li>
					
				<li>
					<asp:Label AssociatedControlID="Description" runat="server"><crm:Snippet runat="server" SnippetName="cases/createCase/description" DefaultText="Description" /></asp:Label>
					<asp:TextBox ID="Description" runat="server" TextMode="MultiLine" />
				</li>
					
				<li id="attachment">
					<asp:Label AssociatedControlID="Attachment" runat="server"><crm:Snippet runat="server" SnippetName="cases/createCase/attachement" DefaultText="Attachment" /></asp:Label>
					<asp:FileUpload ID="Attachment" size="42" runat="server" />
				</li>
			</ul>
				
			<div>
				<asp:Button ID="CreateButton" runat="server" CssClass="PostButton" Text="<%$ Snippet: Cases/CreateCase %>" OnClick="CreateButton_Click" ValidationGroup="CreateCase" />
			</div>
				
		</fieldset>
			
		<crm:CrmHyperLink SiteMarkerName="Cases" runat="server"><crm:Snippet runat="server" SnippetName="cases/createCase/backText" DefaultText="Go Back to All Cases" /></crm:CrmHyperLink>

	</div>

</asp:Content>
