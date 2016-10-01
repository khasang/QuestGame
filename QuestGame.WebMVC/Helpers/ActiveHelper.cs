using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuestGame.WebMVC.Helpers
{
    public static class ActiveHelper
    {
        public static MvcHtmlString Active(this HtmlHelper html, bool active)
        {
            string str = string.Empty;

            if (active)
            {
                str = @"<span class='glyphicon glyphicon-ok'></span>";
            }
            else
            {
                str = @"<span class='glyphicon glyphicon-remove'></span>";
            }

            return new MvcHtmlString(str);
        }
    }
}