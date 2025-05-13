using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApp.Helpers
{
    public static class HtmlHelpers
    {
        public static string CreateSWF(this HtmlHelper helper, string swfName, int width, int height, string flashVars)
        {
            StringBuilder sbSWF = new StringBuilder();

            sbSWF.AppendLine("<object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" width=\"" + width + "\" height=\"" + height + "\" id=\"Main\">");
            sbSWF.AppendLine("<param name=\"movie\" value=\"" + swfName + "\" />");
            sbSWF.AppendLine("<param name=\"wmode\" value=\"transparent\" />");
            sbSWF.AppendLine("<param name=\"quality\" value=\"high\" />");
            sbSWF.AppendLine("<param name=\"bgcolor\" value=\"#ffffff\" />");
            sbSWF.AppendLine("<param name=\"allowScriptAccess\" value=\"sameDomain\" />");
            sbSWF.AppendLine("<param name=\"allowFullScreen\" value=\"true\" />");
            if (!string.IsNullOrEmpty(flashVars))
            {
                sbSWF.AppendLine("<param name=\"flashvars\" value=\"" + flashVars + "\" />");
            }
            sbSWF.AppendLine("<!--[if !IE]>-->");
            sbSWF.AppendLine("<object type=\"application/x-shockwave-flash\" data=\"" + swfName + "\" width=\"" + width + "\" height=\"" + height + "\">");
            sbSWF.AppendLine("<param name=\"quality\" value=\"high\" />");
            sbSWF.AppendLine("<param name=\"wmode\" value=\"transparent\" />");
            sbSWF.AppendLine("<param name=\"bgcolor\" value=\"#ffffff\" />");
            sbSWF.AppendLine("<param name=\"allowScriptAccess\" value=\"sameDomain\" />");
            sbSWF.AppendLine("<param name=\"allowFullScreen\" value=\"true\" />");
            if (!string.IsNullOrEmpty(flashVars))
            {
                sbSWF.AppendLine("<param name=\"flashvars\" value=\"" + flashVars + "\" />");
            }
            sbSWF.AppendLine(" <!--<![endif]-->");
            sbSWF.AppendLine("<!--[if gte IE 6]>-->");
            sbSWF.AppendLine(" <p>");
            sbSWF.AppendLine(" ");
            sbSWF.AppendLine(" version 10.2.0 or greater is not installed.");
            sbSWF.AppendLine("</p>");
            sbSWF.AppendLine("<!--<![endif]-->");
            sbSWF.AppendLine("<a href=\"http://www.adobe.com/go/getflashplayer\">");
            sbSWF.AppendLine(" <img src=\"http://www.adobe.com/images/shared/download_buttons/get_flash_player.gif\" alt=\"Get Adobe Flash Player\" />");
            sbSWF.AppendLine("</a>");
            sbSWF.AppendLine("<!--[if !IE]>-->");
            sbSWF.AppendLine("</object>");
            sbSWF.AppendLine("<!--<![endif]-->");
            sbSWF.AppendLine("</object>");

            return sbSWF.ToString();
        }
    }
}