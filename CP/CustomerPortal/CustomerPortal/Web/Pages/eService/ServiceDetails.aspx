<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ServiceDetails.aspx.cs" Inherits="Site.Pages.eService.ServiceDetails" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentBottom">
	<div>
		<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ServiceDetails/Service" DefaultText="Service: " />
		<asp:Label runat="server" ID="serviceType" />
	</div>
	<div>
		<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ServiceDetails/StartTime" DefaultText="Start Time: " />
		<asp:Label runat="server" ID="startTime" />
	</div>
	<div>
		<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ServiceDetails/EndTime" DefaultText="End Time: " />
		<asp:Label runat="server" ID="endTime" />
	</div>
	<div class="row">
		<asp:HyperLink ID="ExportLink" CssClass="vevent" Text='<%$ Snippet: Service/ExportLink, Add Service to Calendar %>' runat="server" />
	</div>
	<div class="row">
		<crm:CrmHyperLink runat="server" SiteMarkerName="View Scheduled Services">
			<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ServiceDetails/ServicesLink" DefaultText="View All Scheduled Services" />
		</crm:CrmHyperLink>
	</div>
</asp:Content>
