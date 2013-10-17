<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PopupMessageBox.ascx.cs" Inherits="PopupMessageBox" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolKit" %>

<ajaxToolKit:ModalPopupExtender ID="mpext" runat="server" BackgroundCssClass="popupBackground"
    TargetControlID="pnlPopup" PopupControlID="pnlPopup">
</ajaxToolKit:ModalPopupExtender>
<asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Style="display: none;"
    DefaultButton="btnOk">
    <table width="100%">
        <tr class="topHandle">
            <td colspan="2" align="left" runat="server" id="tdCaption">
                &nbsp; <asp:Label ID="lblCaption" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width: 60px" valign="middle" align="center">
                <asp:Image ID="imgInfo" runat="server" ImageUrl="~/assets/images/Info-48x48.png" />
            </td>
            <td valign="middle" align="left">
                <asp:Label ID="lblMessage" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right">
                <asp:Button ID="btnOk" runat="server" Text="Ok" OnClick="btnOk_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>

<script type="text/javascript">
        function fnClickOK(sender, e)
        {
            __doPostBack(sender,e);
        }
</script>


