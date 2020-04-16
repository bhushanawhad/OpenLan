<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EventCalendar.ascx.cs" Inherits="Site.Controls.EventCalendar" %>
<%@ Import Namespace="Site.Library" %>
<%@ Import Namespace="Xrm" %>

<div ID="eventCalenderContainer">
	<asp:Calendar ID="EventsCalendar" runat="server"
		OnVisibleMonthChanged="MonthChanged"
		OnDayRender="EventsCalendar_DayRender"
		CssClass="events-calendar"
		TitleStyle-CssClass="title-bar"
		OtherMonthDayStyle-CssClass="other-month-day-cell"
		TodayDayStyle-CssClass="today-day-cell"
		WeekendDayStyle-CssClass="weekend-day-cell"
		DayStyle-CssClass="day-cell"
		DayHeaderStyle-CssClass="day-header" />
		
	<asp:Repeater ID="CalendarRepeater" runat="server" >
		<ItemTemplate>
			<div class="event-listing">
				<crm:CrmEntityDataSource ID="CampaignEntity" DataItem="<%# Container.DataItem %>" runat="server" />
				<crm:CrmHyperLink SiteMarkerName="Event Details" runat="server" QueryString='<%# string.Format("campaignID={0}", Eval("campaignid")) %>'> 
					<%# ContentUtility.HtmlEncode(Eval("msa_eventname")) %>
				</crm:CrmHyperLink>
				<div class="date-line"><%# FormatEventDate(Container.DataItem as Campaign) %></div>
			</div>
		</ItemTemplate>
	</asp:Repeater>
</div>
