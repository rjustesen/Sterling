using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;

public partial class Admin_Nominees : BasePage
{

    public ScoreService ScoreService;

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
                    Response.Redirect("~/Default.aspx");
                }
            }
            
            LoadRegion();
            SortDirection = SortDirection.Ascending;
            SortColumn = "p.Ranking";
        }
    }

    private IList<Portfolio> GetPortfolios()
    {
        School school = null;
        Category category = null;
        Region region = null;
        string where = " where 1=1 ";

        if (ddlRegion.SelectedValue != "-1")
        {
            region = RegionService.GetRegion(ddlRegion.SelectedValue);
            where += " and p.School.Area.Region.Id = " + region.Id;
        }
        if (ddlCategories.SelectedValue != "-1" && null != region)
        {
            category = CategoryService.GetCategory(region, ddlCategories.SelectedValue);
            where += " and p.Category.Id = " + category.Id;
        }

        if (ddlArea.SelectedValue != "-1" && null != region)
        {
            Area area = null;
            if (null != region)
            {
                area = RegionService.GetArea(region, ddlArea.SelectedValue);
            }
            where += " and p.School.Area.Id = " + area.Id;
        }
        if (ddlSchools.SelectedValue != "-1")
        {
            school = RegionService.GetSchool(ddlSchools.SelectedValue);
            where += " and p.School.Id = " + school.Id;
        }

        if (ddlStatus.SelectedValue != "-1")
        {
            Status status = (Status)Enum.Parse(typeof(Status), ddlStatus.SelectedValue);
            where += " and p.Status = " + (int)status;
        }
        string orderby = (SortDirection == SortDirection.Ascending) ? " order by " + SortColumn + " ASC " : " order by " + SortColumn + " DESC ";
        IList<Portfolio> list = PortfolioService.GetPortfolios(where + orderby );
        
        return list;
    }
    /// <summary>
    /// Load all users
    /// </summary>
    private void LoadNominees()
    {
        IList<Portfolio> list = GetPortfolios();
        btnAdvance.Visible = (list.Count > 0);
        btnCancel.Visible = (list.Count > 0);
        btnEliminate.Visible = (list.Count > 0);
        gvNominees.DataSource = list;
        gvNominees.DataBind();
        lblRecCount.Text = string.Format("{0:d} nominees found", list.Count);
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
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
            if (e.Row.RowState == DataControlRowState.Edit)
            {
                DropDownList ddlEditStatus =  e.Row.FindControl("ddlEditStatus") as DropDownList;
                ddlEditStatus.SelectedValue = app.Status.ToString();
            }
            else
            {
                Label labStatus = e.Row.FindControl("labStatus") as Label;
                if (null != labStatus)
                {
                    labStatus.Text = app.Status.ToDescription();
                }
            }
            ListView lv = e.Row.FindControl("lvScores") as ListView;
            lv.DataSource = ScoreService.GetJudgeStatusForPortfolio(app);
            lv.DataBind();
        }
        
    }

    protected void gvNominees_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if ("Sort".Equals(e.CommandName))
        {
            return;
        }
        if ("Edit".Equals(e.CommandName))
        {
        }
    }

    protected void gvNominees_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    protected void gvNominees_Sorting(object sender, GridViewSortEventArgs e)
    {
        SortDirection =
             (SortDirection == SortDirection.Ascending) ? SortDirection.Descending : SortDirection.Ascending;
        SortColumn = e.SortExpression;
        LoadNominees();
    }

    protected void gvNominees_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvNominees.EditIndex = e.NewEditIndex;
        LoadNominees();
    }

    protected void gvNominees_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        // Retrieve the row being edited. 
        int index = gvNominees.EditIndex;
        GridViewRow row = gvNominees.Rows[index];
        DropDownList ddlEditStatus = row.FindControl("ddlEditStatus") as DropDownList;
        TextBox txtEditScore = row.FindControl("txtEditScore") as TextBox;
        TextBox txtEditRanking = row.FindControl("txtEditRanking") as TextBox;
        int id = Convert.ToInt32(gvNominees.DataKeys[e.RowIndex].Value);
        Portfolio p = PortfolioService.GetPortfolio(id);
        if (null != p)
        {
            p.Status = (Status)Enum.Parse(typeof(Status), ddlEditStatus.SelectedValue);
            p.TotalScore = Convert.ToDouble(txtEditScore.Text);
            p.Ranking = Convert.ToInt32(txtEditRanking.Text);
            PortfolioService.Save(p);
            e.Cancel = false;
            gvNominees.EditIndex = -1;
        }
        else
        {
            e.Cancel = true;
        }
        LoadNominees();
    }


    protected void gvNominees_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvNominees.EditIndex = -1;
     //   LoadNominees();
    } 


    protected void lvScores_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        ListViewDataItem dataItem = (ListViewDataItem)e.Item;

        if (e.Item.ItemType == ListViewItemType.DataItem)
        {
            JudgeStatus js = dataItem.DataItem as JudgeStatus;
            Label lblName = e.Item.FindControl("lblName") as Label;
            Label lblRole = e.Item.FindControl("lblRole") as Label;
            Label labStatus = e.Item.FindControl("labStatus") as Label;
            Label labScore = e.Item.FindControl("labScore") as Label;
            Label labSubmitted = e.Item.FindControl("labSubmitted") as Label;
            lblName.Text = js.Judge.User.FullName;
            labStatus.Text = js.Phase.ToDescription();
            labScore.Text = js.Score.ToString("F");
            labSubmitted.Text = js.Submitted.ToString();
            lblRole.Text = js.Judge.User.Role.ToDescription();
            if (!js.Submitted)
            {
                labSubmitted.ForeColor = System.Drawing.Color.Red;
                labScore.ForeColor = System.Drawing.Color.Red;
            }
        }

    }

    protected void btnAdvance_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvNominees.Rows)
        {
            
            CheckBox cb = row.FindControl("chkNominee") as CheckBox;
            if (null != cb && cb.Checked)
            {
                int id = Convert.ToInt32(gvNominees.DataKeys[row.RowIndex].Value);
                Portfolio app = PortfolioService.GetPortfolio(id);
                ScoreService.AdvanceStatus(app);
            }
        }
        LoadNominees();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvNominees.Rows)
        {

            CheckBox cb = row.FindControl("chkNominee") as CheckBox;
            if (null != cb && cb.Checked)
            {
                int id = Convert.ToInt32(gvNominees.DataKeys[row.RowIndex].Value);
                Portfolio app = PortfolioService.GetPortfolio(id);
                app.Status = Status.Canceled;
                PortfolioService.Save(app);
            }
        }
        LoadNominees();
    }

    protected void btnEliminate_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvNominees.Rows)
        {

            CheckBox cb = row.FindControl("chkNominee") as CheckBox;
            if (null != cb && cb.Checked)
            {
                int id = Convert.ToInt32(gvNominees.DataKeys[row.RowIndex].Value);
                Portfolio app = PortfolioService.GetPortfolio(id);
                app.Status = Status.Elimated;
                PortfolioService.Save(app);
            }
        }
        LoadNominees();
    }


    private void LoadRegion()
    {
        Region region = null;
        if (CurrentUser.Role == RoleType.Administrator)
        {
            ddlRegion.DataMember = "Id";
            ddlRegion.DataTextField = "Name";
            ddlRegion.DataSource = RegionService.GetRegions();
            ddlRegion.DataBind();
            region = RegionService.GetRegion(ddlRegion.SelectedValue);
        }
        else
        {
            AreaUser au = UserService.GetAreaUser(CurrentUser);
            region = au.Area.Region;
            ddlRegion.Items.Insert(0, new ListItem(au.Area.Region.Name, au.Area.Region.Name));
        }
        ddlCategories.DataSource = CategoryService.GetCategories(region);
        ddlCategories.DataMember = "Id";
        ddlCategories.DataTextField = "Name";
        ddlCategories.DataBind();
        ddlCategories.Items.Insert(0, new ListItem("All", "-1"));
        LoadAreas(region);
    }

    private void LoadAreas(Region region)
    {
        ddlArea.DataMember = "Id";
        ddlArea.DataTextField = "Name";
        ddlArea.DataSource = region.Areas;
        ddlArea.DataBind();
        if (CurrentUser.Role == RoleType.Administrator)
        {
            ddlArea.Items.Insert(0, new ListItem("All", "-1"));
        }
        Area area = RegionService.GetArea(region, ddlArea.SelectedValue);
        LoadSchools(area);
    }
    private void LoadSchools(Area area)
    {
        ddlSchools.DataMember = "Id";
        ddlSchools.DataTextField = "Name";
        ddlSchools.DataSource = RegionService.GetSchools(area);
        ddlSchools.DataBind();
        ddlSchools.Items.Insert(0, new ListItem("All", "-1"));
    }

    protected void ddlArea_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!"-1".Equals(ddlArea.SelectedValue))
        {
            Region region = RegionService.GetRegion(ddlRegion.SelectedValue);
            Area area = RegionService.GetArea(region,((DropDownList)sender).SelectedValue);
            LoadSchools(area);
        }
    }

    protected void ddlRegion_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (!"-1".Equals(ddlRegion.SelectedValue))
        {
            Region region = RegionService.GetRegion(ddlRegion.SelectedValue);
            LoadAreas(region);
            ddlCategories.DataSource = CategoryService.GetCategories(region);
            ddlCategories.DataMember = "Id";
            ddlCategories.DataTextField = "Name";
            ddlCategories.DataBind();
            ddlCategories.Items.Insert(0, new ListItem("All", "-1"));
        }
    }
    //GridViewExportUtil.ExportToCSV("Summary.csv", gvInvoice);
}
