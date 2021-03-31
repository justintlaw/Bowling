using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Bowling.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bowling.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-info")]
    public class PaginationTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlInfo;

        public PaginationTagHelper (IUrlHelperFactory urlHelperFactory)
        {
            _urlInfo = urlHelperFactory;
        }

        public PageNumberingInfo PageInfo { get; set; }
        public string PageUrlTeamName { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> KeyValuePairs { get; set; } = new Dictionary<string, object>();

        // This element will not be used on the front end, but is for use in the tag helper
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public string PageClassDisabled { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlInfo.GetUrlHelper(ViewContext);

            TagBuilder finishedTag = new TagBuilder("div");

            // Add Previous button
            TagBuilder previousTag = new TagBuilder("a");
            previousTag.InnerHtml.Append("Previous");
            if (PageInfo.CurrentPage == 1)
            {
                // Disable the button if there is no previous page
                previousTag.AddCssClass(PageClassDisabled);
                previousTag.AddCssClass(PageClass);
            }
            else
            {
                KeyValuePairs["pageNum"] = PageInfo.CurrentPage - 1;
                previousTag.Attributes["href"] = urlHelper.Action("Index", KeyValuePairs);
                previousTag.AddCssClass(PageClassNormal);
                previousTag.AddCssClass(PageClass);
            }
            finishedTag.InnerHtml.AppendHtml(previousTag);

            // Add each number in the pagination list
            for (int i = 1; i <= PageInfo.NumPages; i++)
            {
                KeyValuePairs["pageNum"] = i;
                TagBuilder innerTag = new TagBuilder("a");
                innerTag.Attributes["href"] = urlHelper.Action("Index", KeyValuePairs);

                // Add CSS to the pagination
                if (PageClassesEnabled)
                {
                    innerTag.AddCssClass(i == PageInfo.CurrentPage ? PageClassSelected : PageClassNormal);
                    innerTag.AddCssClass(PageClass);
                }

                innerTag.InnerHtml.Append(i.ToString());

                finishedTag.InnerHtml.AppendHtml(innerTag);
            }

            // Add Next button
            TagBuilder nextTag = new TagBuilder("a");
            nextTag.InnerHtml.Append("Next");
            if (PageInfo.CurrentPage == PageInfo.NumPages || PageInfo.NumPages == 0)
            {
                // Disable the button if there is no next page
                nextTag.AddCssClass(PageClassDisabled);
                nextTag.AddCssClass(PageClass);
            }
            else
            {
                KeyValuePairs["pageNum"] = PageInfo.CurrentPage + 1;
                nextTag.Attributes["href"] = urlHelper.Action("Index", KeyValuePairs);
                nextTag.AddCssClass(PageClassNormal);
                nextTag.AddCssClass(PageClass);
            }
            finishedTag.InnerHtml.AppendHtml(nextTag);

            output.Content.AppendHtml(finishedTag.InnerHtml);
        }
    }
}
