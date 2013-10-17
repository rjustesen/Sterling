using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for GridHelper
/// </summary>
public class GridHelper
{
    public GridHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void AddSortImage(DataControlFieldHeaderCell obj, SortDirection sortDirection)
    {
        // Create the sorting image based on the sort direction.
        System.Web.UI.WebControls.Image sortImage = new System.Web.UI.WebControls.Image();
        if (SortDirection.Ascending == sortDirection)
        {
            sortImage.ImageUrl = "~/assets/images/ascred.gif";
            sortImage.AlternateText = "Ascending Order";
        }
        else
        {
            sortImage.ImageUrl = "~/assets/images/descred.gif";
            sortImage.AlternateText = "Descending Order";
        }
        // Add the image to the appropriate header cell.
        obj.Controls.Add(sortImage);
    }
}
