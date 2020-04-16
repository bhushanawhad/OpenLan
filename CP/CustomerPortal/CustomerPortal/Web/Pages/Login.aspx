<%@ Page Language="C#" MasterPageFile="~/MasterPages/Default.master" AutoEventWireup="True" CodeBehind="Login.aspx.cs" Inherits="Site.Pages.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentBottom" runat="server">
        <asp:Login
            ID="Login1" runat="server" onauthenticate="Login1_Authenticate">
        </asp:Login>
</asp:Content>