<%@ Page Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="true" CodeBehind="KbSearch.aspx.cs" Inherits="Site.Pages.eService.KbSearch" %>
<%@ Import Namespace="Site.Library" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server" >
	<div id="kb-search">
		<asp:TextBox ID="SearchText" runat="server" OnTextChanged="SearchButton_Click" CssClass="kb-search-input" />
		<asp:Button ID="SearchButton" runat="server" Text="<%$ Snippet: KnowledgeBase/Search %>" OnClick="SearchButton_Click" /><br />
		<asp:Label runat="server"><crm:Snippet runat="server" SnippetName="KnowledgeBase/Instructions" DefaultText="Keywords are separated by ','" /></asp:Label>
	</div>

	<div id="kb-results">
		<asp:ListView ID="ResultsList" runat="server" Visible="false">
			<LayoutTemplate>
				<h3><crm:Snippet runat="server" SnippetName="KnowledgeBase/SearchResults" DefaultText="Search Results" /></h3>
				<ul class="link-list"><asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder></ul>
			</LayoutTemplate>
			<ItemTemplate>
				<li>
					<div class="heading"><asp:HyperLink NavigateUrl='<%# GetKbArticleUrl(Eval("kbarticleid")) %>' Text='<%# ContentUtility.HtmlEncode(Eval("Title")) %>' runat="server" /></div>
					<div class="summary"><crm:Snippet runat="server" SnippetName="knowledgeBase/subject" DefaultText="Subject: " /><%# ContentUtility.HtmlEncode(Eval("subject_kb_articles.title")) %></div>
					<div class="summary"><crm:Snippet runat="server" SnippetName="knowledgeBase/keywords" DefaultText="Keywords: " /><%# ContentUtility.HtmlEncode(Eval("keywords")) %></div>
				</li>
			</ItemTemplate>
		</asp:ListView>
		
		<asp:PlaceHolder ID="NoResults" runat="server" Visible="false">
			<h3><crm:Snippet runat="server" SnippetName="KnowledgeBase/SearchResults" DefaultText="Search Results" /></h3>
			<p><crm:Snippet runat="server" SnippetName="KnowledgeBase/NoResults" DefaultText="No articles found." /></p>
		</asp:PlaceHolder>	
	</div>
</asp:Content>