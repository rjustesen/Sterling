//------------------------------------------------------------------------
// Copyright 2011 Beneficial Life Insurance Company
// This program is an unpublished work fully protected by the United      
// States Copyright laws and is considered a trade secret belonging to   
// the copyright holder -- Beneficial Life Insurance Company
//------------------------------------------------------------------------
#region History
/*
 *  RTJ   09/01/2011   Original development
 */
#endregion

#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SS.Service;
using System.Web.UI;
using System.Web.UI.WebControls;
using SS.Model;
#endregion

/// <summary>
/// Summary description for BaseMaster
/// </summary>
public abstract class BaseMaster : MasterPage
{
    public BaseMaster() { }

    public abstract void ShowErrorMessage(string message);
    public abstract void ShowWarningMessage(string message);
    public abstract void ShowInfoMessage(string message);
    public abstract void SetLoginInfo(User user);

}
