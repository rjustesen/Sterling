using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;

public partial class _Default :  BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            switch (CurrentUser.Role)
            {
                case RoleType.Administrator:
                case RoleType.RegionAdministrator:
                    Response.Redirect("~/Admin/Default.aspx");
                    break;
                case RoleType.Nominee:
                    Response.Redirect("~/Nominee/Default.aspx");
                    break;
                case RoleType.AreaJudge:
                case RoleType.RegionJudge:
                    Response.Redirect("~/Judge/Default.aspx");
                    break;
                case RoleType.Coordinator:
                case RoleType.Principal:
                    Response.Redirect("~/School/Default.aspx");
                    break;
                case RoleType.OverallJudge:
                    Response.Redirect("~/OverallJudge/Default.aspx");
                    break;
                default:
                    break;
            }
        }
    }

}
