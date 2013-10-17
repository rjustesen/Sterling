using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;
using Spring.Context;

public partial class _Default : BasePage
{

    private SortDirection SortDirection
    {
        get { return (SortDirection)ViewState["_Direction_"]; }
        set { ViewState["_Direction_"] = value; }
    }

    private string SortColumn
    {
        get { return (string)ViewState["_SortColumn_"]; }
        set { ViewState["_SortColumn_"] = value; }
    }


    public ScoreService ScoreService;

    public Judge Judge
    {
        get
        {
            return Session["CurrentJudge"] as Judge;
        }
        set { Session["CurrentJudge"] = value; }
    }
  
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (null == CurrentUser || CurrentUser.Role != RoleType.OverallJudge)
            {
                MasterPage.ShowErrorMessage("You are not Authorized to view this page");
                Response.Redirect("~/Default.aspx"); 
            }
            else
            {
                Judge = UserService.GetJudge(CurrentUser);
                lblInfo.Text = CurrentUser.FullName;
                SortDirection = SortDirection.Ascending;
                SortColumn = "Ranking";
                LoadNominees();
            }

        }
    }

    /// <summary>
    /// Load all nominees
    /// </summary>
    private void LoadNominees()
    {
        try
        {
            string where = " where p.Status = " + (int)Status.FinalScore + " and p.Ranking <= 4";
            string orderby = (SortDirection == SortDirection.Ascending) ? " order by " + SortColumn + " ASC " : " order by " + SortColumn + " DESC ";
            IList<Portfolio> list = PortfolioService.GetPortfolios(where + orderby);
            gvNominees.DataSource = list; 
            gvNominees.DataBind();
        }
        catch (Exception e)
        {
            log.Error(e.Message);
        }
    }

    protected void gvNominees_Sorting(object sender, GridViewSortEventArgs e)
    {
        SortDirection =
                (SortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        SortColumn = e.SortExpression;
        LoadNominees();
    }

    protected void gvNominees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                DataControlFieldHeaderCell obj = (DataControlFieldHeaderCell)e.Row.Cells[i];
                if (!String.IsNullOrEmpty(this.SortColumn) && obj.ContainingField.SortExpression == this.SortColumn)
                {
                    GridHelper.AddSortImage(obj, SortDirection);
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Portfolio app = e.Row.DataItem as Portfolio;
            Label labStatus = e.Row.FindControl("labStatus") as Label;
            if (null != labStatus)
            {
                labStatus.Text = app.Status.ToDescription();
            }
        }
        
    }
}
