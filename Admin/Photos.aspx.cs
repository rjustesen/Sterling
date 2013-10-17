using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using SS.Model;
using SS.Service;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Photos : BasePage
{

    public ReportService ReportService;

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
        }
    }

    private string GetWhereClause()
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
        if (ddlCategories.SelectedValue != "-1")
        {
            category = CategoryService.GetCategory(region, ddlCategories.SelectedValue);
            where += " and p.Category.Id = " + category.Id;
        }
        
        if (ddlArea.SelectedValue != "-1")
        {
            Area area = RegionService.GetArea(ddlArea.SelectedValue);
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
      

        return where;
    }
    private void LoadPhotos()
    {
        IList<Portfolio> list = PortfolioService.GetPortfolios(GetWhereClause());
        gvPhotos.DataSource = list;
        gvPhotos.DataBind();
        lblRecCount.Text = string.Format("{0:d} nominees found", list.Count);
    }

    protected void gvPhotos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Portfolio portfolio = e.Row.DataItem as Portfolio;
            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
            Label labStatus = e.Row.FindControl("labStatus") as Label;
            Image imgPhoto = e.Row.FindControl("imgPhoto") as Image;
            LinkButton lnkDownLoad = e.Row.FindControl("lnkDownLoad") as LinkButton;

            labStatus.Text = portfolio.Status.ToDescription();

            Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.PersonalPhoto);
            if (null != image)
            {
                imgPhoto.ImageUrl = "~/ImageHandler.ashx?id=" + image.Id;
                lnkDownLoad.CommandArgument = image.Id.ToString();
            }
            else
            {
                imgPhoto.Visible = false;
                imgPhoto.ImageUrl = "";
            }
        }
    }

   
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue.Equals("1"))
        {
            LoadPhotos();
        }
        else
        {
            LoadUsers();
        }
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
        Region region = RegionService.GetRegion(ddlRegion.SelectedValue);
        if (!"-1".Equals(ddlArea.SelectedValue))
        {
            Area area = RegionService.GetArea(region, ((DropDownList)sender).SelectedValue);
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

    private void LoadUsers()
    {
        School school = null;
        Region region = null;
        if (ddlSchools.SelectedValue != "-1")
        {
            school = RegionService.GetSchool(ddlSchools.SelectedValue);
        }
        if (!"-1".Equals(ddlRegion.SelectedValue))
        {
            region = RegionService.GetRegion(ddlRegion.SelectedValue);
        }
        IList<DisplayUser> list = UserService.GetAllUsers(region,null,school, null, "",  RoleType.Coordinator, "");
        
        gvCoordinators.DataSource = list;
        gvCoordinators.DataBind();
        lblRecCount.Text = string.Format("{0:d} users found", list.Count);
    }
    
    protected void gvPhotos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Download"))
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Attachment attachment = PortfolioService.GetAttachment(id);
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + attachment.Name);
            Response.AddHeader("Content-Length", attachment.Data.Length.ToString());
            Response.BinaryWrite(attachment.Data);
            Response.End();
        }
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlType.SelectedValue.Equals("1"))
        {
            pnlNominees.Visible = true;
            pnlCoordinators.Visible = false;
            ddlStatus.Visible = true;
            ddlCategories.Visible = true;
        }
        else
        {
            pnlNominees.Visible = false;
            pnlCoordinators.Visible = true;
            ddlCategories.Visible = false;
            ddlStatus.Visible = false;
        }
    }
    protected void gvCoordinators_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DisplayUser ds = e.Row.DataItem as DisplayUser;
            User user = UserService.GetUser(ds.Id);
            School school = RegionService.GetSchoolByUser(user);

            Label labSchool = e.Row.FindControl("labSchool") as Label;
            Label labCaption = e.Row.FindControl("labCaption") as Label;
            Image imgPhoto = e.Row.FindControl("imgGroupPhoto") as Image;
            LinkButton lnkDownLoad = e.Row.FindControl("lnkDownLoad") as LinkButton;

            labSchool.Text = school.Name;

            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.User, user.Id);
            Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.GroupPhoto);
            if (null != image)
            {
                imgPhoto.Visible = true;
                imgPhoto.ImageUrl = "~/ImageHandler.ashx?id=" + image.Id;
                lnkDownLoad.CommandArgument = image.Id.ToString();
            }
            IList<KeyValue> keyValues = PortfolioService.GetKeyValues(ObjectTypes.User, user.Id);
            var val = keyValues.FirstOrDefault(x => "GroupCaption".Equals(x.KeyName) && x.Type == KeyValueType.String);
            if (val != null)
            {
                labCaption.Text = val.StringValue;
            }
        }
    }

    protected void btnDownloadAll_Click(object sender, EventArgs e)
    {
        string filename = PortfolioService.ExtractPersonalPhotos(GetWhereClause());
        byte[] buf = FileUtils.LoadFile(filename);
        Response.Clear();
        Response.ContentType = "application/zip";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(System.IO.Path.ChangeExtension(filename, "zip")));
        Response.AddHeader("Content-Length", buf.Length.ToString());
        Response.BinaryWrite(buf);
        Response.End();
        System.IO.File.Delete(filename);
    }

    protected void lnkGroupDownload_Click(object sender, EventArgs e)
    {
        School school = null;
        if (ddlSchools.SelectedValue != "-1")
        {
            school = RegionService.GetSchool(ddlSchools.SelectedValue);
        }
        string filename = UserService.ExtractGroupPhotos(school);
        byte[] buf = FileUtils.LoadFile(filename);
        Response.Clear();
        Response.ContentType = "application/zip";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(System.IO.Path.ChangeExtension(filename, "zip")));
        Response.AddHeader("Content-Length", buf.Length.ToString());
        Response.BinaryWrite(buf);
        Response.End();
        System.IO.File.Delete(filename);
    }

    protected void btnFinal_Click(object sender, EventArgs e)
    {
        if (ddlStatus.SelectedValue != "-1")
        {
            Status status = (Status)Enum.Parse(typeof(Status), ddlStatus.SelectedValue);
            string title = DateTime.Now.Year.ToString() + " ";
            string fileName = string.Empty;
            if (ddlArea.SelectedValue != "-1")
            {
                Area area = RegionService.GetArea(ddlArea.SelectedValue);
                fileName = Path.Combine(Path.GetTempPath(), area.Name + ".pdf");
                title = DateTime.Now.Year.ToString() + " " + area.Name + " Area Finalists";
                ReportService.CreateFinalistReport(fileName, status, area, title);
            }
            else
            {
                Region region = RegionService.GetRegion(ddlRegion.SelectedValue);
                title += region.Name + " Finalists";
                fileName = Path.Combine(Path.GetTempPath(), "report.pdf");
                ReportService.CreateFinalistReport(fileName, status, null, title);
            }
            byte[] buf = FileUtils.LoadFile(fileName);
            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(fileName));
            Response.AddHeader("Content-Length", buf.Length.ToString());
            Response.BinaryWrite(buf);
            Response.End();
            System.IO.File.Delete(fileName);
        }
    }
}
