using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.IO;
using System.Reflection;



/// <summary>
/// Summary description for GridviewExportUtil
/// </summary>
public class GridViewExportUtil
{

    /// <summary>
    /// Export a gridview to an xls file - use to actually build an html table for export
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    public static void Export(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();

        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        HttpContext.Current.Response.ContentType = "application/ms-excel";

        using (StringWriter sw = new StringWriter())
        {
            using (HtmlTextWriter htw = new HtmlTextWriter(sw))
            {
                //  Create a form to contain the grid  
                Table table = new Table();
                //  add the header row to the table  
                if (gv.HeaderRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.HeaderRow);
                    table.Rows.Add(gv.HeaderRow);
                }
                //  add each of the data rows to the table  
                foreach (GridViewRow row in gv.Rows)
                {
                    GridViewExportUtil.PrepareControlForExport(row);
                    table.Rows.Add(row);
                }
                //  add the footer row to the table  
                if (gv.FooterRow != null)
                {
                    GridViewExportUtil.PrepareControlForExport(gv.FooterRow);
                    table.Rows.Add(gv.FooterRow);
                }
                //  render the table into the htmlwriter  
                table.RenderControl(htw);
                //  render the htmlwriter into the response  
                HttpContext.Current.Response.Write(sw.ToString());
                HttpContext.Current.Response.End();
            }
        }

    }

    // Replace any of the contained controls with literals
    private static void PrepareControlForExport(Control control)
    {
        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if ((current is LinkButton))
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((LinkButton)current).Text));
            }
            else if ((current is ImageButton))
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((ImageButton)current).AlternateText));
            }
            else if ((current is HyperLink))
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((HyperLink)current).Text));
            }
            else if ((current is DropDownList))
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((DropDownList)current).SelectedItem.Text));
            }
            else if ((current is CheckBox))
            {
                control.Controls.Remove(current);
                control.Controls.AddAt(i, new LiteralControl(((CheckBox)current).Checked.ToString()));
            }
            if (current.HasControls())
            {
                GridViewExportUtil.PrepareControlForExport(current);
            }
        }
    }

    /// <summary>
    /// Export a gridview to a csv file
    /// Supports template fields in the grid
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="gv"></param>
    public static void ExportToCSV(string fileName, GridView gv)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fileName));
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
     //   HttpContext.Current.Response.ContentType = "application/text";
        HttpContext.Current.Response.Charset = "";
        StringBuilder sb = new StringBuilder();
        for (int k = 0; k < gv.Columns.Count; k++)
        {
            //add separator         
            sb.Append("\"" + gv.Columns[k].HeaderText + "\",");
        }
        //append new line     
        sb.Append("\r\n");
        foreach (GridViewRow row in gv.Rows)
        {
            foreach (TableCell cell in row.Cells)
            {
                if (cell.HasControls())
                {
                    foreach (Control ctl in cell.Controls)
                    {
                        if ((ctl is LinkButton))
                        {
                            sb.Append("\"" + ((LinkButton)ctl).Text + "\",");
                        }
                        else if ((ctl is ImageButton))
                        {
                            sb.Append("\"" + ((ImageButton)ctl).AlternateText + "\",");
                        }
                        else if ((ctl is HyperLink))
                        {
                            sb.Append("\"" + ((HyperLink)ctl).Text + "\",");
                        }
                        else if ((ctl is DropDownList))
                        {
                            sb.Append("\"" + ((DropDownList)ctl).SelectedItem.Text + "\",");
                        }
                        else if ((ctl is CheckBox))
                        {
                            sb.Append("\"" + ((CheckBox)ctl).Checked.ToString() + "\",");
                        }
                        else if (ctl is Label)
                        {
                            sb.Append("\"" + ((Label)ctl).Text + "\",");
                        }
                        else if (ctl is TextBox)
                        {
                            sb.Append("\"" + ((TextBox)ctl).Text + "\",");
                        }
                    }
                }
                else
                {
                    sb.Append("\"" + cell.Text + "\",");
                }
            }
            sb.Append("\r\n");
        }

        HttpContext.Current.Response.Output.Write(sb.ToString());
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    } 
}

