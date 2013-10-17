using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;
using Spring.Context;
using GenericServices;


public partial class Admin_Default : BasePage
{
    private StateService _stateService;


    public StateService StateService
    {
        get
        {
            if (null == _stateService)
            {
                IApplicationContext ctx = Spring.Context.Support.ContextRegistry.GetContext();
                _stateService = ctx["StateService"] as StateService;
            }
            return _stateService;
        }
        set { _stateService = value; }
    }




    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (null == CurrentUser || (CurrentUser.Role != RoleType.Coordinator && CurrentUser.Role != RoleType.Principal))
            {
                MasterPage.ShowErrorMessage("You are not Authorized to view this page");
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                lblSchool.Text = string.Format("Nominees for {0:s} High School", GetSchool().Name);
                LoadApplications();
            }
        }
    }


    private School GetSchool()
    {
        return RegionService.GetSchoolByUser(CurrentUser);
    }


    /// <summary>
    /// Load all applications for this school
    /// </summary>
    private void LoadApplications()
    {
        School school = GetSchool();
        IList<Portfolio> list = PortfolioService.GetPortfolios(school);
        //lnkSubmit.Visible = (list.FirstOrDefault(x => x.Status != ApplicationStatus.Certified) == null);
        gvNominees.DataSource = list;
        gvNominees.DataBind();

        IList<Category> missing = new List<Category>();
        IList<Category> categories = CategoryService.GetCategories(school.Area.Region);

        foreach (Category c in categories)
        {
            if (list.FirstOrDefault(x => x.Category.Name.Equals(c.Name)) == null)
            {
                missing.Add(c);
            }
        }
        lvMissingCats.DataSource = missing;
        lvMissingCats.DataBind();

        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.User, CurrentUser.Id);
        Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.GroupPhoto);
        if (null != image)
        {
            imgPhoto.Visible = true;
            imgPhoto.ImageUrl = "~/ImageHandler.ashx?id=" + image.Id;
        }
        IList<KeyValue> keyValues = PortfolioService.GetKeyValues(ObjectTypes.User, CurrentUser.Id);
        var val = keyValues.FirstOrDefault(x => "GroupCaption".Equals(x.KeyName) && x.Type == KeyValueType.String);
        if (val != null)
        {
            txtCaption.Text = val.StringValue;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        IList<KeyValue> keyValues = PortfolioService.GetKeyValues(ObjectTypes.User, CurrentUser.Id);
        var val = keyValues.FirstOrDefault(x => "GroupCaption".Equals(x.KeyName) && x.Type == KeyValueType.String);
        if (val == null)
        {
            val = new KeyValue();
            val.Type = KeyValueType.String;
            val.KeyName = "GroupCaption";
            val.ObjectRowId = CurrentUser.Id;
            val.ObjectType = ObjectTypes.User;
            
        }
        if (txtCaption.Text.Length >= 999)
        {
            val.StringValue = txtCaption.Text.Substring(0, 999);
        }
        else
        {
            val.StringValue = txtCaption.Text;
        }
        PortfolioService.SaveKeyValue(val);
    }

    protected void gvNominees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Portfolio portfolio = e.Row.DataItem as Portfolio;
            if (null != portfolio)
            {
                Label labName = e.Row.FindControl("labName") as Label;
                Label labEmail = e.Row.FindControl("labEmail") as Label;
                Label labPhone = e.Row.FindControl("labPhone") as Label;
                Label labCategory = e.Row.FindControl("labCategory") as Label;


                LinkButton btnEdit = e.Row.FindControl("btnEdit") as LinkButton;
                LinkButton btnDelete = e.Row.FindControl("btnDelete") as LinkButton;
                LinkButton btnView = e.Row.FindControl("btnView") as LinkButton;
                LinkButton btnCertify = e.Row.FindControl("btnCertify") as LinkButton;
                LinkButton btnStatus = e.Row.FindControl("btnStatus") as LinkButton;

                btnStatus.Text = portfolio.Status.ToString();
                btnStatus.Attributes.Add("onmouseover", "tooltip.show('" + GetStatus(portfolio) + "');");
                btnStatus.Attributes.Add("onmouseout", "tooltip.hide();");

                btnDelete.Attributes.Add("onclick", "return confirm('Are you sure you want to delete this Nominee?');");
                btnDelete.CommandArgument = portfolio.Id.ToString();
                btnView.CommandArgument = portfolio.Id.ToString();

                btnCertify.Visible = (portfolio.Status == Status.Submited);
                btnCertify.CommandArgument = portfolio.Id.ToString();
                btnStatus.CommandArgument = portfolio.Id.ToString();
                btnEdit.CommandArgument = portfolio.Id.ToString();
                if (portfolio.Status < Status.Area)
                {
                    btnEdit.Visible = (portfolio.Status != Status.Complete);
                }
                else
                {
                    btnEdit.Visible = false;
                }

                labName.Text = portfolio.User.FullName;
                labEmail.Text = portfolio.User.EMail;
                labPhone.Text = portfolio.User.PhoneNumber;
                labCategory.Text = portfolio.Category.Name;

            }
        }
    }

    private string GetStatus(Portfolio portfolio)
    {
        StringBuilder html = new StringBuilder("<table><tr><th colspan=\"2\">Missing Items (Things that need to be competed)</th></tr>");
        IList<MissingItem> items = PortfolioService.GetMissingItems(portfolio);
        if (items.Count == 0)
        {
            html.Append("<tr><td>All Items Completed</td></tr>");
        }
        foreach (MissingItem i in items)
        {
            html.Append(string.Format("<tr><td>{0:s}</td><td>{1:s}</tr>", i.Name, i.Description));
        }
        html.Append("</table>");
        return html.ToString();
    }
    /// <summary>
    /// Link to page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvNominees_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(gvNominees.DataKeys[0].Values[0]);
        Portfolio portfolio = null;

        if (!"".Equals(e.CommandArgument))
        {
            id = Convert.ToInt32(e.CommandArgument);
            portfolio = PortfolioService.GetPortfolio(id);
        }
        if ("Select".Equals(e.CommandName))
        {
            Response.Redirect("StudentEdit.aspx?id=" + id.ToString());
        }
        if ("DeleteUser".Equals(e.CommandName))
        {
            if (portfolio != null)
            {
                PortfolioService.Delete(portfolio);
                UserService.DeleteUser(portfolio.User);
                LoadApplications();
            }
        }
        if ("ViewPortfolio".Equals(e.CommandName))
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=JavaScript id='openit'>");
            sb.Append("window.open('../ReportView.aspx?report=profile&id=" + portfolio.Id + "', '', '');");
            sb.Append("</script>");
            if (!ClientScript.IsStartupScriptRegistered("openit"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "openit", sb.ToString());
            }
        }
        if ("CertifyStudent".Equals(e.CommandName))
        {
            if (portfolio != null)
            {
                Label lblName = mdlDlgPrincipal.FindControl("lblName") as Label;
                Label lblId = mdlDlgPrincipal.FindControl("lblId") as Label;
                Label lblSchool = mdlDlgPrincipal.FindControl("lblSchool") as Label;
                Label lblCategory = mdlDlgPrincipal.FindControl("lblCategory") as Label;
                lblId.Text = id.ToString();
                lblName.Text = portfolio.User.FullName;
                lblSchool.Text = portfolio.School.Name;
                lblCategory.Text = portfolio.Category.Name;
                mdlDlgPrincipal.ShowModal();
            }
        }
        if ("CheckStatus".Equals(e.CommandName))
        {
            if (portfolio.Status == Status.Certified)
            {
                Label lblId = mdlDlgPrincipal.FindControl("lblId") as Label;
                lblId.Text = portfolio.Id.ToString();
                btnReport_Click(null, null);
            }

        }
    }



    protected void lnkNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentEdit.aspx?id=-1");
    }

    protected void btnOk_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            mdlDlgPrincipal.HideModal();
            Label lblId = mdlDlgPrincipal.FindControl("lblId") as Label;
            Portfolio app = PortfolioService.GetPortfolio(Convert.ToInt32(lblId.Text));
            app.Status = Status.Certified;
            PortfolioService.Save(app);
            btnReport_Click(null, null);
            LoadApplications();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        mdlDlgPrincipal.HideModal();
    }

    protected void btnDlgClose_Click(object sender, EventArgs e)
    {
        modalImageDlg.HideModal();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        FileUpload fileUpload = modalImageDlg.FindControl("txtFileUpload") as FileUpload;
        if (null != fileUpload && fileUpload.HasFile)
        {
            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.User, CurrentUser.Id);
            Attachment image = null;
            if (null != attachments && attachments.Count > 0)
            {
                image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.GroupPhoto);
            }
            if (null == image)
            {
                image = new Attachment();
                image.Category = AttachmentCategory.GroupPhoto;
                image.Type = AttachmentType.User;
                image.ObjectRowId = CurrentUser.Id;
                image.Description = "Group Picture";
            }
            image.Name = fileUpload.FileName;
            using (BinaryReader reader = new BinaryReader(fileUpload.FileContent))
            {
                byte[] buf = reader.ReadBytes((int)fileUpload.FileContent.Length);
                fileUpload.FileContent.Close();
                image.Data = buf;// ImageUtils.NormalizeImage(buf);
                reader.Close();
            }
            PortfolioService.SaveAttachment(image);
            imgPhoto.Visible = true;
            imgPhoto.ImageUrl = "~/ImageHandler.ashx?id=" + image.Id;
        }
        modalImageDlg.HideModal();
    }

    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        IList<Portfolio> list = PortfolioService.GetPortfolios(GetSchool());
        foreach (Portfolio app in list)
        {
            if (app.Status == Status.Certified)
            {
                app.Status = Status.Complete;
                PortfolioService.Save(app);
            }
        }
        LoadApplications();
    }

    protected void btnImage_Click(object sender, EventArgs e)
    {
        modalImageDlg.ShowModal();
    }



    protected void btnReport_Click(object sender, EventArgs e)
    {
        Label lblId = mdlDlgPrincipal.FindControl("lblId") as Label;

        StringBuilder sb = new StringBuilder();
        sb.Append("<script language=JavaScript id='openit'>");
        sb.Append("window.open('../ReportView.aspx?report=principal&id=" + lblId.Text + "', '', '');");
        sb.Append("</script>");
        if (!ClientScript.IsStartupScriptRegistered("openit"))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "openit", sb.ToString());
        }
    }

    protected void btnNominees_Click(object sender, EventArgs e)
    {
        Label lblId = mdlDlgPrincipal.FindControl("lblId") as Label;

        StringBuilder sb = new StringBuilder();
        sb.Append("<script language=JavaScript id='openit'>");
        sb.Append("window.open('../ReportView.aspx?report=nominees&id=" + GetSchool().Id + "', '', '');");
        sb.Append("</script>");
        if (!ClientScript.IsStartupScriptRegistered("openit"))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "openit", sb.ToString());
        }
    }

    protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        CheckBox chkTranscript = mdlDlgPrincipal.FindControl("chkTranscript") as CheckBox;
        CheckBox chkSchool = mdlDlgPrincipal.FindControl("chkSchool") as CheckBox;
        CheckBox chkForms = mdlDlgPrincipal.FindControl("chkForms") as CheckBox;
        CheckBox chkTests = mdlDlgPrincipal.FindControl("chkTests") as CheckBox;
        e.IsValid = (chkTranscript.Checked && chkSchool.Checked && chkForms.Checked && chkTests.Checked);
    }
}
