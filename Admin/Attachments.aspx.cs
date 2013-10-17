using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;

public partial class Admin_Attachments : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (null == CurrentUser || CurrentUser.Role != RoleType.Administrator)
            {
                MasterPage.ShowErrorMessage("You are not Authorized to view this page");
                Response.Redirect("~/Default.sapx");
            }
        }
    }


    private void LoadData()
    {
        //public virtual int Id { get; set; }
        //public virtual string  { get; set; }
        //public virtual string Description { get; set; }
        //public virtual int  { get; set; }
        string where = " where 1=1 ";

        if (ddlCategory.SelectedValue != "-1")
        {
            AttachmentCategory category = (AttachmentCategory)Enum.Parse(typeof(AttachmentCategory), ddlCategory.SelectedValue.ToString());
            where += " and a.Category = " + (int)category;
        }
        if (ddlType.SelectedValue != "-1")
        {
            AttachmentType type = (AttachmentType)Enum.Parse(typeof(AttachmentType), ddlType.SelectedValue.ToString());
            where += " and a.Type = " + (int)type;
        }
        if (!String.IsNullOrEmpty(txtDescription.Text))
        {
            where += " and (a.Description like '%" + txtDescription.Text + "%')";
        }
        if (!String.IsNullOrEmpty(txtName.Text))
        {
            where += " and (a.Name like '%" + txtName.Text + "%')";
        }
        if (!String.IsNullOrEmpty(txtObjectRow.Text))
        {
            where += " and (a.ObjectRowId = " + txtObjectRow.Text + ")";
        }
       // string orderby = (SortDirection == SortDirection.Ascending) ? " order by " + SortColumn + " ASC " : " order by " + SortColumn + " DESC ";
        IList<Attachment> list = PortfolioService.GetAttachments(where);
        gvAttachments.DataSource = list;
        gvAttachments.DataBind();
    }


    protected void gvAttachments_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Attachment attachment = e.Row.DataItem as Attachment;
            
            //if (null != attachment)
            //{
            //    Label lblObject = e.Row.FindControl("lblObject") as Label;
            //    lblObject.Text = attachment.ObjectRowId
            //}
        }
    }

    protected void gvAttachments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if ("Select".Equals(e.CommandName))
        {
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadData();
    }
}
