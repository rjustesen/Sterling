using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Service;
using SS.Model;
using log4net;


public partial class ProfileEdit : BasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request["reset"] != null)
            {
                lnkPassword_Click(null, null);
            }
            User user = UserService.GetUser(CurrentUser.Id);
            txtEmail.Text = user.EMail;
            txtFullName.Text = user.FullName;
            txtPhone.Text = user.PhoneNumber;
       
        }
    }

    protected void lnkPassword_Click(object sender, EventArgs e)
    {
        pnlProfie.Visible = false;
        pnlPassword.Visible = true;
        lnkProfile.Visible = true; 
        lnkPassword.Visible = false;
    }

    protected void lnkProfile_Click(object sender, EventArgs e)
    {
        pnlProfie.Visible = true;
        pnlPassword.Visible = false;
        lnkPassword.Visible = true;
        lnkProfile.Visible = false;
    }
    

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx"); 
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            SaveUser();
            ((BaseMaster)MasterPage).ShowInfoMessage("User Profile Updated");
        }     
    }
    protected void btnCancelPassword_Click(object sender, EventArgs e)
    {
          Response.Redirect("~/Default.aspx"); 
    }

    protected void btnSavePassword_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string message;
            if (!UserService.ChangePassword(CurrentUser, txtCurrentPassword.Text, txtNewPassword.Text, out message))
            {
                NewPasswordValidator.ErrorMessage = "Your new Password is not strong enough - " + message;
                NewPasswordValidator.IsValid = false;
            }
            else
            {
                bool redirect = (!CurrentUser.IsApproved);
                CurrentUser.IsApproved = true;
                UserService.UpdateUser(CurrentUser);
                ((BaseMaster)MasterPage).ShowInfoMessage("Your Password was changed");
                if (redirect)
                {
                    Response.Redirect("~/Default.aspx");
                }
                else
                {
                    lnkProfile_Click(null, null);
                }
            }
        }
    }

    private void SaveUser()
    {
        if (Page.IsValid)
        {
            CurrentUser.EMail = txtEmail.Text;
            CurrentUser.FullName = txtFullName.Text;
            CurrentUser.PhoneNumber = txtPhone.Text;

            UserService.UpdateUser(CurrentUser);
        }
        
    }
   
    protected void NewPasswordValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator sender = (CustomValidator)source;
        args.IsValid = true;
        if (String.IsNullOrEmpty(txtNewPassword.Text))
        {
                sender.ErrorMessage = "New password cannot be empty";
                args.IsValid = false;
        }
        string msg = string.Empty;
        if (!UserService.ValidatePassword(txtNewPassword.Text, out msg))
        {
            sender.ErrorMessage = msg;
            args.IsValid = false;
        }
    }
    protected void CurrentPasswordValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator sender = (CustomValidator)source;
        string password = UserService.DecodePassword(CurrentUser.EMail);
        if (password != txtCurrentPassword.Text)
        {
            sender.ErrorMessage = "The Current Password is not correct";
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void ConfirmValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator sender = (CustomValidator)source;
        if (txtNewPassword.Text != txtConfirm.Text)
        {
            sender.ErrorMessage = "New Passwords do not match";
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

    protected void EmailValidator_ServerValidate(object source, ServerValidateEventArgs args)
    {
        CustomValidator sender = (CustomValidator)source;
        SS.Model.User u = UserService.GetUser(txtEmail.Text);
        if (u.EMail != CurrentUser.EMail)
        {
            sender.ErrorMessage = "The email entered has been used, please enter a new email address";
            args.IsValid = false;
        }
        else
        {
            args.IsValid = true;
        }
    }

}
