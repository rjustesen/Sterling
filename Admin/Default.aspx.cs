using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.Services;
using System.Text;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;


public partial class Admin_Default : BasePage
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


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (null == CurrentUser || CurrentUser.Role != RoleType.Administrator)
            {
                if (CurrentUser.Role != RoleType.RegionAdministrator)
                {
                    MasterPage.ShowErrorMessage("You are not Authorized to view this page");
                    Response.Redirect("~/Logout.aspx");
                }
            }
            if (CurrentUser.Role == RoleType.Administrator)
            {
                ddlSearchRegion.DataSource = RegionService.GetRegions();
                ddlSearchRegion.DataMember = "Id";
                ddlSearchRegion.DataTextField = "Name";
                ddlSearchRegion.DataBind();
                ddlSearchRegion.Items.Insert(0, new ListItem("All", "-1"));
            }
            else
            {
                lnkAttachments.Visible = false;
                AreaUser au = UserService.GetAreaUser(CurrentUser);
                ddlSearchRegion.Items.Insert(0, new ListItem(au.Area.Region.Name, au.Area.Region.Name));
            }
            LoadAreas();
            LoadSchools();
            SortDirection = SortDirection.Descending;
            SortColumn = "FullName";
        }
    }

    /// <summary>
    /// Load all users
    /// </summary>
    private void LoadUsers()
    {
        School school = null;
        Category category = null;
        RoleType role = RoleType.All;
        Region region = null;
        Area area = null;
        if (ddlSearchRegion.SelectedValue != "-1")
        {
            region = RegionService.GetRegion(ddlSearchRegion.SelectedValue);
        }
        if (ddlCategories.SelectedValue != "-1")
        {
           category = CategoryService.GetCategory(region, ddlCategories.SelectedValue);
        }
        if (ddlSearchArea.SelectedValue != "-1")
        {
            area = RegionService.GetArea(region, ddlSearchArea.SelectedValue);
        }
        if (ddlSearchSchool.SelectedValue != "-1")
        {
            school = RegionService.GetSchool(ddlSearchSchool.SelectedValue);
        }
        if (ddlUserType.SelectedValue != "-1")
        {
            role = (RoleType)Convert.ToInt32(ddlUserType.SelectedValue);
        }
        string orderby =  (SortDirection == SortDirection.Ascending) ? " order by " + SortColumn + " ASC " : " order by " + SortColumn  + " DESC ";
        IList<DisplayUser> list = UserService.GetAllUsers(region, area, school, category, txtSearchUserName.Text, role, orderby);
        gvUsers.DataSource = list;
        gvUsers.DataBind();
        lblRecCount.Text = string.Format("{0:d} users found", list.Count);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadUsers();
    }

    protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e)
    {
        SortDirection =
                (SortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        SortColumn = e.SortExpression;
        LoadUsers();
    }

    protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DisplayUser ds = e.Row.DataItem as DisplayUser;
            if (null != ds)
            {
                Label labStatus = e.Row.FindControl("labStatus") as Label;
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnStatus = e.Row.FindControl("btnStatus") as LinkButton;
                LinkButton btnUnlock = e.Row.FindControl("btnUnlock") as LinkButton;
                
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this user?');");

                if (null != ds.Status)
                {
                    labStatus.Text = ((Status)Enum.ToObject(typeof(Status), ds.Status)).ToDescription(); 
                }
                if (ds.Disabled)
                {
                    btnStatus.Text = "Enable";
                }
                else
                {
                    btnStatus.Text = "Disable";
                }
                btnUnlock.Visible = false;
                HyperLink btnView = e.Row.FindControl("btnView") as HyperLink;
                User u = UserService.GetUser(ds.Id);
                if (UserService.IsAccountLocked(u))
                {
                    btnUnlock.Visible = true;
                }
                if (ds.Role == RoleType.Nominee)
                {
                    Portfolio portfolio = PortfolioService.GetPortfolioByUser(u);
                    if (null != portfolio)
                    {
                        btnView.NavigateUrl = "~/ReportView.aspx?report=profile&id=" + portfolio.Id.ToString();
                    }
                }
                else if (ds.Role == RoleType.AreaJudge || ds.Role == RoleType.RegionJudge)
                {
                    btnView.Text = "Create Scores";
                    btnView.NavigateUrl = "~/Admin/BuildScores.aspx?id=" + ds.Id.ToString();
                }
                else
                {
                    btnView.Visible = false;
                }
            }
        }
    }

    /// <summary>
    /// Link to page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if ("Sort".Equals(e.CommandName))
        {
            return;
        }
        if (e.CommandArgument.ToString() != "")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Select")
            {
                EditUser(id);
            }
            if (e.CommandName == "DeleteUser")
            {
                User user = UserService.GetUser(id);
                if (user != null)
                {
                    UserService.DeleteUser(user);
                    LoadUsers();
                }
            }
            if (e.CommandName == "ChangeStatus")
            {
                User user = UserService.GetUser(id);
                if (user != null)
                {
                    user.IsDisabled = !user.IsDisabled;
                    UserService.UpdateUser(user);
                    LoadUsers();
                }
            }
            if (e.CommandName == "ResetPassword")
            {
                User user = UserService.GetUser(id);
                string newPassword = UserService.NewPassword(user);
                user.IsApproved = false;
                user.IsDisabled = false;
                UserService.SaveUser(user);
                EMailService.SendEmail("Sterling Scholar password reset request", UserService.FormatTemplate(System.Web.HttpContext.Current.Server.MapPath("~/") + "/assets/PasswordResetTemplate.html", user), user.EMail, UserService.AdminEmailAddress, null);
            }
            if (e.CommandName == "Unlock")
            {
                User user = UserService.GetUser(id);
                UserService.UnlockUser(user);
                EMailService.SendEmail("Sterling Scholar Account unlock request", UserService.FormatTemplate(System.Web.HttpContext.Current.Server.MapPath("~/") + "/assets/UnlockTemplate.html", user), user.EMail, UserService.AdminEmailAddress, null);
            }
            //if (e.CommandName.Equals("ViewPortfolio"))
            //{
            //    StringBuilder sb = new StringBuilder();
            //    sb.Append("<script language=JavaScript id='openit'>");
            //    sb.Append("window.open('../ReportView.aspx?report=profile&id=" + e.CommandArgument  + "', '', '');");
            //    sb.Append("</script>");
            //    if (!ClientScript.IsStartupScriptRegistered("openit"))
            //    {
            //        ClientScript.RegisterStartupScript(this.GetType(), "openit", sb.ToString());
            //    }
            //}
        }
    }

    /// <summary>
    /// Add some attributes to the grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvUsers_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes.Add("onmouseover", "this.style.color='#0000FF'; this.style.cursor='pointer'; ");
            //e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#BEBBBB'; this.style.color='#444444';");
            //if (e.Row.RowIndex % 2 == 0)
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#85A3C2'; this.style.color='#0000FF'; this.style.cursor='pointer';");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White'; this.style.color='#444444';this.style.textDecoration='none';");
            //}
            //else
            //{
            //    e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#85A3C2'; this.style.color='#0000FF'; this.style.cursor='pointer';");
            //    e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#BEBBBB'; this.style.color='#BEBBBB';this.style.textDecoration='none';");
            //}
            //for (int i  = 0; i < e.Row.Cells.Count - 3; i++)
            //{
            //    e.Row.Cells[i].Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(gvUsers, "Select$" + e.Row.RowIndex);
            //}
        }
    }

    private void EditUser(int id)
    {
        UserInput input = UserService.GetUserProfile(id);
        
        Label lblId = modalDialog.FindControl("lblId") as Label;
        TextBox txtFullName = modalDialog.FindControl("txtFullName") as TextBox;
        TextBox txtEmail = modalDialog.FindControl("txtEmail") as TextBox;
        TextBox txtComment = modalDialog.FindControl("txtComment") as TextBox;
        TextBox txtPhone = modalDialog.FindControl("txtPhone") as TextBox;
        DropDownList ddlRoleType = modalDialog.FindControl("ddlRoleType") as DropDownList;
        DropDownList ddlRegion = modalDialog.FindControl("ddlRegion") as DropDownList;
        DropDownList ddlArea = modalDialog.FindControl("ddlArea") as DropDownList;
        DropDownList ddlCategory = modalDialog.FindControl("ddlCategory") as DropDownList;
        DropDownList ddlSchool = modalDialog.FindControl("ddlSchool") as DropDownList;
        Label labCustomError = modalDialog.FindControl("labCustomError") as Label;

        labCustomError.Text = "";
        labCustomError.Visible = false;
        lblId.Text = id.ToString();
        txtFullName.Text = input.FullName;
        txtEmail.Text = input.EMail;
        txtComment.Text = input.Comment;
        txtPhone.Text = input.Phone;
        int i = 0;

        ddlRoleType.Items.Clear();
        ddlRegion.Items.Clear();
        ddlArea.Items.Clear();
        ddlCategories.Items.Clear();
        ddlSchool.Items.Clear();
        

        Region region = null;
        if (CurrentUser.Role == RoleType.Administrator)
        {
            ddlRoleType.Items.Insert(i++, new ListItem("Administrator", "0", true));
            ddlRegion.DataMember = "Id";
            ddlRegion.DataTextField = "Name";
            IList<Region> regions = RegionService.GetRegions();
            ddlRegion.DataSource = regions;
            ddlRegion.DataBind();
            region = regions[0];
        }
        else if (CurrentUser.Role == RoleType.RegionAdministrator)
        {
            AreaUser au = UserService.GetAreaUser(CurrentUser);
            ddlRegion.Items.Insert(0, new ListItem(au.Area.Region.Name, au.Area.Region.Name));
            region = au.Area.Region;
        }
        if (id == -1)
            input.Role = RoleType.Nominee;

        ddlRoleType.Items.Insert(i++, new ListItem("Region Administrator", "7"));
        ddlRoleType.Items.Insert(i++, new ListItem("School Coordinator", "1", true));
        ddlRoleType.Items.Insert(i++, new ListItem("Area Judge", "2"));
        ddlRoleType.Items.Insert(i++, new ListItem("Region Judge", "3", true));
        ddlRoleType.Items.Insert(i++, new ListItem("Nominee", "4"));
        ddlRoleType.Items.Insert(i++, new ListItem("Principal", "5", true));
        ddlRoleType.SelectedValue = Convert.ToInt32(input.Role).ToString();
      
        if (null != input && null != input.Area && input.Id > 0)
        {
            ddlRegion.SelectedValue = input.Area.Region.Name;
            ddlArea.DataMember = "Id";
            ddlArea.DataTextField = "Name";
            ddlArea.DataSource = RegionService.GetAreas(input.Area.Region);
            ddlArea.DataBind();
            ddlArea.SelectedValue = input.Area.Name;
            ddlCategory.DataMember = "Id";
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataSource = CategoryService.GetCategories(input.Area.Region);
            ddlCategory.DataBind();
            if (null != input.Category)
            {
                ddlCategory.SelectedValue = input.Category.Name;
            }
            LoadSchools(input.Area);
            if (null != input.School)
            {
                ddlSchool.SelectedValue = input.School.Name;
            }
        }
        else
        {
            ddlRegion.SelectedValue = region.Name;
            ddlArea.DataMember = "Id";
            ddlArea.DataTextField = "Name";
            ddlArea.DataSource = RegionService.GetAreas(region);
            ddlArea.DataBind();
            ddlArea.SelectedValue = region.Areas[0].Name;

            ddlCategory.DataMember = "Id";
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataSource = CategoryService.GetCategories(region);
            ddlCategory.DataBind();
        }

        ShowDropDowns(input.Role);
        modalDialog.ShowModal();
    }

    protected void lnkNew_Click(object sender, EventArgs e)
    {
        EditUser(-1);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        
        Label lblId = modalDialog.FindControl("lblId") as Label;
        TextBox txtFullName = modalDialog.FindControl("txtFullName") as TextBox;
        TextBox txtEmail = modalDialog.FindControl("txtEmail") as TextBox;
        TextBox txtComment = modalDialog.FindControl("txtComment") as TextBox;
        TextBox txtPhone = modalDialog.FindControl("txtPhone") as TextBox;
        DropDownList ddlRoleType = modalDialog.FindControl("ddlRoleType") as DropDownList;
        DropDownList ddlRegion = modalDialog.FindControl("ddlRegion") as DropDownList;
        DropDownList ddlArea = modalDialog.FindControl("ddlArea") as DropDownList;
        DropDownList ddlCategory = modalDialog.FindControl("ddlCategory") as DropDownList;
        DropDownList ddlSchool = modalDialog.FindControl("ddlSchool") as DropDownList;
        Label labCustomError = modalDialog.FindControl("labCustomError") as Label;

        UserInput input = new UserInput();
        input.Id = Convert.ToInt32(lblId.Text);
        input.FullName = txtFullName.Text;
        input.EMail = txtEmail.Text;
        input.Comment = txtComment.Text;
        input.Phone = txtPhone.Text;
        input.Role = (RoleType)Convert.ToInt32(ddlRoleType.SelectedValue);

        if (input.Role == RoleType.RegionAdministrator)
        {
            Region region = RegionService.GetRegion(ddlRegion.SelectedValue);
            input.Area = RegionService.GetArea(region, ddlArea.SelectedValue);
        }
        if (input.Role == RoleType.AreaJudge | input.Role == RoleType.RegionJudge)
        {
            input.Area = RegionService.GetArea(ddlArea.SelectedValue);
            input.Category = CategoryService.GetCategory(input.Area.Region, ddlCategory.SelectedValue);
        }
        else if (input.Role == RoleType.Coordinator | input.Role == RoleType.Principal | input.Role == RoleType.Nominee)
        {
            input.School = RegionService.GetSchool(ddlSchool.SelectedValue);
            if (input.Role == RoleType.Nominee)
            {
                input.Category = CategoryService.GetCategory(input.Area.Region, ddlCategory.SelectedValue);
            }
        }

        MembershipCreateStatus status = UserService.AddNewUser(input);
        if (status == MembershipCreateStatus.DuplicateEmail)
        {
            labCustomError.Text = "The email address is a duplicate, please enter a different email address";
            labCustomError.Visible = true;
        }
        else
        {
            User user = UserService.GetUser(input.EMail);
            if (input.Id <= 0)
            {
                UserService.SendUserEmail(user, "Sterling Scholar Registration Request", System.Web.HttpContext.Current.Server.MapPath("~/") + "/assets/NewUserTemplate.html");
            }
            modalDialog.HideModal();
        }
    }


    protected void btnClose_Click(object sender, EventArgs e)
    {
        modalDialog.HideModal();
    }

    protected void ddlRegion_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlRegion = modalDialog.FindControl("ddlRegion") as DropDownList;
        DropDownList ddlArea = modalDialog.FindControl("ddlArea") as DropDownList;
        DropDownList ddlCategory = modalDialog.FindControl("ddlCategory") as DropDownList;
        Region region  = RegionService.GetRegion(ddlRegion.SelectedValue);
        ddlArea.DataMember = "Id";
        ddlArea.DataTextField = "Name";
        ddlArea.DataSource = region.Areas;
        ddlArea.DataBind();
        ddlCategory.DataMember = "Id";
        ddlCategory.DataTextField = "Name";
        ddlCategory.DataSource = CategoryService.GetCategories(region);
        ddlCategory.DataBind();
        LoadSchools(region.Areas[0]);
    }

    private void ShowDropDowns(RoleType role)
    {
        DropDownList ddlRegion = modalDialog.FindControl("ddlRegion") as DropDownList;
        DropDownList ddlArea = modalDialog.FindControl("ddlArea") as DropDownList;
        DropDownList ddlCategory = modalDialog.FindControl("ddlCategory") as DropDownList;
        DropDownList ddlSchool = modalDialog.FindControl("ddlSchool") as DropDownList;
        Label labRole = modalDialog.FindControl("labRole") as Label;
        Label labSchool = modalDialog.FindControl("labSchool") as Label;
        Label labRegion = modalDialog.FindControl("labRegion") as Label;
        Label labArea = modalDialog.FindControl("labArea") as Label;
        Label labCategory = modalDialog.FindControl("labCategory") as Label;
                
        if (role == RoleType.AreaJudge | role == RoleType.RegionJudge)
        {
            ddlRegion.Visible = true;
            ddlArea.Visible = (role == RoleType.AreaJudge);
            labArea.Visible = (role == RoleType.AreaJudge);
            ddlSchool.Visible = false;
            labSchool.Visible = false;
            ddlCategory.Visible = true;
            labCategory.Visible = true;
        } 
        else if (role == RoleType.Coordinator | role == RoleType.Principal)
        {
            ddlRegion.Visible = true;
            labRegion.Visible = true;
            ddlArea.Visible = true;
            labArea.Visible = true;
            ddlSchool.Visible = true;
            labSchool.Visible = true;
            ddlCategory.Visible = false;
            labCategory.Visible = false;
            Area area = RegionService.GetArea(ddlArea.SelectedValue);
            LoadSchools(area);
        }
        else if (role == RoleType.Administrator)
        {
            ddlRegion.Visible = false;
            ddlArea.Visible = false;
            ddlCategory.Visible = false;
            ddlSchool.Visible = false;
            labSchool.Visible = false;
            labRegion.Visible = false;
            labArea.Visible = false;
            labCategory.Visible = false;
        }
        else if (role == RoleType.RegionAdministrator)
        {
            ddlRegion.Visible = true;
            ddlArea.Visible = true;
            ddlCategory.Visible = false;
            ddlSchool.Visible = false;
            labSchool.Visible = false;
            labRegion.Visible = true;
            labArea.Visible = true;
            labCategory.Visible = false;
        }
        else if (role == RoleType.Nominee)
        {
            ddlRegion.Visible = true;
            labRegion.Visible = true;
            ddlArea.Visible = true;
            labArea.Visible = true;
            ddlSchool.Visible = true;
            labSchool.Visible = true;
            ddlCategory.Visible = true;
            labCategory.Visible = true;
            Region region = RegionService.GetRegion(ddlRegion.SelectedValue);
            Area area = RegionService.GetArea(region, ddlArea.SelectedValue);
            LoadSchools(area);
        }
    }

    private void LoadSchools(Area area)
    {
        DropDownList ddlSchool = modalDialog.FindControl("ddlSchool") as DropDownList;
        ddlSchool.DataMember = "Id";
        ddlSchool.DataTextField = "Name";
        ddlSchool.DataSource = RegionService.GetSchools(area);
        ddlSchool.DataBind();
    }

    protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Area area = RegionService.GetArea(((DropDownList)sender).SelectedValue);
        LoadSchools(area);
    }

    protected void ddlRoleType_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        RoleType role = (RoleType)Convert.ToInt32(((DropDownList)sender).SelectedValue);
        ShowDropDowns(role);
    }

    private void LoadAreas()
    {
        if (!"-1".Equals(ddlSearchRegion.SelectedValue))
        {
            Region region = RegionService.GetRegion(ddlSearchRegion.SelectedValue);
            ddlSearchArea.DataSource = RegionService.GetAreas(region);
            ddlSearchArea.DataMember = "Id";
            ddlSearchArea.DataTextField = "Name";
            ddlSearchArea.DataBind();
            if (region.Areas.Count > 1)
            {
                ddlSearchArea.Items.Insert(0, new ListItem("All", "-1"));
            }
            ddlCategories.DataSource = CategoryService.GetCategories(region);
            ddlCategories.DataMember = "Id";
            ddlCategories.DataTextField = "Name";
            ddlCategories.DataBind();
            ddlCategories.Items.Insert(0, new ListItem("All", "-1"));
        }
        else
        {
            ddlSearchArea.DataSource = null;
            ddlSearchArea.DataBind();
            ddlCategories.DataSource = null;
            ddlCategories.DataBind();
            ddlSearchArea.Items.Insert(0, new ListItem("All", "-1"));
            ddlCategories.Items.Insert(0, new ListItem("All", "-1"));
        }
    }

    private void LoadSchools()
    {
        if (!"-1".Equals(ddlSearchRegion.SelectedValue))
        {
            Region region = RegionService.GetRegion(ddlSearchRegion.SelectedValue);
            Area area = RegionService.GetArea(region, ddlSearchArea.SelectedValue);
            IList<School> schools = RegionService.GetSchools(area);
            ddlSearchSchool.DataSource = schools;
            ddlSearchSchool.DataMember = "Id";
            ddlSearchSchool.DataTextField = "Name";
            ddlSearchSchool.DataBind();
            ddlSearchSchool.Items.Insert(0, new ListItem("All", "-1"));
        }
        else
        {
            ddlSearchSchool.DataSource = null;
            ddlSearchSchool.DataBind();
            ddlSearchSchool.Items.Insert(0, new ListItem("All", "-1"));
        }
    }

    protected void ddlSearchRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadAreas();
        LoadSchools();
    }
    protected void ddlSearchArea_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSchools();
    }

}
