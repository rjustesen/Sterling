﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Logout : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UserService.LogOff(CurrentUser);
        Session["SessionUser"] = null;
        Response.Redirect("~/Login.aspx");
    }
}
