<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ScheduleService.aspx.cs" Inherits="Site.Pages.eService.ScheduleService" EnableEventValidation="false" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentBottom">
	<asp:Panel runat="server" ID="SearchPanel">
		<div id="SearchSchedule">
			<div class="sectionSpacer">
				<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ScheduleService/ServiceType" DefaultText="Requested Service Type: " />
				<asp:DropDownList runat="server" ID="ServiceType" CssClass="serviceDropdown" />
			</div>
			<div class="sectionSpacer">
				<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ScheduleService/DateRange" DefaultText="Set Date Range: " /><br />
				<div class="leftCalendar">
					<asp:Calendar runat="server" ID="StartDate" />
				</div>
				<div class="rightCalendar">
					<asp:Calendar runat="server" ID="EndDate" />
				</div>
			</div>
			<div class="sectionSpacer" style="clear:both;">
				<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ScheduleService/TimeZone" DefaultText="Select Your Current Time Zone: " /><br />
				<asp:DropDownList runat="server" ID="TimeZoneSelection" />
			</div>
			<div class="sectionSpacer">
				<crm:Snippet runat="server" CssClass="instructionText" SnippetName="Services/ScheduleService/TimeOfDay" DefaultText="Time of Day: " />
				<asp:DropDownList runat="server" ID="StartTime" />
				<asp:DropDownList runat="server" ID="EndTime" />
			</div>
			<div class="sectionSpacer">
				<asp:Button runat="server" OnClick="FindTimes_Click" Text="<%$ Snippet: Services/ScheduleService/FindAvailableTimes %>" />
				<asp:Label runat="server" ID="ErrorLabel" CssClass="errorLabel" Visible="false" />
			</div>
		</div>
	</asp:Panel>
	<asp:Panel ID="NoServicesMessage" runat="server" Visible="false">
		<p><strong>There are no services available to be scheduled.</strong></p>
	</asp:Panel>
	<asp:Panel ID="ResultsDisplay" runat="server" Visible="false">
		<div class="sectionSpacer">
			<asp:Label runat="server" Text="Please select from the following list of available appointment times:" />
		</div>
		<asp:GridView runat="server" ID="AvailableTimes"
			AutoGenerateColumns="false"
			DataKeyNames="AvailableResource, ScheduledStartUniversalTime, ScheduledEndUniversalTime"
			GridLines="None"
			OnRowDataBound="AvailableTimes_RowDataBound"
			OnSelectedIndexChanged="AvailableTimes_SelectedIndexChanged"
			HeaderStyle-BackColor="#ADADAD"
			HeaderStyle-ForeColor="White"
			HeaderStyle-Height="30px"
			RowStyle-Height="30px"
			Width="450px">
			<SelectedRowStyle BackColor="#889BAC" ForeColor="White" />
			<AlternatingRowStyle BackColor="#E8E8E8" />
			<Columns>
				<asp:TemplateField HeaderText="Scheduled Start">
					<ItemTemplate>
						<%# Eval("ScheduledStart") %>
					</ItemTemplate>
				</asp:TemplateField>
				<asp:TemplateField HeaderText="Scheduled End">
					<ItemTemplate>
						<%# Eval("ScheduledEnd") %>
					</ItemTemplate>
				</asp:TemplateField>
			</Columns>
		</asp:GridView>
		<asp:Label runat="server" ID="BookingError" CssClass="bookingError" /><br />
		<asp:Button runat="server" ID="ScheduleServiceButton" Text="<%$ Snippet: Services/ScheduleService/ScheduleService %>" OnClick="ScheduleService_Click" />
	</asp:Panel>
</asp:Content>