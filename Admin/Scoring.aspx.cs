using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;
using Spring.Context;

public partial class Admin_Scoring : BasePage
{
    public ScoreService ScoreService;

    //public ScoreService ScoreService
    //{
    //    get
    //    {
    //        if (null == _scoreService)
    //        {
    //            IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
    //            _scoreService = ctx["ScoreService"] as ScoreService;
    //        }
    //        return _scoreService;
    //    }
    //    set { _scoreService = value; }
    //}


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
            if (CurrentUser.Role == RoleType.Administrator)
            {
                ddlSearchRegion.DataSource = RegionService.GetRegions();
                ddlSearchRegion.DataMember = "Id";
                ddlSearchRegion.DataTextField = "Name";
                ddlSearchRegion.DataBind();
            }
            else
            {
                AreaUser au = UserService.GetAreaUser(CurrentUser);
                ddlSearchRegion.Items.Insert(0, new ListItem(au.Area.Region.Name, au.Area.Region.Name));
            }

            LoadScores();
        }
    }

    private void LoadScores()
    {
        Region region = RegionService.GetRegion(ddlSearchRegion.SelectedValue);
        gvScore.DataSource = ScoreService.GetScoreCategories(region);
        gvScore.DataBind();
    }

    protected void gvScore_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ScoreCategory sc = e.Row.DataItem as ScoreCategory;
            if (null != sc)
            {
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this user?');");
            }
        }
    }

    protected void lnkNew_Click(object sender, EventArgs e)
    {
        EditScore(-1);
    }

    private void EditScore(int id)
    {
        Label lblId = modalDialog.FindControl("lblId") as Label;
        TextBox txtName = modalDialog.FindControl("txtName") as TextBox;
        TextBox txtWeight = modalDialog.FindControl("txtWeight") as TextBox;
        TextBox txtMinRange = modalDialog.FindControl("txtMinRange") as TextBox;
        TextBox txtMaxRange = modalDialog.FindControl("txtMaxRange") as TextBox;

        ScoreCategory sc = null;
        if (id > 0)
        {
            sc = ScoreService.GetScoreCategory(id);
        }
        else
        {
            sc = new ScoreCategory();
            Region region = RegionService.GetRegion(ddlSearchRegion.SelectedValue);
            sc.Region = region;
        }
        lblId.Text = id.ToString();
        txtName.Text = sc.Name;
        txtWeight.Text = sc.Weight.ToString();
        txtMinRange.Text = sc.MinRange.ToString();
        txtMaxRange.Text = sc.MaxRange.ToString();
        modalDialog.ShowModal();
    }

    protected void gvScore_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditScore")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            EditScore(id);
        }
        if (e.CommandName == "DeleteScore")
        {
            int id = Convert.ToInt32(e.CommandArgument);
            ScoreCategory sc = ScoreService.GetScoreCategory(id);
            ScoreService.Delete(sc);
            LoadScores();
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        this.Validate("ScoreEdit");
        if (IsValid)
        {
            Label lblId = modalDialog.FindControl("lblId") as Label;
            TextBox txtName = modalDialog.FindControl("txtName") as TextBox;
            TextBox txtWeight = modalDialog.FindControl("txtWeight") as TextBox;
            TextBox txtMinRange = modalDialog.FindControl("txtMinRange") as TextBox;
            TextBox txtMaxRange = modalDialog.FindControl("txtMaxRange") as TextBox;
            ScoreCategory sc = null;
            if (Convert.ToInt32(lblId.Text) > 0)
            {
                sc = ScoreService.GetScoreCategory(Convert.ToInt32(lblId.Text));
            }
            else
            {
                sc = new ScoreCategory();
                Region region = RegionService.GetRegion(ddlSearchRegion.SelectedValue);
                sc.Region = region;
            }
            sc.Name = txtName.Text;
            sc.Weight = Convert.ToInt32(txtWeight.Text);
            sc.MinRange = Convert.ToInt32(txtMinRange.Text);
            sc.MaxRange = Convert.ToInt32(txtMaxRange.Text);
            ScoreService.Save(sc);
            modalDialog.HideModal();
            LoadScores();
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        modalDialog.HideModal();
    }

    protected void ddlSearchRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadScores();     
    }

}
