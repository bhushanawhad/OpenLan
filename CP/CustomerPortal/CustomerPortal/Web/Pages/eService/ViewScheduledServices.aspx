<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ViewScheduledServices.aspx.cs" Inherits="Site.Pages.eService.ViewScheduledServices" %>

<asp:Content runat="server" ContentPlaceHolderID="ContentBottom">
	<asp:GridView runat="server" ID="BookedAppointments"
		AutoGenerateColumns="false"
		GridLines="None" 
		HeaderStyle-BackColor="#ADADAD"
		HeaderStyle-ForeColor="White"
		HeaderStyle-Height="30px"
		RowStyle-Height="30px"
		Width="700px" 
		OnRowCommand="BookedAppointments_OnRowCommand" 
		RowStyle-HorizontalAlign="Center">
		<AlternatingRowStyle BackColor="#E8E8E8" />
		<Columns>
			<asp:TemplateField HeaderText="Scheduled Start">
				<ItemTemplate>
					<%# DateTime.Parse(Eval("scheduledstart").ToString()).ToString("ddd, MMM d, yyyy <br /> h:mm tt")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Scheduled End">
				<ItemTemplate>
					<%# DateTime.Parse(Eval("scheduledend").ToString()).ToString("ddd, MMM d, yyyy <br /> h:mm tt")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Service Type">
				<ItemTemplate>
					<%# Eval("servicetype")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Date Booked">
				<ItemTemplate>
					<%# DateTime.Parse(Eval("dateBooked").ToString()).ToString("ddd, MMM d, yyyy <br /> h:mm tt")%>
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="Cancel Service">
				<ItemTemplate>
					<asp:Button runat="server" CommandName="Cancel" CommandArgument='<%# Eval("serviceId") %>' Text="Cancel" />
				</ItemTemplate>
			</asp:TemplateField>
			<asp:TemplateField HeaderText="">
				<ItemTemplate>
					<asp:HyperLink CssClass="vevent icon" NavigateUrl='<%# GetEventExportUrl(Eval("serviceId")) %>' ToolTip='<%$ Snippet: Service/ExportLink, Add Service to Calendar %>' runat="server" />
				</ItemTemplate>
			</asp:TemplateField>
		</Columns>
	</asp:GridView>	
	<crm:CrmHyperLink runat="server" SiteMarkerName="BookService">
		<crm:Snippet runat="server" SnippetName="Services/ScheduleService/NewService" Editable="false" />
	</crm:CrmHyperLink>
</asp:Content>
