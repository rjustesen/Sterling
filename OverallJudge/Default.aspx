<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" MasterPageFile="~/MasterPage.master" Inherits="_Default" %>
<%@ Register Src="~/CustomControls/ModalDialog.ascx" TagName="ModalDialog" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <link href="../assets/css/grid.css" rel="stylesheet" type="text/css" media="screen" />
  <link href="../assets/css/accordion.css" rel="stylesheet" type="text/css" />
   <style type="text/css">
    img {
        float: left;
        border: none;
    }
    .footer
    {
        background-color: #CCCCCC;
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="menuPlaceHolder" Runat="Server">
    <asp:Panel ID="pnlMenu" runat="server" >
    <div id="menu">
    	<ul>
		    <li class="first active"><asp:HyperLink runat="server" Text="Home" NavigateUrl="~/Default.aspx"></asp:HyperLink></li>
		    <li><asp:HyperLink ID="HyperLink2" runat="server" Text="Account" NavigateUrl="~/Profile.aspx"></asp:HyperLink></li>
	    </ul>
      <br class="clearfix" />
    </div>
	</asp:Panel>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" Runat="Server">
    <script src="../assets/javascript/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="../assets/javascript/jquery-ui-1.10.2.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">
    
    $(function() {
        $("input[type=submit], button").button();
    });
    </script>
    <asp:Panel ID="pnlInfo" runat="server" ScrollBars="Auto">
        <table>
         <tr>
            <td class="headerText"><asp:Label runat="server" ID="lblInfo" ></asp:Label></td>
         </tr>
        </table>
    <br />
    </asp:Panel>
   <asp:Panel ID="pnlUserList" runat="server" >
         <asp:GridView ID="gvNominees" runat="server" 
           AllowSorting="true"
            AutoGenerateColumns="False" 
            CssClass="grid"
            AlternatingRowStyle-CssClass="alt"
            HorizontalAlign="Center" 
            DataKeyNames="Id" 
            onrowdatabound="gvNominees_RowDataBound" 
            OnSorting="gvNominees_Sorting" >
            <Columns>
              <asp:TemplateField HeaderText="Name" SortExpression="User.FullName">
                    <ItemTemplate><asp:Label runat="server" ID="labName" Text='<%#Eval("User.FullName") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="School" SortExpression="School.Name">
                    <ItemTemplate><asp:Label runat="server" ID="labSchool" Text='<%#Eval("School.Name") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Category" SortExpression="Category.Name">
                    <ItemTemplate><asp:Label runat="server" ID="labCategory" Text='<%#Eval("Category.Name") %>'  ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" SortExpression="Status">
                    <ItemTemplate><asp:Label runat="server" ID="labStatus" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Overall Average Score" SortExpression="TotalScore">
                    <ItemTemplate><asp:Label runat="server" ID="labTotalScore" Text='<%#Eval("TotalScore") %>'></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ranking" SortExpression="Ranking">
                    <ItemTemplate><asp:Label runat="server" ID="labRanking" Text='<%#Eval("Ranking") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:HyperLink runat="server" ID="lblView" Target="_blank" Text="View Portfolio" NavigateUrl='<%# "~/ReportView.aspx?report=profile&id=" + Eval("Id").ToString() %>'></asp:HyperLink></ItemTemplate>
                </asp:TemplateField>
           </Columns>
             <EmptyDataTemplate>
                 No Nominees in the current judging status were found
             </EmptyDataTemplate>
             <AlternatingRowStyle CssClass="alt" />
          </asp:GridView>
        </asp:Panel>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

