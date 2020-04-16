<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/Default.master" CodeBehind="ChangePassword.aspx.cs" Inherits="Site.Pages.ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentBottom" runat="server">
    <%--    <asp:ChangePassword
            ID="ChangePassword1" runat="server" OnChangedPassword="ChangePassword1_ChangedPassword" OnChangingPassword="ChangePassword1_ChangingPassword">
        </asp:ChangePassword>--%>
    <asp:Panel ID="ChangePasswordPanel" runat="server" >
        <asp:Table ID="table1" runat="server">
            <asp:TableRow>
                <asp:TableCell><asp:Label ID="Label1" Text="UserName:" runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:TextBox ID="userNameTextBox" runat="server" ReadOnly="True"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell><asp:Label ID="Label2" Text="New Password:" runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:TextBox ID="passwordTextBox" runat="server" TextMode="Password"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell><asp:Label ID="Label3" Text="Confirm Password:" runat="server"></asp:Label></asp:TableCell>
                <asp:TableCell><asp:TextBox ID="confirmPasswordTextBox" runat="server" TextMode="Password"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
             <asp:TableRow>
                <asp:TableCell><asp:Button ID="SubmitButton" Text="Submit" runat="server" OnClick="SubmitButton_Click"></asp:Button></asp:TableCell>
            </asp:TableRow>
        </asp:Table>

    </asp:Panel>
    <asp:CompareValidator id="CompareValidator1" 
             runat="server" ErrorMessage="Passwords do not match!" 
             ControlToValidate="confirmPasswordTextBox" 
             ControlToCompare="passwordTextBox"></asp:CompareValidator>
    <asp:Label ID="Message1" Runat="server" ForeColor="Red" /><br />
</asp:Content>