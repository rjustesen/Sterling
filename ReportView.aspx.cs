using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SS.Model;
using SS.Service;

public partial class ReportView : BasePage
{
    public ReportService ReportService;


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string report = Request["report"];
            MemoryStream stream = null;
            
            if ("profile".Equals(report))
            {
                string id = Request["id"];
                String UrlDirectory = Request.Url.GetLeftPart(UriPartial.Path);
                UrlDirectory = UrlDirectory.Substring(0, UrlDirectory.LastIndexOf("/"));
                Portfolio portfolio = PortfolioService.GetPortfolio(Convert.ToInt32(id));
                if (CurrentUser.Role == RoleType.Nominee)
                {
                    if (portfolio.User.Id != CurrentUser.Id)
                    {
                        portfolio = PortfolioService.GetPortfolioByUser(CurrentUser);
                    }
                }
                else if (CurrentUser.Role == RoleType.Coordinator)
                {
                    School school = RegionService.GetSchoolByUser(CurrentUser);
                    if (portfolio.School.Id != school.Id)
                    {
                        portfolio = null;
                    }
                }
                if (null != portfolio)
                {
                    IList<Attachment> list = PortfolioService.GetAttachments(AttachmentCategory.Portfolio, AttachmentType.FinalPortfolio, portfolio.Id);
                    if (list.Count > 0)
                    {
                        stream = new MemoryStream();
                        Attachment attachment = PortfolioService.GetAttachment(list[0].Id);
                        stream.Write(attachment.Data, 0, attachment.Data.Length);
                    }
                    else
                    {
                        stream = ReportService.CreatePortfolioReport(portfolio, UrlDirectory);
                    }
                }
            }
            if ("principal".Equals(report))
            {
                if (CurrentUser.Role == RoleType.Principal || CurrentUser.Role == RoleType.Coordinator)
                {
                    string id = Request["id"];
                    Portfolio portfolio = PortfolioService.GetPortfolio(Convert.ToInt32(id));
                    stream = ReportService.CreatePrincipalReport(portfolio);
                }
                else
                {
                    Response.Redirect("~/Default.aspx"); 
                }
            }
            if ("nominees".Equals(report))
            {
                if (CurrentUser.Role != RoleType.Nominee)
                {
                    string id = Request["id"];
                    School school = RegionService.GetSchool(Convert.ToInt32(id));
                    stream = ReportService.NomineeReport(school);
                }
                else
                {
                    Response.Redirect("~/Default.aspx"); 
                }
            }
            Response.Clear();
            Response.ContentType = "application/pdf";

            Response.OutputStream.Write(stream.GetBuffer(), 0, (int)stream.GetBuffer().Length);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();
        }
    }
}
