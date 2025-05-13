using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace WebApp.Models
{
    public class XMLSitemapResult : ActionResult
    {
        /// <summary>
        /// Get the list of sitemap urls.
        /// </summary>
        private IEnumerable<IXMLSitemapItem> items;

        /// <summary>
        /// Construct this instance.
        /// </summary>
        /// <param name="sitemapUrls">List of urls in this sitemap.</param>
        public XMLSitemapResult(IEnumerable<IXMLSitemapItem> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Writes the result to the output by serializing the list of urls
        /// into the required
        /// sitemap format.
        /// </summary>
        /// <param name="context">Controller context.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            XNamespace blank = XNamespace.Get(@"http://www.sitemaps.org/schemas/sitemap/0.9");
            XNamespace xsi = XNamespace.Get(@"http://www.w3.org/2001/XMLSchema-instance");

            string encoding = context.HttpContext.Response.ContentEncoding.WebName;
            XDocument sitemap = new XDocument
                (
                    new XDeclaration("1.0", encoding, "yes"),
                    new XElement(blank + "urlset",
                            new XAttribute("xmlns", blank.NamespaceName),
                            new XAttribute(XNamespace.Xmlns + "xsi", xsi.NamespaceName),
                                from item in items
                                select CreateItemElement(item, blank)
                            )
                );

            context.HttpContext.Response.ContentType = "text/xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
        }

        private XElement CreateItemElement(IXMLSitemapItem item, XNamespace blank)
        {
            XElement itemElement = new XElement(blank + "url", new XElement(blank + "loc", item.Url.ToLower()));

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement(blank + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement(blank + "changefreq", item.ChangeFrequency.Value.ToString()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement(blank + "priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)));

            return itemElement;
        }

    }
}