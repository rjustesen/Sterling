<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StudentEdit.aspx.cs" Inherits="Judge_StudentEdit" %>
<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
    <asp:Panel ID="pnlAdminMenu" runat="server" >
    <div id="menu">
        <ul>
	        <li  class="first active"><asp:HyperLink ID="HyperLink1" runat="server" Text="Home" NavigateUrl="~/Default.aspx"></asp:HyperLink></li>
	        <li><asp:HyperLink ID="HyperLink2" runat="server" Text="Account" NavigateUrl="~/Profile.aspx"></asp:HyperLink></li>
        </ul>
        <br class="clearfix" />
    </div>
	</asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <script src="../assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    //<!--
    $(function() {
        $("input[type=submit], button").button();
    });
    
    function ComputeScore(element, id) {
        var val = document.getElementById(element).value;
        ScoreWebService.ComputeScore(val, id, onSuccess, onFailure);
    }

    function onSuccess(result) {
        var arr = $("span[id$='lblTotal']").each(function() {
            $(this).html(result);
        }).get();
    }

    function onFailure(result) {
    }
    //-->
  </script>    

    <table width="40%">
        <tr>
            <td><asp:HyperLink runat="server" ID="lnkTest" ImageUrl="~/assets/images/ico-back.png" Text="Goback" NavigateUrl="~/Judge/Default.aspx" ></asp:HyperLink>
            <asp:LinkButton runat="server" ID="lnkBack" Text="Save and Goback" OnClick="lnkBack_Click" ></asp:LinkButton></td>
        </tr>
    </table>
    
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pnlInfo" runat="server">
            <table class="grid">
                <tr>
                    <th>Student</th>
                    <th>School</th>
                    <th>Category</th>
                    <th>Application Status</th>
                    <th>Judging Phase</th>
                    <th>Rank</th>
                </tr>
                <tr>
                  <td><asp:Label Runat="server" id="lblName" ></asp:Label></td>
                  <td><asp:Label Runat="server" id="lblSchool" ></asp:Label></td>
                  <td><asp:Label Runat="server" id="lblCategory" ></asp:Label></td>
                  <td><asp:Label Runat="server" id="lblStatus" ></asp:Label></td>
                  <td><asp:Label Runat="server" id="lblPhase" ></asp:Label></td>
                  <td><asp:Label Runat="server" id="lblRank" ></asp:Label></td>
                </tr>
            </table>
         </asp:Panel>
        <asp:GridView runat="server" ID="gvScores"  
            AutoGenerateColumns="false"  
            CssClass="grid"
            ShowFooter="true"
            DataKeyNames="Id"
            OnRowDataBound="gvScores_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate><asp:Label runat="server" ID="lblId" Text='<%#Eval("Id") %>' Visible="false"></asp:Label></ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Score Category">
                <ItemTemplate><asp:Label runat="server" ID="lblName"></asp:Label></ItemTemplate>
                <FooterTemplate>
                   <asp:Label runat="server" ID="lblTest" Text="Total Average Score"></asp:Label>
                </FooterTemplate>     
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Score">
                <ItemTemplate>
                    <asp:TextBox ID="txtScore" runat="server" Text='<%#Eval("Value") %>' Width="50px" ></asp:TextBox>
                    <asp:RangeValidator ID="rangeValidator1" runat="server" ControlToValidate="txtScore" Type="Double"
                                    Display="Dynamic"
                                    ValidationGroup="ScoreEdit">*</asp:RangeValidator>
                    <asp:RegularExpressionValidator ID="regValidator1" ControlToValidate="txtScore" Display="Dynamic" 
                                        ErrorMessage="Please Enter Numeric Value" 
                                        ValidationExpression="(\d+\.\d+)|(\d+\.)|(\.\d+)|(\d+)" Runat="server"></asp:RegularExpressionValidator></td>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label runat="server" ID="lblTotal" ></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="">
                <ItemTemplate><asp:Label runat="server" ID="lblRange"></asp:Label></ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:panel runat="server" ID="pnlLink">
        <table width="40%">
            <tr>
            <td><asp:LinkButton runat="server" ID="btnShowPortfolio" Text="Show Portfolio" OnClick="btnShowPortfolio_OnClick" ></asp:LinkButton></td>
            <td><asp:HyperLink runat="server" ID="lnkPortfolio" Text="Open Portfolio in a new Window" Target="_blank"></asp:HyperLink></td>
            </tr>
        </table>
    </asp:panel>   
    <asp:Panel ID="pnlReport" runat="server" >
        <cc1:ShowPdf ID="ShowPdf1" runat="server" BorderStyle="Inset" BorderWidth="2px" FilePath="fw9.pdf"
            Height="450px" Style="z-index: 103; position: relative; " Width="900px" Visible="false" />
    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

