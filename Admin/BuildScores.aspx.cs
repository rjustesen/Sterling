using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;


public partial class Admin_BuildScores : BasePage
{
    public ScoreService ScoreService;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int id = Convert.ToInt32(Request["id"]);
            User user = UserService.GetUser(id);
            Judge judge = UserService.GetJudge(user);
            if (null != judge)
            {
                ScoringPhase phase = ScoreService.GetJudgeScoreStatus(judge);
                IList<JudgeStatus> list = ScoreService.GetJudgeStatusForJudge(judge, phase, "j.Status", true);
                if (list.Count == 0)
                {
                    IList<Portfolio> apps = PortfolioService.GetPortfolios(judge.Category);
                    foreach (Portfolio a in apps)
                    {
                        if (ScoreService.IsJudgeInArea(judge, a))
                        {
                            ScoreService.BuildScoresForJudge(a, judge);
                        }
                    }
                }
                MasterPage.ShowInfoMessage("Scores created for Judge " + judge.User.FullName);
            }
        }
    }
}
