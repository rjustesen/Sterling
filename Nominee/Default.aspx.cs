using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using SS.Model;
using SS.Service;
using GenericServices;
using Spring.Context;

public partial class Nominee_Default :  BasePage
{


    private StateService _stateService;

    private IList<MissingItem> MissingItems
    {
        get { return ViewState["_missingItems"] as IList<MissingItem>; }
        set 
        { 
            ViewState["_missingItems"] = value;
            lnkSubmit.Visible = (value.FirstOrDefault(x => x.Required) == null) && (GetPortfolio().Status == Status.Incomplete);
        }
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
            if (null == CurrentUser || CurrentUser.Role != RoleType.Nominee)
            {
                MasterPage.ShowErrorMessage("You are not Authorized to view this page");
            }
            else
            {
                LoadPortfolio();
                ddlState.DataSource = StateService.GetStates();
                ddlState.DataValueField = "StateCode";
                ddlState.DataTextField = "StateName";
                ddlState.DataBind();
            }
        }
        else
        {
            string eventTarget = this.Request.Params.Get("__EVENTTARGET");
            if (eventTarget.Equals(AccordianControl.ClientID + "_AccordionExtender"))
            {
                string parameter = Request["__EVENTARGUMENT"];
                int index = Convert.ToInt32(parameter);
                SavePortfolio(index);
            }
        }
    }
    //   using System.Text.RegularExpressions;  static bool AllNumeric(string inputString) { return Regex.IsMatch(inputString, @"^\d+$"); }  

    public string GetImage(object item)
    {
        if (item != null)
        {
            int index = Convert.ToInt32(Convert.ToString(item));

            var missing = MissingItems.FirstOrDefault(x => x.Index == index);
            if ("accordionHeaderSelected".Equals(AccordianControl.Panes[index].HeaderCssClass))
            {
                if (null == missing)
                {
                    return "../assets/images/checked.png";
                }
                else
                {
                    return "../assets/images/unchecked.png";
                }
            }
            else
            {
                if (null == missing)
                {
                    return "../assets/images/checked.png";
                }
                else
                {
                    return "../assets/images/unchecked.png";
                }
            }
        }
        return "";
    }

    
    private void LoadPortfolio(Portfolio portfolio)
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

    private void SavePortfolio(Portfolio portfolio)
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
        PortfolioService.SavePortfolio(portfolio);
    }

    private void LoadPortfolio()
    {
        Portfolio portfolio = GetPortfolio();
        MissingItems = PortfolioService.GetMissingItems(portfolio);
        lblName.Text = CurrentUser.FullName;

        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
        var letters = from a in attachments
                      where a.Category == AttachmentCategory.LetterOfRecomendation
                      select a;

        lnkLetter.Visible = !letters.Any();
        lvLetters.DataSource = letters;
        lvLetters.DataBind();

        var docs = from a in attachments
                   where a.Category == AttachmentCategory.CategoryDescription
                   select a;

        lnkCatDocsUpload.Visible = docs.Count() < 2;

        lvCatDocs.DataSource = docs;
        lvCatDocs.DataBind();

        docs = from a in attachments
               where a.Category == AttachmentCategory.LeadershipDescription
               select a;

        lnkLeadership.Visible = docs.Count() < 2;

        lvLeadership.DataSource = docs;
        lvLeadership.DataBind();

        docs = from a in attachments
               where a.Category == AttachmentCategory.CitizenshipDescription
               select a;

        lnkCitizenship.Visible = docs.Count() < 2;
        lvCitizenship.DataSource = docs;
        lvCitizenship.DataBind();

        Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.PersonalPhoto);
        if (null != image && AccordianControl.SelectedIndex == 0)
        {
            picNominee.Visible = true;
            picNominee.ImageUrl = "~/ImageHandler.ashx?id=" + image.Id;
        }
        else
        {
            picNominee.Visible = false;
            picNominee.ImageUrl = "";
        }

        lvTranscript.DataSource = attachments.Where(x => x.Category == AttachmentCategory.Transcript);
        lvTranscript.DataBind();

        lblSchoolEdit.Text = portfolio.School.Name;
        lblCategoryEdit.Text = portfolio.Category.Name;
        txtName.Text = CurrentUser.FullName;
        txtParents.Text = portfolio.Parents;
        txtAddress.Text = portfolio.Address;
        txtCity.Text = portfolio.City;
        ddlState.SelectedValue = portfolio.State;
        txtZip.Text = portfolio.Zip;
        txtPhone.Text = portfolio.User.PhoneNumber;
        ddlGender.SelectedValue = portfolio.Sex.ToString();
        LoadPortfolio(portfolio);


        IEnumerable<PortfolioItem> catItems = portfolio.Items.Where(x => x.Type == ItemType.Category);
        foreach (PortfolioItem item in catItems)
        {
            TextBox tb = AccordionPane4.FindControl("txtCategory" + item.ItemIndex.ToString()) as TextBox;
            if (tb != null)
            {
                tb.Text = item.Text;
            }
        }

        PortfolioItem i = portfolio.Items.FirstOrDefault(x => x.Type == ItemType.CategoryDescription);
        if (i != null)
        {
            txtCategoryDescription.Text = i.Text;
        }

        IEnumerable<PortfolioItem> leaderItems = portfolio.Items.Where(x => x.Type == ItemType.Leadership);
        foreach (PortfolioItem item in leaderItems)
        {
            TextBox tb = AccordionPane6.FindControl("txtLeadership" + item.ItemIndex.ToString()) as TextBox;
            if (tb != null)
            {
                tb.Text = item.Text;
            }
        }
        i = portfolio.Items.FirstOrDefault(x => x.Type == ItemType.LeadershipDescription);
        if (i != null)
        {
            txtLeadershipDescription.Text = i.Text;
        }

        IEnumerable<PortfolioItem> citizenItems = portfolio.Items.Where(x => x.Type == ItemType.Citizenship);

        foreach (PortfolioItem item in citizenItems)
        {
            TextBox tb = AccordionPane8.FindControl("txtCitizenship" + item.ItemIndex.ToString()) as TextBox;
            if (tb != null)
            {
                tb.Text = item.Text;
            }
        }

        i = portfolio.Items.FirstOrDefault(x => x.Type == ItemType.CitizenshipDescription);
        if (i != null)
        {
            txtCitizenshipDescription.Text = i.Text;
        }

        i = portfolio.Items.FirstOrDefault(x => x.Type == ItemType.LifeEnrichment);
        if (i != null)
        {
            txtLifeEnrichment.Text = i.Text;
        }

        i = portfolio.Items.FirstOrDefault(x => x.Type == ItemType.UniqueQualities);
        if (i != null)
        {
            txtUniqueQualities.Text = i.Text;
        }

        if (null != portfolio)
        {
            IList<Attachment> list = PortfolioService.GetAttachments(AttachmentCategory.Document, AttachmentType.Portfolio, portfolio.Id);
            lvAttachDocs.DataSource = list;
            lvAttachDocs.DataBind();
            btnUploadDocs.Visible = (list.Count == 0);
            list = PortfolioService.GetAttachments(AttachmentCategory.Image, AttachmentType.Portfolio, portfolio.Id);
            btnImage.Visible = (list.Count == 0);
            lvImage.DataSource = list;
            lvImage.DataBind();
            list = PortfolioService.GetAttachments(AttachmentCategory.Media, AttachmentType.Portfolio, portfolio.Id);
            btnMedia.Visible = (list.Count == 0);
            lvMedia.DataSource = list;
            lvMedia.DataBind();
            list = PortfolioService.GetAttachments(AttachmentCategory.Url, AttachmentType.Portfolio, portfolio.Id);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

            var attachment = list.FirstOrDefault(x => "Document".Equals(x.Name));
            if (null != attachment)
            {
                txtDocDescription.Text = attachment.Description;
                attachment = PortfolioService.GetAttachment(attachment.Id);
                txtDocLink.Text = encoding.GetString(attachment.Data); 
            }
            attachment = list.FirstOrDefault(x => "Image".Equals(x.Name));
            if (null != attachment)
            {
                txtImageDescription.Text = attachment.Description;
                attachment = PortfolioService.GetAttachment(attachment.Id);
                txtImageLink.Text = encoding.GetString(attachment.Data); 
            }
            attachment = list.FirstOrDefault(x => "Media".Equals(x.Name));
            if (null != attachment)
            {
                txtMediaDescription.Text = attachment.Description;
                attachment = PortfolioService.GetAttachment(attachment.Id);
                txtMediaLink.Text = encoding.GetString(attachment.Data); 
            }
        }
        else
        {
            PortfolioService.SavePortfolio(portfolio);
        }
        if (portfolio.Status != Status.Incomplete)
        {
            DisableButtons(Page.Controls);
        }
    }

    private void DisableButtons(ControlCollection controlCollection)
    {     
        foreach (Control control in controlCollection)     
        {
            if (control is Button)
                ((Button)control).Visible = false;
            if (control is LinkButton && !control.ID.Equals("lnkView"))
                ((LinkButton)control).Visible = false;    
            if (control.HasControls())
                DisableButtons(control.Controls);     
        } 
    }


    private Portfolio GetPortfolio()
    {
        return PortfolioService.GetPortfolioByUser(CurrentUser);
    }

    private void SavePortfolio(int index)
    {
        Portfolio portfolio = GetPortfolio();
        switch (index)
        {
            case 0:
                CurrentUser.FullName = txtName.Text;
                portfolio.Parents = txtParents.Text;
                portfolio.Address = txtAddress.Text;
                portfolio.City = txtCity.Text;
                portfolio.State = ddlState.SelectedValue;
                portfolio.Zip = txtZip.Text;
                CurrentUser.PhoneNumber = txtPhone.Text;
                portfolio.Sex = (Sexes)Enum.Parse(typeof(Sexes), ddlGender.SelectedValue.ToString());
                UserService.SaveUser(CurrentUser);
                break;
            case 1:
                SavePortfolio(portfolio);
                break;
            case 2:
                break;
            case 3:
                for (int i = 0; i < 6; i++)
                {
                    TextBox tb = AccordianControl.FindControl("txtCategory" + i.ToString()) as TextBox;
                    if (tb != null && !string.IsNullOrEmpty(tb.Text))
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.Category, i);
                        item.Text = tb.Text;
                    }
                    else
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.Category, i);
                        if (null != item)
                        {
                            PortfolioService.RemovePortfolioItem(portfolio, ItemType.Category, i);
                        }
                    }
                }
                break;
            case 4:
                {
                    if (!String.IsNullOrEmpty(txtCategoryDescription.Text))
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.CategoryDescription, 0);
                        item.Text = txtCategoryDescription.Text;
                    }
                    else
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.CategoryDescription, 0);
                        if (null != item)
                        {
                            PortfolioService.RemovePortfolioItem(portfolio, ItemType.CategoryDescription, 0);
                        }
                    }
                }
                break;
            case 5:
                for (int i = 0; i < 6; i++)
                {
                    TextBox tb = AccordianControl.FindControl("txtLeadership" + i.ToString()) as TextBox;
                    if (tb != null && !string.IsNullOrEmpty(tb.Text))
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.Leadership, i);
                        item.Text = tb.Text;
                    }
                    else
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.Leadership, i);
                        if (null != item)
                        {
                            PortfolioService.RemovePortfolioItem(portfolio, ItemType.Leadership, i);
                        }
                    }
                }
                break;
            case 6:
                if (!String.IsNullOrEmpty(txtLeadershipDescription.Text))
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.LeadershipDescription, 0);
                    item.Text = txtLeadershipDescription.Text;
                }
                else
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.LeadershipDescription, 0);
                    if (null != item)
                    {
                        PortfolioService.RemovePortfolioItem(portfolio, ItemType.LeadershipDescription, 0);
                    }
                }
                break;
            case 7:
                for (int i = 0; i < 6; i++)
                {
                    TextBox tb = AccordianControl.FindControl("txtCitizenship" + i.ToString()) as TextBox;
                    if (tb != null && !string.IsNullOrEmpty(tb.Text))
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.Citizenship, i);
                        item.Text = tb.Text;
                    }
                    else
                    {
                        PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.Citizenship, i);
                        if (null != item)
                        {
                            PortfolioService.RemovePortfolioItem(portfolio, ItemType.Citizenship, i);
                        }
                    }
                }
                break;
            case 8:
                if (!String.IsNullOrEmpty(txtCitizenshipDescription.Text))
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.CitizenshipDescription, 0);
                    item.Text = txtCitizenshipDescription.Text;
                }
                else
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.CitizenshipDescription, 0);
                    if (null != item)
                    {
                        PortfolioService.RemovePortfolioItem(portfolio, ItemType.CitizenshipDescription, 0);
                    }
                }
                break;
            case 9:
                if (!String.IsNullOrEmpty(txtUniqueQualities.Text))
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.UniqueQualities, 0);
                    item.Text = txtUniqueQualities.Text;
                }
                else
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.UniqueQualities, 0);
                    if (null != item)
                    {
                        PortfolioService.RemovePortfolioItem(portfolio, ItemType.UniqueQualities, 0);
                    }
                }
                break;
            case 10:

                if (!String.IsNullOrEmpty(txtLifeEnrichment.Text))
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.LifeEnrichment, 0);
                    item.Text = txtLifeEnrichment.Text;
                }
                else
                {
                    PortfolioItem item = PortfolioService.GetPortfolioItem(portfolio, ItemType.LifeEnrichment, 0);
                    if (null != item)
                    {
                        PortfolioService.RemovePortfolioItem(portfolio, ItemType.LifeEnrichment, 0);
                    }
                }
                break;
            case 11:
                {
                    IList<Attachment> list = PortfolioService.GetAttachments(AttachmentCategory.Url, AttachmentType.Portfolio, portfolio.Id);
                    var attachment = list.FirstOrDefault(x => "Document".Equals(x.Name));
                    if (!string.IsNullOrEmpty(txtDocLink.Text))
                    {
                        SaveUrl(attachment, "Document",txtDocDescription.Text, txtDocLink.Text, portfolio);
                    }
                    else if (null != attachment)
                    {
                        PortfolioService.DeleteAttachment(attachment);
                    }
                    attachment = list.FirstOrDefault(x => "Image".Equals(x.Name));
                    if (!string.IsNullOrEmpty(txtImageLink.Text))
                    {
                        SaveUrl(attachment, "Image", txtImageDescription.Text, txtImageLink.Text, portfolio);
                    }
                    else if (null != attachment)
                    {
                        PortfolioService.DeleteAttachment(attachment);
                    }
                    attachment = list.FirstOrDefault(x => "Media".Equals(x.Name));
                    if (!string.IsNullOrEmpty(txtMediaLink.Text))
                    {
                        SaveUrl(attachment, "Media", txtMediaDescription.Text, txtMediaLink.Text, portfolio);
                    }
                    else if (null != attachment)
                    {
                        PortfolioService.DeleteAttachment(attachment);
                    }
                    break;
                }
            default:
                break;
        }
        PortfolioService.Save(portfolio);
    }

    private void SaveUrl(Attachment attachment, string name, string description, string url, Portfolio portfolio)
    {
        if (null == attachment)
        {
            attachment = new Attachment();
        }
        attachment.Name = name;
        attachment.Category = AttachmentCategory.Url;
        attachment.Type = AttachmentType.Portfolio;
        attachment.ObjectRowId = portfolio.Id;
        attachment.Description = description;
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        Byte[] bytes = encoding.GetBytes(url);
        attachment.Data = bytes;
        PortfolioService.SaveAttachment(attachment);
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

    protected void lnkLetter_Click(object sender, EventArgs e)
    {
        Portfolio portfolio = GetPortfolio();
        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
        Attachment image = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.LetterOfRecomendation);
        if (null == image)
        {
            ShowUploadDlg(-1, AttachmentCategory.LetterOfRecomendation, "Upload Letter of Recommendation", "Include a Letter of Recommendation from any teacher or instructor in a supported format.");
        }
        else
        {
            ShowUploadDlg(image.Id, AttachmentCategory.LetterOfRecomendation, "Upload Letter of Recommendation", "Include a Letter of Recommendation from any teacher or instructor in a supported format.");
        }
    }

    protected void btnDelAttachment_Command(Object sender, CommandEventArgs e)
    {
        if (null != e.CommandArgument)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Attachment attachment = PortfolioService.GetAttachment(id);
            PortfolioService.DeleteAttachment(attachment);
            LoadPortfolio();
        }
    }

    protected void cmdUpload_Command(Object sender, CommandEventArgs e)
    {
        if (null != e.CommandArgument)
        {
            AttachmentCategory category = (AttachmentCategory)Enum.Parse(typeof(AttachmentCategory), e.CommandArgument.ToString());
            ShowUploadDlg(-1, category, "Upload Supporting Attachment", "You can upload a supporting attachment for each description.");
        }
    }

    protected void lnkPicture_Click(object sender, EventArgs e)
    {
        Portfolio portfolio = GetPortfolio();
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

    protected void lnkTrans_Click(object sender, EventArgs e)
    {
        Portfolio portfolio = GetPortfolio();
        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, portfolio.Id);
        Attachment attachment = attachments.FirstOrDefault(x => x.Category == AttachmentCategory.Transcript);
        if (null == attachment)
        {
            ShowUploadDlg(-1, AttachmentCategory.Transcript, "Upload Transcript", "A certified list of high school grades must be included in your profile. This transcript must be stamped, bearing the official school seal or principal's signature and MUST include class ranking, cumulative GPA and ACT scores. Proof of ACT scores is required if not on the transcript.");
        }
        else
        {
            ShowUploadDlg(attachment.Id, AttachmentCategory.Transcript, "Upload Transcript", "A certified list of high school grades must be included in your profile. This transcript must be stamped, bearing the offical school seal or principal's signature and MUST include class ranking, cumulative GPA and ACT scores. Proof of ACT scores is required if not on the transcript.");
        }
    }



    protected void lnkView_Click(object sender, EventArgs e)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script language=JavaScript id='openit'>");
        sb.Append("window.open('../ReportView.aspx?report=profile&id=" + GetPortfolio().Id + "', '', '');");
        sb.Append("</script>");
        if (!ClientScript.IsStartupScriptRegistered("openit"))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "openit", sb.ToString());
        }
    }


    

    protected void lnkViewTrans_Click(object sender, EventArgs e)
    {
        Portfolio application = PortfolioService.GetPortfolioByUser(CurrentUser);
        IList<Attachment> attachments = PortfolioService.GetAttachments(AttachmentType.Portfolio, application.Id);
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        string arg = String.Empty;
        if (sender is Button && !String.IsNullOrEmpty(((Button)sender).CommandArgument))
        {
            arg = ((Button)sender).CommandArgument;
        }
        if (!String.IsNullOrEmpty(arg))
        {
            this.Validate(arg);
        }
        else
        {
            this.Validate();
        }
        if (this.IsValid)
        {
            SavePortfolio(AccordianControl.SelectedIndex);
            MissingItems = PortfolioService.GetMissingItems(GetPortfolio());
            if (AccordianControl.SelectedIndex < AccordianControl.Panes.Count)
            {
                AccordianControl.SelectedIndex += 1;
            }
            else
            {
                AccordianControl.SelectedIndex = 0;
            }
        }
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
            int id = Convert.ToInt32(lblAttachmentId.Text);
            Attachment attachment = null;
            if (id > 0)
            {
                attachment = PortfolioService.GetAttachment(id);
            }
            if (id <= 0 || null == attachment)
            {
                attachment = new Attachment();
                Portfolio portfolio = GetPortfolio();
                if (category == AttachmentCategory.Image || category == AttachmentCategory.Document || category == AttachmentCategory.Media)
                {
                    attachment.Type = AttachmentType.Portfolio;
                    attachment.ObjectRowId = portfolio.Id;
                }
                else
                {
                    attachment.Type = AttachmentType.Portfolio;
                    attachment.ObjectRowId = GetPortfolio().Id;
                }
                attachment.Category = category;
            }
            
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
            LoadPortfolio();
        }

    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        modalDialog.HideModal();
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        this.Validate();
        if (this.IsValid)
        {
            SavePortfolio(AccordianControl.SelectedIndex);
            Portfolio portfolio = GetPortfolio();
            portfolio.Status = Status.Submited;
            PortfolioService.Save(portfolio);
            lnkSubmit.Visible = false;
            ClientHelper.ClientMessage(this.Page, "Information", "Portfolio Saved and Submitted. You must also print, sign and submit the Parent/Student Declaration Form before your application is complete.");
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



    #endregion

}
