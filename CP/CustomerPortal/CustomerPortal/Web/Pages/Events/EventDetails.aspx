<%@ Page Language="C#" AutoEventWireup="True" MasterPageFile="~/MasterPages/Default.master" ValidateRequest="false" CodeBehind="EventDetails.aspx.cs" Inherits="Site.Pages.Events.EventDetails" %>
<%@ Import Namespace="Site.Library" %>
<%@ Import Namespace="Xrm" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentBottom">
	<div>
		<asp:ListView runat="server" ID="Details">
			<LayoutTemplate>
				<div class="event-details"><asp:PlaceHolder runat="server" ID="ItemPlaceholder"/></div>
				<asp:Button runat="server" ID="RegisterButton" CssClass="register-button" OnClick="RegisterButton_Click" />
				<asp:Label runat="server" ID="Message" />
			</LayoutTemplate>
			<ItemTemplate>
				<crm:CrmEntityDataSource ID="Event" DataItem='<%# Container.DataItem %>' runat="server" />
				<h3>
					<crm:Property DataSourceID="Event" PropertyName="msa_eventname,Name" Literal="true" HtmlEncode="true" runat="server" /> (<%# GetCampaignStatusLabel(Container.DataItem) %>)
				</h3>
				<div class="event-topic" visible='<%# Eval("msa_eventtopic") != null %>' runat="server">Topic: <%# ContentUtility.HtmlEncode(Eval("msa_eventtopic")) %></div>
				<div class="event-datetime"><%# FormatEventDateRange(Container.DataItem as Campaign) %></div>
				<div class="event-location"><%# Eval("msa_city") %>, <%# Eval("msa_stateprovince") %>, <%# Eval("msa_countryregion") %></div>
				<div class="event-cost"><%# string.Format("Cost: ${0}", Eval("msa_costofadmission") ?? 0) %></div>
				<div class="event-address">
					Event Address:<br />
					<%# ContentUtility.HtmlEncode(Eval("msa_street1")) %><br />
					<%# ContentUtility.FormatSequence(", ", Eval("msa_city"), Eval("msa_stateprovince"), Eval("msa_zippostalcode")) %>
					<asp:HyperLink NavigateUrl='<%# Eval("msa_mappingurl") %>' Text='<%# Eval("msa_mappingurl") == null ? string.Empty : "(Map)" %>' runat="server" />
				</div>
				<div class="event-description"><%# ContentUtility.HtmlEncode(Eval("msa_eventdetails")) %></div>
				<div class="event-brochure" visible='<%# Eval("msa_eventbrochureurl") != null %>' runat="server">
					<asp:HyperLink Text="Event Brochure" NavigateUrl='<%# Eval("msa_eventbrochureurl") %>' runat="server" />
				</div>
				<div class="event-contact"><%# ContactInfo() %></div>
			</ItemTemplate>
		</asp:ListView>
	</div>
</asp:Content>

<asp:Content ContentPlaceHolderID="SidebarBottom" runat="server">
	<events:EventCalendar runat="server" />
</asp:Content>
