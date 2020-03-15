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
        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public PageViewModel PageModel { get; set; }

        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; } = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            for (int i = 1, totalPage = PageModel.TotalPages; i < totalPage; i++)
                ul.InnerHtml.AppendHtml(CreateItem(i));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreateItem(int pageNumber)
        {
            var li = new TagBuilder("li");
            var a = new TagBuilder("a");

            if (pageNumber == PageModel.PageNumber)
            {
                a.MergeAttribute("data-page", PageModel.PageNumber.ToString());
                li.AddCssClass("active");
            }
            else
            {
                PageUrlValues["page"] = pageNumber;
                a.Attributes["href"] = "#";

                foreach (var (key, value) in PageUrlValues.Where(p => p.Value != null))
                    a.MergeAttribute($"data-{key}", value.ToString());
            }

            a.InnerHtml.AppendHtml(pageNumber.ToString());
            li.InnerHtml.AppendHtml(a);

            return li;
        }
    }
}
