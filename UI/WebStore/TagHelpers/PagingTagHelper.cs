using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.ViewModels;

namespace WebStore.TagHelpers
{
    public class PagingTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _UrlHelperFactory;

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        public string PageAction { get; set; }

        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public PagingTagHelper(IUrlHelperFactory UrlHelperFactory) => _UrlHelperFactory = UrlHelperFactory;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _UrlHelperFactory.GetUrlHelper(ViewContext);

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            for (int i = 1, totalPage = PageModel.TotalPages; i < totalPage; i++)
                ul.InnerHtml.AppendHtml(CreateItem(i, urlHelper));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreateItem(int pageNumber, IUrlHelper url)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");

            if (pageNumber == PageModel.PageNumber)
                li.AddCssClass("active");
            else
            {
                PageUrlValues["page"] = pageNumber;
                a.Attributes["href"] = url.Action(PageAction, PageUrlValues);
            }

            a.InnerHtml.AppendHtml(pageNumber.ToString());
            li.InnerHtml.AppendHtml(a);

            return li;
        }
    }
}
