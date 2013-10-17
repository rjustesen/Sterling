<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ModalDialog.ascx.cs" Inherits="ModalDialog" %>

<asp:Panel ID="pnlModalMask" runat="server" Visible="False" CssClass="modalBackground"
    style="position: fixed; 
        left: 0px; 
        top: 0px; 
        background-color: Gray; 
        z-index: 5000;
        filter:alpha(opacity=70); 
        opacity: 0.7;">
</asp:Panel>
<asp:Panel ID="pnlModelMain" runat="server" Visible="False"
    style="z-index: 5001;
        position: fixed; 
        background-color: White; 
        border: 3px solid black; 
        padding: 10px;">
</asp:Panel>