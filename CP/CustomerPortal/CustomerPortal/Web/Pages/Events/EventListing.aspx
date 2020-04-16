<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Default.master" ValidateRequest="false" CodeBehind="EventListing.aspx.cs" Inherits="Site.Pages.Events.EventListing" %>
<%@ Import Namespace="Site.Library" %>
<%@ Import Namespace="Xrm" %>

<asp:Content ContentPlaceHolderID="ContentBottom" runat="server">
	<div class="Event-List-Menu">
		<asp:Menu runat="server" ID="EventListFilter" 
			StaticMenuItemStyle-CssClass="menu-item" 
			StaticSelectedStyle-CssClass="selected"
			StaticMenuItemStyle-HorizontalPadding="12px"
			StaticMenuItemStyle-VerticalPadding="4px"
			Orientation="Horizontal" 
			OnMenuItemClick="Menu_Click">
			<Items>
				<asp:MenuItem Text="Featured Events" Value="Featured" Selected="true" />
				<asp:MenuItem Text="Recently Added" Value="RecentlyAdded" />
				<asp:MenuItem Text="Upcoming Events" Value="Upcoming" />
				<asp:MenuItem Text="Past Events" Value="Past" />
			</Items>
		</asp:Menu>
	</div>
	<div class="Event-List">
		<asp:Repeater runat="server" ID="EventsRepeater">
			<ItemTemplate>
				<crm:CrmEntityDataSource ID="Event" DataItem='<%# Container.DataItem %>' runat="server" />
				<h3><crm:CrmHyperLink runat="server" SiteMarkerName="Event Details" QueryString='<%# string.Format("campaignID={0}", Eval("campaignid")) %>'><crm:Property DataSourceID="Event" PropertyName="msa_eventname,Name" Literal="true" HtmlEncode="true" runat="server" /> (<%# GetCampaignStatusLabel(Container.DataItem) %>)</crm:CrmHyperLink></h3>
				<div class="date-line"><%# FormatEventDate(Container.DataItem as Campaign) %></div>
				<div class="location-line"><%# ContentUtility.FormatSequence(", ", Eval("msa_city"), Eval("msa_stateprovince"), Eval("msa_countryregion")) %></div>
				<div class="details"><%# ContentUtility.HtmlEncode(Eval("msa_eventdetails")) %></div>
			</ItemTemplate>
		</asp:Repeater>
	</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="SidebarBottom" runat="server">
	<events:EventCalendar runat="server" />
</asp:Content>
