using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
namespace hoclaptrinhweb
{
    /// <summary>
    /// [Usage]
    /// --- From each view / partial view ---
    /// @Html.AddClientStyle("~/ContentV3/detailnam.min.css",0);
    /// @Html.AddClientStyle("http://s.tgdd.vn/comment/Content/css/comment.min.css",1);
    /// 
    /// --- From the main Master/View (just before last Body tag so all "registered" scripts are included) ---
    /// @Html.ClientStyles();
    /// </summary>
    public static class StyleExtensions
    {
        public static string themePath = "/Themes/2015/";


        public static string sKey = "client-style-list";

        public static MvcHtmlString AddClientStyle(this HtmlHelper helper, string stylePath, int iOrder = 0)
        {
            //If script list does not already exist then initialise
            if (!helper.ViewContext.HttpContext.Items.Contains(sKey))
                helper.ViewContext.HttpContext.Items[sKey] = new Dictionary<string, KeyValuePair<string, int>>();
            var arrStyle = helper.ViewContext.HttpContext.Items[sKey] as Dictionary<string, KeyValuePair<string, int>>;
            if (!arrStyle.ContainsKey(stylePath))
                arrStyle.Add(stylePath, new KeyValuePair<string, int>(stylePath, iOrder));
            return MvcHtmlString.Empty;
        }

        private static string MakeStyleTag(HtmlHelper helper, string url)
        {
            string content = "";
            string path = helper.ViewContext.HttpContext.Server.MapPath(url);
            content = System.IO.File.ReadAllText(path);
            if (System.Web.HttpContext.Current.Request.IsLocal)
            {
                content = content.Replace("url(images/", "url(" + themePath + "images/");
                content = content.Replace("url(fonts/", "url(" + themePath + "fonts/");
            }
            else
            {

            }
            if (System.Web.HttpContext.Current.Request.IsLocal == true)
                content = "/* " + url + " */" + content;
            return content;
        }

        public static MvcHtmlString GetClientStyles(this HtmlHelper helper)
        {
            var scripts = helper.ViewContext.HttpContext.Items[sKey] as Dictionary<string, KeyValuePair<string, int>> ?? new Dictionary<string, KeyValuePair<string, int>>();
            var scriptList = new List<string>();
            if (scripts.Count > 0)
            {
                scriptList.AddRange(scripts.OrderBy(s => s.Value.Value).Select(s => s.Value.Key));
            }
            //Generate a script tag for each script
            var scriptsToRender = scriptList.Select(s => MakeStyleTag(helper, s)).ToList();
            scriptsToRender.Insert(0, "<style>");
            scriptsToRender.Insert(scriptsToRender.Count, "</style>");
            return (scriptsToRender.Count > 0
                ? MvcHtmlString.Create(string.Join(Environment.NewLine, scriptsToRender))
                : MvcHtmlString.Empty);
        }

    }
}