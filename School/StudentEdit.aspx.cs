using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
using SS.Service;
using Spring.Context;
using GenericServices;
using System.Configuration;


public partial class School_StudentEdit : BasePage
{
    private StateService _stateService;

    private int PortfolioId
    {
        get { return Convert.ToInt32(ViewState["appid"]); }
        set { ViewState["appid"] = value.ToString(); }
    }

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
            PortfolioId = Convert.ToInt32(Request["id"]);
            LoadApplication();
        }
    }


    protected void lnkTrans_Click(object sender, EventArgs e)
    {
        Portfolio portfolio = PortfolioService.GetPortfolio(PortfolioId);
        if (null != portfolio)
        {
            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
            Attachment attachment = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.Transcript);
            if (null == attachment)
            {
                ShowUploadDlg(-1, AttachmentCategory.Transcript, "Upload Transcript", "A certified list of high school grades must be included in your profile. This transcript must be stamped, bearing the offical school seal or principal's signature and MUST include class ranking, cumulative GPA and ACT scores. Proof of ACT scores is required if not on the transcript.");
            }
            else
            {
                ShowUploadDlg(attachment.Id, AttachmentCategory.Transcript, "Upload Transcript", "A certified list of high school grades must be included in your profile. This transcript must be stamped, bearing the offical school seal or principal's signature and MUST include class ranking, cumulative GPA and ACT scores. Proof of ACT scores is required if not on the transcript.");
            }
        }
        else
        {
            ShowUploadDlg(-1, AttachmentCategory.Transcript, "Upload Transcript", "A certified list of high school grades must be included in your profile. This transcript must be stamped, bearing the offical school seal or principal's signature and MUST include class ranking, cumulative GPA and ACT scores. Proof of ACT scores is required if not on the transcript.");
        }
    }

    protected void lnkViewTrans_Click(object sender, EventArgs e)
    {
        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, PortfolioId);
        Attachment attachment = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.Transcript);

        if (null != attachment)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script language=JavaScript id='openit'>");
            sb.Append("window.open('../DocView.aspx?id=" + attachment.Id + "', '', '');");
            sb.Append("</script>");
            if (!ClientScript.IsStartupScriptRegistered("openit"))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "openit", sb.ToString());
            }
        }
    }

    private void LoadTestScores(Portfolio portfolio)
    {
        txtACTEnglish.Text = PortfolioService.GetKeyValue(portfolio, "ACTEnglish").StringValue;
        txtACTMath.Text = PortfolioService.GetKeyValue(portfolio, "ACTMath").StringValue;
        txtACTReading.Text = PortfolioService.GetKeyValue(portfolio, "ACTReading").StringValue;
        txtACTScience.Text = PortfolioService.GetKeyValue(portfolio, "ACTScience").StringValue;
        txtACTComposite.Text = PortfolioService.GetKeyValue(portfolio, "ACTComposite").StringValue;
        txtSATReading.Text = PortfolioService.GetKeyValue(portfolio, "SATReading").StringValue;
        txtSATMath.Text = PortfolioService.GetKeyValue(portfolio, "SATMath").StringValue;
        txtSATWriting.Text = PortfolioService.GetKeyValue(portfolio, "SATWriting").StringValue;
    }

    private void SaveTestScores(Portfolio portfolio)
    {
        if (!string.IsNullOrEmpty(txtACTEnglish.Text))
            PortfolioService.SaveKeyValue(portfolio, "ACTEnglish", txtACTEnglish.Text);
        if (!string.IsNullOrEmpty(txtACTMath.Text))
            PortfolioService.SaveKeyValue(portfolio, "ACTMath", txtACTMath.Text);
        if (!string.IsNullOrEmpty(txtACTReading.Text))
            PortfolioService.SaveKeyValue(portfolio, "ACTReading", txtACTReading.Text);
        if (!string.IsNullOrEmpty(txtACTScience.Text))
            PortfolioService.SaveKeyValue(portfolio, "ACTScience", txtACTScience.Text);
        if (!string.IsNullOrEmpty(txtACTComposite.Text))
            PortfolioService.SaveKeyValue(portfolio, "ACTComposite", txtACTComposite.Text);
        if (!string.IsNullOrEmpty(txtSATReading.Text))
            PortfolioService.SaveKeyValue(portfolio, "SATReading", txtSATReading.Text);
        if (!string.IsNullOrEmpty(txtSATMath.Text))
            PortfolioService.SaveKeyValue(portfolio, "SATMath", txtSATMath.Text);
        if (!string.IsNullOrEmpty(txtSATWriting.Text))
            PortfolioService.SaveKeyValue(portfolio, "SATWriting", txtSATWriting.Text);
        
    }


    private void LoadApplication()
    {
        Portfolio  portfolio = PortfolioService.GetPortfolio(PortfolioId);

       
        ddlState.DataSource = StateService.GetStates();
        ddlState.DataValueField = "StateCode";
        ddlState.DataTextField = "StateName";
        ddlState.DataBind();

        if (null != portfolio)
        {
            ddlCategory.DataMember = "Id";
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataSource = CategoryService.GetCategories(portfolio.School.Area.Region);
            ddlCategory.DataBind();


            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
            
            Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.PersonalPhoto);
            lnkPicture.Visible = true;

            lnkTransUpload.Visible = (attachments.FirstOrDefault(x => x.Category == AttachmentCategory.Transcript) == null);
            lvTranscript.DataSource = attachments.Where(x => x.Category == AttachmentCategory.Transcript); ;
            lvTranscript.DataBind();

            if (null != image)
            {
                picNominee.Visible = true;
                picNominee.ImageUrl = "~/ImageHandler.ashx?id=" + image.Id;
            }

            LoadTestScores(portfolio);
            ddlStatus.SelectedValue = portfolio.Status.ToString();

            ddlCategory.SelectedValue = portfolio.Category.Name;
            ddlCategory.Enabled = false;
            txtFullName.Text = portfolio.User.FullName;
            txtEmail.Text = portfolio.User.EMail;
            txtComment.Text = portfolio.User.Comment;
            txtPhone.Text = portfolio.User.PhoneNumber;
            txtParents.Text = portfolio.Parents;
            txtAddress.Text = portfolio.Address;
            txtCity.Text = portfolio.City;
            ddlState.SelectedValue = portfolio.State;
            txtZip.Text = portfolio.Zip;
            ddlGender.SelectedValue = portfolio.Sex.ToString();
            lblStudentName.Text = "Missing Items for " + portfolio.User.FullName;
            lvItems.DataSource = PortfolioService.GetMissingItems(portfolio);
            lvItems.DataBind();
        }
        else
        {
            ddlCategory.DataMember = "Id";
            ddlCategory.DataTextField = "Name";
            ddlCategory.DataSource = CategoryService.GetCategories(GetSchool().Area.Region);
            ddlCategory.DataBind();

            ddlCategory.Enabled = true;
            ddlStatus.Enabled = false;
            txtFullName.Text = "";
            txtEmail.Text = "";
            txtComment.Text = "";
            txtPhone.Text = "";
            txtParents.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            ddlState.SelectedValue = "UT";
            txtZip.Text = "";
            lnkPicture.Visible = false;
            lnkTransUpload.Visible = false;
        }
        
    }

    protected void valACTSAT_ServerValidate(object source, ServerValidateEventArgs args)
    {
        bool hasACT = (!string.IsNullOrEmpty(txtACTEnglish.Text) &&
                       !string.IsNullOrEmpty(txtACTMath.Text) &&
                       !string.IsNullOrEmpty(txtACTReading.Text) &&
                       !string.IsNullOrEmpty(txtACTScience.Text) &&
                       !string.IsNullOrEmpty(txtACTComposite.Text));
        bool hasSAT = (!string.IsNullOrEmpty(txtSATReading.Text) &&
                       !string.IsNullOrEmpty(txtSATMath.Text) &&
                       !string.IsNullOrEmpty(txtSATWriting.Text));
        if (!hasACT && !hasSAT)
        {
            args.IsValid = false;
        }
    }

    protected void EmailValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator sender = (CustomValidator)source;
        SS.Model.User u = UserService.GetUser(txtEmail.Text);
        Portfolio portfolio = PortfolioService.GetPortfolioByUser(u);
        if (u != null && PortfolioId == -1)
        {
            sender.ErrorMessage = "The email entered has been used, please enter a new email address";
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    private School GetSchool()
    {
        return RegionService.GetSchoolByUser(CurrentUser);
    }

    private bool CategoryExists(string category)
    {
        IList<Portfolio> list = PortfolioService.GetPortfolios(GetSchool());
        if (list.FirstOrDefault(x => x.Category.Name.Equals(category)) != null)
        {
            return true;
        }
        return false;
    }

    private void SavePortfolio()
    {
        UserInput input = null;
        Portfolio p = PortfolioService.GetPortfolio(PortfolioId);
        if (null != p)
        {
            input = UserService.GetUserProfile(p.User.Id);
           
        }
        else
        {
            input = new UserInput();
        }

        input.FullName = txtFullName.Text;
        input.EMail = txtEmail.Text;
        input.Comment = txtComment.Text;
        input.Phone = txtPhone.Text;
        input.Role = RoleType.Nominee;
      
        input.School = GetSchool();
        input.Category = CategoryService.GetCategory(input.School.Area.Region, ddlCategory.SelectedValue);

        MembershipCreateStatus status = UserService.AddNewUser(input);
        if (status == MembershipCreateStatus.Success)
        {
            User user = UserService.GetUser(input.EMail);
            Portfolio portfolio = PortfolioService.GetPortfolioByUser(user);
            portfolio.Parents = txtParents.Text;
            portfolio.Address = txtAddress.Text;
            portfolio.City = txtCity.Text;
            portfolio.Zip = txtZip.Text;
            portfolio.State = ddlState.SelectedValue;
            portfolio.Sex = (Sexes)Enum.Parse(typeof(Sexes), ddlGender.SelectedValue.ToString());

            if (ddlStatus.Enabled)
            {
                portfolio.Status = (Status)Enum.Parse(typeof(Status), ddlStatus.SelectedValue);
            }
            PortfolioService.Save(portfolio);
            SaveTestScores(portfolio);
            if (input.Id <= 0)
            {
                EMailService.SendEmail("Sterling Scholar Registration Request", UserService.FormatTemplate(System.Web.HttpContext.Current.Server.MapPath("~/") + "/assets/NewUserTemplate.html", user), user.EMail, UserService.AdminEmailAddress, null);
            }
          
            PortfolioId = portfolio.Id;
            lnkPicture.Visible = true;
            lnkTransUpload.Visible = true;
        }
    }


    protected void btnSave_Click(object sender, EventArgs e)
    {
        bool redirect = (PortfolioId > 0);
        //if (Accordian1.SelectedIndex == 0)
        //{
        //   // this.Validate("UserEdit");
        //}
        //else
        //{
        //    this.Validate("ScoreEdit");
        //}
        //if (this.IsValid)
        //{
            if (CategoryExists(ddlCategory.SelectedValue) && PortfolioId <= 0)
            {
                labCustomError.Text = "You already have a nominee in the " + ddlCategory.SelectedValue + " category.";
                labCustomError.Visible = true;
                return;
            }
            labCustomError.Text = "";
            labCustomError.Visible = false;
            SavePortfolio();
            if (redirect)
            {
                Response.Redirect("Default.aspx");
            }
       // }
    }

    #region Image methods

    private void ShowUploadDlg(int id, AttachmentCategory category, string title, string instructions)
    {
        Label lblAttachmentCategory = modalDialog.FindControl("lblAttachmentCategory") as Label;
        Label lblUploadInstructions = modalDialog.FindControl("lblUploadInstructions") as Label;
        Label lblAttachmentName = modalDialog.FindControl("lblAttachmentName") as Label;
        Label lblAttachmentId = modalDialog.FindControl("lblAttachmentId") as Label;
        Label lblFormats = modalDialog.FindControl("lblFormats") as Label;
        Label lblTitle = modalDialog.FindControl("lblTitle") as Label;

        lblFormats.Text = FileUtils.SupportedFileFormats();
        lblAttachmentCategory.Text = Enum.GetName(typeof(AttachmentCategory), category);
        lblUploadInstructions.Text = instructions;
        lblAttachmentName.Text = category.ToDescription();
        lblTitle.Text = title;
        lblAttachmentId.Text = id.ToString();

        modalDialog.ShowModal();
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        FileUpload fileUpload = modalDialog.FindControl("txtFileUpload") as FileUpload;
        Label lblAttachmentCategory = modalDialog.FindControl("lblAttachmentCategory") as Label;
        Label lblUploadInstructions = modalDialog.FindControl("lblUploadInstructions") as Label;
        Label lblAttachmentName = modalDialog.FindControl("lblAttachmentName") as Label;
        Label lblAttachmentId = modalDialog.FindControl("lblAttachmentId") as Label;

        AttachmentCategory category = (AttachmentCategory)Enum.Parse(typeof(AttachmentCategory), lblAttachmentCategory.Text);
        this.Validate("Upload");
        
        if (this.IsValid && null != fileUpload && fileUpload.HasFile && FileUtils.IsValidFile(fileUpload.FileName))
        {
            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, PortfolioId);
            int id = Convert.ToInt32(lblAttachmentId.Text);
            Attachment attachment = attachments.FirstOrDefault(x => x.Id == id);
            if (null == attachment)
            {
                attachment = new Attachment();
                attachment.Type = AttachmentType.Portfolio;
                attachment.ObjectRowId = PortfolioId;
            }
            attachment.Category = category;
            attachment.Description = lblAttachmentName.Text;
            attachment.Name = fileUpload.FileName;

            using (BinaryReader reader = new BinaryReader(fileUpload.FileContent))
            {
                byte[] buf = reader.ReadBytes((int)fileUpload.FileContent.Length);
                reader.Close();
                fileUpload.FileContent.Close();
                if (FileUtils.IsImageFile(attachment.Name))
                {
                    attachment.Data = buf;// ImageUtils.NormalizeImage(buf);
                }
                else
                {
                    attachment.Data = buf;
                }
            }
            PortfolioService.SaveAttachment(attachment);
            modalDialog.HideModal();
            LoadApplication();
        }

    }


    protected void validUpload_ServerValidate(object source, ServerValidateEventArgs args)
    {
        FileUpload fileUpload = modalDialog.FindControl("txtFileUpload") as FileUpload;

        if (null == fileUpload || !fileUpload.HasFile || !FileUtils.IsValidFile(fileUpload.FileName))
        {
            args.IsValid = false;
        }
    }


    protected void btnClose_Click(object source, EventArgs e)
    {
        modalDialog.HideModal();
    }

    protected void lnkPicture_Click(object sender, EventArgs e)
    {
        Portfolio portfolio = PortfolioService.GetPortfolio(PortfolioId);
        if (null != portfolio)
        {
            IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
            Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.PersonalPhoto);
            if (null == image)
            {
                ShowUploadDlg(-1, AttachmentCategory.PersonalPhoto, "Upload Photo", "Personal photos must be in JPG or PNG format and should be no larger than 640 x 480 pixels. Images will be automatically resized to these dimensions but we recommend resizing your images before you upload them to avoid any potential problems.");
            }
            else
            {
                ShowUploadDlg(image.Id, AttachmentCategory.PersonalPhoto, "Upload Photo", "Personal photos must be in JPG or PNG format and should be no larger than 640 x 480 pixels. Images will be automatically resized to these dimensions but we recommend resizing your images before you upload them to avoid any potential problems.");
            }
        }
        else
        {
            ShowUploadDlg(-1, AttachmentCategory.PersonalPhoto, "Upload Photo", "Personal photos must be in JPG or PNG format and should be no larger than 640 x 480 pixels. Images will be automatically resized to these dimensions but we recommend resizing your images before you upload them to avoid any potential problems.");
        }
    }
    #endregion

    protected void btnDelAttachment_Command(Object sender, CommandEventArgs e)
    {
        if (null != e.CommandArgument)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Attachment attachment = PortfolioService.GetAttachment(id);
            PortfolioService.DeleteAttachment(attachment);
            LoadApplication();
        }
    }
    
    protected void valTrans_ServerValidate(object source, ServerValidateEventArgs args)
    {
        Portfolio portfolio = PortfolioService.GetPortfolio(PortfolioId);
        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
        if (attachments.FirstOrDefault(x => x.Category == AttachmentCategory.Transcript) == null)
        {
            args.IsValid = false;
        }
    }


}
