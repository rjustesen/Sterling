using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using SS.Service;
using SS.Model;
using Spring.Context;
using log4net;

/// <summary>
/// Summary description for ScoreService
/// </summary>
[WebService(Namespace = "http://benlife.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ScoreWebService : System.Web.Services.WebService
{

    private ILog log = LogManager.GetLogger(typeof(ScoreWebService));

    public static ScoreService ScoreService;
    public static PortfolioService PortfolioService;

    
    public ScoreWebService()
    {
        
    }

    [WebMethod]
    public string ComputeScore(string value, int id)
    {
        Score score = ScoreService.GetScore(id);
        double total = 0;
        total = score.Portfolio.TotalScore;
        if (null != score && null != value )
        {
            try
            {
                double val = Convert.ToDouble(value);
                if (val <= score.Category.MaxRange && val >= score.Category.MinRange)
                {
                    score.Value = val;
                    ScoreService.Save(score);
                }
                total = ScoreService.ComputeScore(score.Judge, score.Portfolio);
            }
            catch (Exception e)
            {
                log.Debug(e.Message);
            }
        }
        return total.ToString("F");
    }
}

