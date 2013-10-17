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
            if (null == CurrentUser || (CurrentUser.Role != RoleType.AreaJudge && CurrentUser.Role != RoleType.RegionJudge))
            {
                MasterPage.ShowErrorMessage("You are not Authorized to view this page");
                Response.Redirect("~/Default.aspx"); 
            }
            else
            {
                Judge = UserService.GetJudge(CurrentUser);
                if (CurrentUser.Role == RoleType.AreaJudge)
                {
                    lblInfo.Text = Judge.Area.Name + " Area";
                }
                else
                {
                    lblInfo.Text = " Region";
                }
                lblCategory.Text = Judge.Category.Name;
                SortDirection = SortDirection.Ascending;
                SortColumn = "j.Application.Ranking";
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
            DateTime dt = DateTime.Now;
            lnkSubmit.Visible = !ScoreService.IsJudgeSubmited(Judge);
            ScoringPhase phase = ScoreService.GetJudgeScoreStatus(Judge);
            labScoreStatus.Text = "Judging Phase: " + phase.ToDescription();
            IList<JudgeStatus> list = ScoreService.GetJudgeStatusForJudge(Judge, phase, SortColumn, SortDirection == SortDirection.Ascending);
            if (lnkSubmit.Visible && list.Count == 0)
            {
                lnkSubmit.Visible = false;
            }
            //foreach (JudgeStatus js  in list)
            //{
            //    if (js.Application.TotalScore == 0)
            //    {
            //        ScoreService.ComputeTotalScore(js.Application);
            //    }
            //}
            gvNominees.DataSource = list; 
            gvNominees.DataBind();
            TimeSpan ts = DateTime.Now - dt;
            log.Debug("LoadNominees took " + ts.ToString());
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
            JudgeStatus js = e.Row.DataItem as JudgeStatus;
            if (null != js)
            {
                Label labStatus = e.Row.FindControl("labStatus") as Label;
                Label labScore = e.Row.FindControl("labScore") as Label;
                Label labTotalScore = e.Row.FindControl("labTotalScore") as Label;
                Label labRanking = e.Row.FindControl("labRanking") as Label;
                HyperLink btnScores = e.Row.FindControl("btnScores") as HyperLink;
                labStatus.Text = js.Phase.ToDescription();
                labScore.Text = js.Score.ToString("F");
                labTotalScore.Text = js.Portfolio.TotalScore.ToString("F");
                labRanking.Text = js.Portfolio.Ranking.ToString();
                if (js.Score == 0)
                {
                    labScore.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    //labStatus.Text = js.Application.Status.ToDescription();
                    labScore.Text = ScoreService.ComputeScore(Judge, js.Portfolio).ToString("F");
                }

                //labScore.Attributes.Add("onmouseover", "tooltip.show('" + GetOtherScores(js.Application) + "');");
                //labScore.Attributes.Add("onmouseout", "tooltip.hide();");

                btnScores.Visible = !js.Submitted;
            }
        }
        
    }

    private string GetOtherScores(Portfolio portfolio)
    {
        StringBuilder html = new StringBuilder("<table><tr><th colspan=\"2\">Judges Scores</th></tr>");
        IList<Judge> judges = ScoreService.GetJudgesByCategory(Judge, Judge.Category);
        if (judges.Count == 0)
        {
            html.Append("<tr><td>No other judges found</td></tr>");
        }
        foreach (Judge j in judges)
        {
            JudgeStatus js = ScoreService.GetJudgeStatus(j, portfolio);
            if (null != js)
            {
                html.Append(string.Format("<tr><td>{0:s}</td><td align=\"right\">&nbsp;{1:F}</td></tr>", j.User.FullName, js.Score));
            }
        }
        html.Append("</table>");
        return html.ToString();
    }

    private void ViewScores(int id)
    {
        Label lblName = modalDialog.FindControl("lblName") as Label;
        Label lblFooter = modalDialog.FindControl("lblFooter") as Label;
        ListView lv = modalDialog.FindControl("lvScores") as ListView;
        Portfolio p = PortfolioService.GetPortfolio(id);
        lblName.Text = p.User.FullName;
        lblFooter.Text = " Overall Average Score: " + p.TotalScore.ToString("F");
        IList<JudgeStatus> list = ScoreService.GetJudgeStatusForPortfolio(p);
        lv.DataSource = list.Where(x => x.Judge.User.Role == Judge.User.Role);
        lv.DataBind();
        modalDialog.ShowModal();
    }

    protected void lvScores_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem dataItem = (ListViewDataItem)e.Item;

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            JudgeStatus js = dataItem.DataItem as JudgeStatus;
            Label lblName = e.Item.FindControl("lblName") as Label;
            Label labStatus = e.Item.FindControl("labStatus") as Label;
            Label labScore = e.Item.FindControl("labScore") as Label;
            Label labSubmitted = e.Item.FindControl("labSubmitted") as Label;
            lblName.Text = js.Judge.User.FullName;
            labStatus.Text = js.Phase.ToDescription();
            labScore.Text = js.Score.ToString("F");
            labSubmitted.Text = (js.Submitted) ? "Yes" : "No";
            if (!js.Submitted)
            {
                labSubmitted.ForeColor = System.Drawing.Color.Red;
                labScore.ForeColor = System.Drawing.Color.Red;
            }
        }

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        modalDialog.HideModal();
    }

    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        ScoreService.SubmitScores(Judge);
        LoadNominees();
    }

    protected void gvNominees_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if ("ViewScores".Equals(e.CommandName) && null != e.CommandArgument)
        {
            ViewScores(Convert.ToInt32(e.CommandArgument));
        }
    }
}
