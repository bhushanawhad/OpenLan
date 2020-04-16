<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditCase.aspx.cs" Inherits="Site.Pages.eService.EditCase" MasterPageFile="~/MasterPages/Default.master" %>
<%@ Import Namespace="Microsoft.Xrm.Portal.Core" %>
<%@ Import Namespace="Microsoft.Xrm.Sdk" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	
	<div id="service-cases">
		<h3><asp:Label runat="server" ID="actionResultMessage" /></h3>
		<asp:Panel runat="server" ID = "CaseInfoPanel" >
			<h3>
				<asp:Label ID="TitleLabel" runat="server" /><br />
				<asp:Label ID="CaseNumberLabel" runat="server" />
			</h3>
			<p>Status: <asp:Label ID="StatusReasonLabel" runat="server" /> <asp:Label ID="SubjectLabel" runat="server" /> <asp:Label ID="CaseTypeLabel" runat="server" /></p>
			<p>
                Description
                <asp:TextBox ID="descriptionTextBox" runat="server" TextMode="MultiLine" ReadOnly="True" Width="100%" Height="100px"></asp:TextBox>
			</p>
			<div>
				<asp:ValidationSummary ValidationGroup="EditCase" runat="server" />
			</div>
			
			<asp:Label ID="KB" runat="server" />
		</asp:Panel>
		
		<asp:Panel ID="EditPanel" runat="server">
			<fieldset>
				<asp:PlaceHolder ID="UpdateFields" runat="server">
					<ul>
						<li>
							<asp:Label AssociatedControlID="NewNote" runat="server"><crm:Snippet runat="server" SnippetName="cases/editCase/note" DefaultText="Add a New Note:" /></asp:Label>
							<asp:TextBox ID="NewNote" runat="server" TextMode="MultiLine" />
						</li>
					</ul>
					
					<ul>
						<li>
							<asp:Label AssociatedControlID="Attachment" runat="server"><crm:Snippet runat="server" SnippetName="cases/editCase/attachment" DefaultText="Add a New Attachment:" /></asp:Label>
							<asp:FileUpload ID="Attachment" runat="server" />
						</li>
					</ul>
				</asp:PlaceHolder>
				<div>
					<asp:Button ID="UpdateButton" runat="server" CssClass="PostButton" Text="<%$ Snippet: Cases/UpdateCase, Update Case %>" OnClick="UpdateButton_Click" ValidationGroup="EditCase" />
					<asp:Button ID="CancelCaseButton" runat="server" CssClass="PostButton" Text="<%$ Snippet: Cases/CancelCase, Cancel Case %>" OnClick="CancelCaseButton_Click" />
				</div>
			</fieldset>
		</asp:Panel>

        <asp:Panel ID="ActivitiesPanel" runat="server">
            <asp:Label Font-Bold="true" runat="server">Activites </asp:Label>
        <asp:GridView ID="ActivityList" runat="server" CssClass="cases" OnRowDataBound="ActivityList_OnRowDataBound" AutoGenerateColumns ="false" >
        <Columns>
            <asp:BoundField DataField="createdon" HeaderText="Created On" />
            <asp:BoundField DataField="subject" HeaderText="Subject" />
            <asp:BoundField DataField="description" HeaderText="Description" />
            <asp:BoundField DataField="activitytypecode" HeaderText="Type" />
            <asp:BoundField DataField="actualdurationminutes" HeaderText="Actual Duration Minutes" />
            <asp:BoundField DataField="statecode" HeaderText="Status" />
        </Columns>
		</asp:GridView>
        </asp:Panel>
			
		<asp:Panel ID="CaseInfoPanelNotes" runat="server">
			<asp:LinqDataSource ID="CrmNoteSource" runat="server" ContextTypeName="Xrm.XrmServiceContext" TableName="AnnotationSet"
				Where="ObjectId.Id == @CaseID && NoteText.Contains(@Filter)" OrderBy="CreatedOn DESC" OnSelecting="LinqDataSourceSelecting">
				<WhereParameters>
					<asp:QueryStringParameter Name="CaseID" QueryStringField="CaseID" DefaultValue="00000000-0000-0000-0000-000000000000" DbType="Guid" />
				</WhereParameters>
			</asp:LinqDataSource>
				
			<asp:ListView ID="NotesList" runat="server" DataSourceID="CrmNoteSource">
				<LayoutTemplate>
					<ul id="case-notes"><asp:PlaceHolder ID="ItemPlaceHolder" runat="server"></asp:PlaceHolder></ul>
				</LayoutTemplate>
				<ItemTemplate>
					<li>
						<p class="note-data">Created at <%# DateTime.Parse(Eval("CreatedOn").ToString()).ToLocalTime()%></p>
						<asp:HyperLink CssClass="note-attachment" NavigateUrl='<%# (Container.DataItem as Entity).GetRewriteUrl() %>' Visible='<%# Eval("FileSize") != null %>' runat="server"><%# Eval("FileName") %>&nbsp;(<%# Eval("FileSize") %>)</asp:HyperLink>
						<p class="note-content"><%# FormatNote(Eval("NoteText"))%></p>
					</li>
				</ItemTemplate>
			</asp:ListView>
		</asp:Panel>
		
		<asp:Panel ID="CloseCasePanel" runat="server" Visible="false">
			<fieldset>
				<ul>
					<li>
						<crm:CrmMetadataDataSource ID="SatisfactionSource" runat="server" 
							AttributeName="customersatisfactioncode" 
							EntityName="incident" />
							
						<asp:Label AssociatedControlID="Satisfaction" runat="server"><crm:Snippet runat="server" SnippetName="cases/editCase/staisfaction" DefaultText="Satisfaction" /></asp:Label>
						<asp:DropDownList ID="Satisfaction" runat="server"
							DataSourceID="SatisfactionSource"
							DataTextField="OptionLabel"
							DataValueField="OptionValue" />
					</li>
					<li>
						<asp:Label AssociatedControlID="Resolution" runat="server"><crm:Snippet runat="server" SnippetName="cases/editCase/resolution" DefaultText="Resolution" /></asp:Label>
						<asp:TextBox runat="server" ID="Resolution" />
					</li>
				</ul>
				<div>
					<asp:Button ID="ResolveButton" runat="server" CssClass="PostButton" Text="<%$ Snippet: Cases/CloseCase, Submit %>" OnClick="ResolveButton_Click" />
				</div>
			</fieldset>
		</asp:Panel>

		<asp:Panel ID="ReopenCase" runat="server" Visible="false">
			<div>
				<asp:Button ID="ReopenButton" runat="server" CssClass="PostButton" Text="<%$ Snippet: Cases/ReopenCase, Reopen Case %>" OnClick="ReopenButton_Click" />
			</div>
		</asp:Panel>

		<ul class="case-data">
			<li>Created: <asp:Label ID="CreatedOn" runat="server" /></li>
			<li>Last Modified: <asp:Label ID="LastModifiedOn" runat="server" /></li>
		</ul>
		<crm:CrmHyperLink sitemarkername="Cases" runat="server"><crm:Snippet runat="server" SnippetName="cases/createCase/backText" DefaultText="Go Back to All Cases" /></crm:CrmHyperLink>
	</div>

</asp:Content>
