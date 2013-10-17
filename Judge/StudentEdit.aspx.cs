using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;
using Spring.Context;

public partial class Judge_StudentEdit : BasePage
{
    private double totalScore = 0;

   
    public ScoreService ScoreService;


    public Judge Judge
    {
        get
        {
            return Session["CurrentJudge"] as Judge;
        }
        set { Session["CurrentJudge"] = value; }
    }

    private int Id
    {
        get { return Convert.ToInt32(ViewState["StatusID"]); }
        set { ViewState["StatusID"] = value.ToString(); }
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
                Id = Convert.ToInt32(Request["id"]);
                Judge = UserService.GetJudge(CurrentUser);
                LoadApplication();
            }
        }
    }

    private void LoadApplication()
    {
        JudgeStatus js = ScoreService.GetJudgeStatus(Id);

        lblName.Text = js.Portfolio.User.FullName;
        lblSchool.Text = js.Portfolio.School.Name;
        lblCategory.Text = js.Portfolio.Category.Name;
        lblStatus.Text = ScoreService.GetScoreType(js.Portfolio.Status).ToDescription();
        lblRank.Text = js.Portfolio.Ranking.ToString();
        lblPhase.Text = js.Phase.ToDescription();
        BindScores(js);
        lnkPortfolio.NavigateUrl = "../ReportView.aspx?report=profile&id=" + js.Portfolio.Id.ToString();
    }


    private void BindScores(JudgeStatus js)
    {
        ScoreType type = ScoreService.GetScoreType(js.Portfolio.Status);
        var list = from s in js.Portfolio.Scores
                   where s.Type == type && s.Judge.Id == Judge.Id
                            select s;
        totalScore = ScoreService.ComputeScore(Judge, js.Portfolio);
        gvScores.DataSource = list;
        gvScores.DataBind();
    }

   
    protected void gvScores_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Score score = e.Row.DataItem as Score;
            Label lblName = e.Row.FindControl("lblName") as Label;
            Label lblMasId = e.Row.FindControl("lblMasId") as Label;
            TextBox txtScore = e.Row.FindControl("txtScore") as TextBox;
            Label lblRange = e.Row.FindControl("lblRange") as Label;

            RangeValidator rangeValidator1 = e.Row.FindControl("rangeValidator1") as RangeValidator;
          //  RegularExpressionValidator regValidator1 = e.Row.FindControl("regValidator1") as RegularExpressionValidator;

            txtScore.Attributes.Add("onchange", "javascript:ComputeScore('" + txtScore.ClientID+ "',"+ score.Id + ");");
            
            lblName.Text = score.Category.Name;
            rangeValidator1.MinimumValue = Convert.ToDouble(score.Category.MinRange).ToString();
            rangeValidator1.MaximumValue = Convert.ToDouble(score.Category.MaxRange).ToString();
            lblRange.Text = string.Format("Minimum: {0:d},  Maximum: {1:d}, Weight: {2:f}%", score.Category.MinRange, score.Category.MaxRange, score.Category.Weight);
            rangeValidator1.ErrorMessage= string.Format("Scores cannot be less than {0:d} or exceed {1:d}.",score.Category.MinRange, score.Category.MaxRange);
            rangeValidator1.ToolTip= string.Format("Scores cannot be less than {0:d} or exceed {1:d}.",score.Category.MinRange, score.Category.MaxRange);
            //labWeight.Text = String.Format(" {0:f}% Weight", score.Category.Weight);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotal = e.Row.FindControl("lblTotal") as Label;
            if (lblTotal != null)
            {
                lblTotal.Text = totalScore.ToString("F");
            }
        }
    }

    protected void UpdateScores()
    {
        foreach (GridViewRow row in gvScores.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    // Get the productId from the GridView's datakeys
                    int id = Convert.ToInt32(gvScores.DataKeys[row.RowIndex].Value);
                    TextBox txtScore = row.Cells[1].FindControl("txtScore") as TextBox;
                    Score score = ScoreService.GetScore(id);
                    double val = Convert.ToDouble(txtScore.Text);
                    if (val <= score.Category.MaxRange && val >= score.Category.MinRange)
                    {
                        score.Value = val;
                        ScoreService.Save(score);
                    }
                }
                catch (Exception e)
                {
                    log.Debug(e.Message);
                }
            }
        }
    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        UpdateScores();
        DateTime dt = DateTime.Now;
        JudgeStatus js = ScoreService.GetJudgeStatus(Id);
        ScoreService.ComputeTotalScore(js.Portfolio);
        TimeSpan ts = DateTime.Now - dt;
        log.Debug("ComputeTotalScore took: " + ts.ToString());
        ScoreService.ComputeRanking(js.Portfolio.Status, js.Portfolio.Category, js.Judge.Area);
        ts = DateTime.Now - dt;
        log.Debug("ComputeRanking took: " + ts.ToString());
        Response.Redirect("~/Judge/Default.aspx");
    }


    protected void btnShowPortfolio_OnClick(object sender, EventArgs e)
    {
        JudgeStatus js = ScoreService.GetJudgeStatus(Id);
        if (ShowPdf1.Visible)
        {
            ShowPdf1.Visible = false;
            btnShowPortfolio.Text = "Show Portfolio";
        }
        else
        {
            btnShowPortfolio.Text = "Hide Portfolio";
            ShowPdf1.Visible = true;
            ShowPdf1.FilePath = "../ReportView.aspx?report=profile&id=" + js.Portfolio.Id.ToString();
        }
        
       
    }
  
}
