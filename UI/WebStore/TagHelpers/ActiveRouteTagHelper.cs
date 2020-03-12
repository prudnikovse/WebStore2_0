using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class ActiveRouteTagHelper : TagHelper
    {
        public const string AttributeName = "is-active";
        public const string IgnoreActionName = "ignore-action";

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        private IDictionary<string, string> RouteValues { get; set; } =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        [HtmlAttributeNotBound, ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var ignoreAction = context.AllAttributes.ContainsName(IgnoreActionName);

            if (IsActive(ignoreAction))
                MakeActive(output);

            output.Attributes.RemoveAll(AttributeName);
            output.Attributes.RemoveAll(IgnoreActionName);
        }

        private bool IsActive(bool ignoreAction)
        {
            var routeValues = ViewContext.RouteData.Values;

            var currentController = routeValues["Controller"].ToString();
            var currentAction = routeValues["Action"].ToString();

            if (!string.Equals(Controller, currentController,
                StringComparison.OrdinalIgnoreCase))
                return false;

            if (!ignoreAction && !string.Equals(Action, currentAction,
                StringComparison.OrdinalIgnoreCase))
                return false;

            foreach(var (key, value) in RouteValues)
            {
                if (!routeValues.ContainsKey(key) || routeValues[key].ToString() != value)
                    return false;
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(attr => attr.Name == "class");

            if(classAttr == null)
            {
                output.Attributes.Add(new TagHelperAttribute("class", "active"));
            }
            else
            {
                if (classAttr.Value.ToString().Contains("active"))
                    return;

                output.Attributes.SetAttribute("class", classAttr.Value == null ?
                    "active" : $"{classAttr.Value} active");
            }
        }
    }
}
