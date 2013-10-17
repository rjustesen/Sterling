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
        //<!--
        $(function() {
            $("input[type=submit], button").button();
        });
        //-->
    </script>
    <asp:Panel ID="pnlInfo" runat="server" ScrollBars="Auto">
        <table>
         <tr>
            <td class="headerText"><asp:Label runat="server" ID="lblInfo" ></asp:Label></td>
         </tr>
         <tr>
            <td class="headerText"><asp:Label runat="server" ID="lblCategory" ></asp:Label></td>
         </tr>
         <tr> 
            <td class="headerText"><asp:Label Runat="server" id="labScoreStatus" ></asp:Label></td></tr>
        </table>
        <table width="80%">
            <tr>
            <td><asp:LinkButton runat="server" ID="lnkSubmit" 
                                Text="Submit Scores" 
                                OnClientClick="return confirm('Are you sure you want to submit the scores for all nominees? \n(Scores cannot be edited after submission)');" 
                                onclick="lnkSubmit_Click"></asp:LinkButton></td>
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
            OnSorting="gvNominees_Sorting" onrowcommand="gvNominees_RowCommand">
            <Columns>
              <asp:TemplateField HeaderText="Name" SortExpression="j.Application.User.FullName">
                    <ItemTemplate><asp:Label runat="server" ID="labName" Text='<%#Eval("Application.User.FullName") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="School" SortExpression="j.Application.School.Name">
                    <ItemTemplate><asp:Label runat="server" ID="labSchool" Text='<%#Eval("Application.School.Name") %>' ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" SortExpression="j.Status">
                    <ItemTemplate><asp:Label runat="server" ID="labStatus" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Judge Score" SortExpression="j.Score">
                    <ItemTemplate><asp:Label runat="server" ID="labScore" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Overall Average Score" SortExpression="j.Application.TotalScore">
                    <ItemTemplate><asp:Label runat="server" ID="labTotalScore" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ranking" SortExpression="j.Application.Ranking">
                    <ItemTemplate><asp:Label runat="server" ID="labRanking" ></asp:Label></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:HyperLink runat="server" ID="btnScores" NavigateUrl='<%# "~/Judge/StudentEdit.aspx?id=" + Eval("Id").ToString() %>' Text="Edit Scores"></asp:HyperLink></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate><asp:HyperLink runat="server" ID="lblView" Target="_blank" Text="View Portfolio" NavigateUrl='<%# "~/ReportView.aspx?report=profile&id=" + Eval("Application.Id").ToString() %>'></asp:HyperLink></ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="lnkScores" CommandArgument='<%#Eval("Application.Id") %>' CommandName="ViewScores" Text="View Scores"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
           </Columns>
             <EmptyDataTemplate>
                 No Nominees in the current judging status were found
             </EmptyDataTemplate>
             <AlternatingRowStyle CssClass="alt" />
          </asp:GridView>
        </asp:Panel>
        
         <uc1:ModalDialog ID="modalDialog" runat="server"  Width="500" Height="350" >  
         <Content>
            <span><strong>Scores for <asp:Label runat="server" ID="lblName"></asp:Label></strong></span>
             <asp:ListView ID="lvScores" runat="server" 
                OnItemDataBound="lvScores_ItemDataBound">
                <LayoutTemplate>
                    <table border="0" cellpadding="1" class="grid">
                        <thead>
                        <tr>
                            <th align="left">Judge</th>
                            <th align="left">Status</th>
                            <th align="left">Score</th>
                            <th align="left">Submitted</th>
                            <th></th>
                            <th></th>
                        </tr>
                        </thead>
                        <tbody>
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </tbody>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label runat="server" ID="lblName"></asp:Label></td>
                        <td><asp:Label runat="server" ID="labStatus"></asp:Label></td>
                        <td><asp:Label runat="server" ID="labScore"></asp:Label></td>
                        <td><asp:Label runat="server" ID="labSubmitted"></asp:Label></td>
                    </tr>
                    
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <tr style="background-color:#EFEFEF">
                        <td><asp:Label runat="server" ID="lblName"></asp:Label></td>
                        <td><asp:Label runat="server" ID="labStatus"></asp:Label></td>
                        <td><asp:Label runat="server" ID="labScore"></asp:Label></td>
                        <td><asp:Label runat="server" ID="labSubmitted"></asp:Label></td>
                    </tr>
                </AlternatingItemTemplate>
             </asp:ListView>
            <span><strong><asp:Label runat="server" ID="lblFooter" CssClass="footer"></asp:Label></strong></span>
            <div style="text-align:left;padding:8px;">
                <asp:Button ID="btnClose" runat="server" Text="Close" CausesValidation="false"  onclick="btnClose_Click" />
            </div>                    
        </Content>
    </uc1:ModalDialog>   
    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BottomCenterPlaceHolder" Runat="Server">
</asp:Content>

