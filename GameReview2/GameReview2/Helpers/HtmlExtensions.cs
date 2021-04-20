using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameReview2.Helpers
{
    public static class HtmlExtensions
    {
        public static IHtmlString StripHtml(this HtmlHelper helper, string content, int limit)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            if (limit > 0 && htmlDoc.DocumentNode.InnerText.Length > limit)
                return new HtmlString(htmlDoc.DocumentNode.InnerText.Substring(0, limit) + "...");
            return new HtmlString(htmlDoc.DocumentNode.InnerText);
        }

        public static IHtmlString GetThumbnail(this HtmlHelper helper, byte[] photo, int thumbWidth = 120, int thumbHeight = 120)
        {
            Image image = ImageConverter.ConvertToThumbnail(photo, thumbWidth, thumbHeight);

            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);

            return GetImage(helper, ms.ToArray(), thumbWidth, thumbHeight);
        }

        public static IHtmlString GetImage(this HtmlHelper helper, byte[] photo, int? width, int? height)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", String.Format("data:image/jpeg;base64,{0}", Convert.ToBase64String(photo)));
            if (height.HasValue)
                builder.MergeAttribute("height", height.Value.ToString());
            if (width.HasValue)
                builder.MergeAttribute("width", width.ToString());
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static IHtmlString GetArrow(this HtmlHelper helper, string currentSortBy, string sortBy, string sortDir)
        {
            if (currentSortBy == sortBy)
            {
                if (sortDir == "asc")
                    return helper.Raw(@"<span>&uarr;</span>");
                else
                    return helper.Raw(@"<span>&darr;</span>");
            }
            return helper.Raw("");
        }
    }
}