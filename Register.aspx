<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
<asp:Panel id="pnlUserEdit" runat="server" Visible="false">
        <fieldset>
        <legend>Register User Profile</legend>
            <table>
                <tr>
                    <td>User Name</td>
                    <td><asp:textbox id="txtUserName" Runat="server"></asp:textbox></td>
                    <td>Full Name</td>
                    <td><asp:textbox id="txtFullName" Runat="server" Width="250px" MaxLength="50"></asp:textbox></td>
                </tr>
                <tr>
                    <td>Email</td>
                    <td><asp:textbox id="txtEmail" Runat="server" Width="250px"></asp:textbox></td>
                </tr>
               
                <tr>
                 <td>Comment</td>
                 <td colspan="3"><asp:textbox id="txtComment" Runat="server" TextMode="MultiLine" Width="250px" Wrap="true"></asp:textbox></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Panel runat="server" ID="pnlJudge" Visible="true">
                        <table>
                         <tr>
                            <td><asp:Label runat="server" ID="labRegion" Text="Region" AssociatedControlID="ddlRegion"></asp:Label></td>             
                            <td><asp:dropdownlist id="ddlRegion" Runat="server">
                            </asp:dropdownlist></td>
                        </tr>
                        <tr>
                            <td>Area</td>             
                            <td><asp:dropdownlist id="ddlArea" Runat="server">
                            </asp:dropdownlist></td>
                        </tr>
                       
                        <tr>
                            <td>Category</td>             
                            <td><asp:dropdownlist id="ddlCategory" Runat="server">
                            </asp:dropdownlist></td>
                        </tr>
                        </table>
                        </asp:Panel>
                     </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td><asp:button id="btnSave" runat="server" Text="Save" ></asp:button></td>
                    <td><asp:button id="btnCancel" runat="server" Text="Cancel" ></asp:button></td>
                </tr>
            </table>
        </fieldset>     
       </asp:Panel>     
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

