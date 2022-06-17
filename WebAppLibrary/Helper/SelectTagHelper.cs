using Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace TagHelpers
{
    public class SelectTagHelper : TagHelper
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        public SelectTagHelper(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            IStringLocalizer Localizer = _localizerFactory.Create(ViewContext);

            output.TagName = "div";
            output.Attributes.SetAttribute("class", ColClass);
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrEmpty(Id)) { Id = For?.Name.Replace(".", "_"); }
            if (string.IsNullOrEmpty(Id)) { Id = context.UniqueId; }

            if (string.IsNullOrEmpty(LabelText)) { LabelText = Localizer.GetString(Id); }


            string attrs = $@" id='{Id}'  class='form-select' ";

            attrs += IsRequired == true ? $" data-isrequired=true data-isrequiredmessage='{RequiredMessage}' " : "";
            attrs += !string.IsNullOrEmpty(GroupName) ? $" data-groupname='{GroupName}' " : "";

            if (Multiple == true) attrs += " multiple ";
            if (context.AllAttributes["multiple"] != null) { attrs += "multiple "; }


            if (For?.Metadata.IsEnum == true)
            {
                if (string.IsNullOrEmpty(SelectedValue)) { SelectedValue = For.Model.Adapt<int?>().ToString(); }
            }
            else
            {
                if (string.IsNullOrEmpty(SelectedValue)) { SelectedValue = For?.Model?.ToString(); }
            }

            if (!string.IsNullOrEmpty(OnChange)) attrs += $" onchange='{OnChange}' ";




            if (SelectedValues == null) SelectedValues = new List<string>();
            if (!string.IsNullOrEmpty(SelectedValue)) SelectedValues.Add(SelectedValue);


            string toReturn = $@"<label class='form-label'>{LabelText}</label><select {attrs}>";
            if (FirstItem != null) { toReturn += $"<option value='{FirstItem.Value}'>{FirstItem.Text}</option>"; }


            if (Items != null)
            {
                string s = "";
                foreach (var item in Items)
                {
                    if (SelectedValues.Contains(item.Value)) { s = "selected='selected'"; };
                    toReturn += $"<option value='{item.Value}' {s}>{item.Text}</option>";
                    s = "";
                }
            }


            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<div class='form-control-feedback'>{HelperText}</div>";
            }


            toReturn += $@"</select>{HelperText}";
            output.Content.SetHtmlContent(toReturn);

            if (ViewContext.ViewBag.JSInitInline != null) JSInitInline = ViewContext.ViewBag.JSInitInline;
            if (JSInitInline == true) output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");

        }
        public string ColClass { get; set; } = "col-12 col-sm-6";
        public string Id { get; set; }

        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string PlaceHolder { get; set; }
        public string HelperText { get; set; } = "";

        public string GroupName { get; set; }

        public string RequiredMessage { get; set; }

        public bool Multiple { get; set; } = false;
        public bool IsRequired { get; set; }
        public bool? JSInitInline { get; set; }


        public string OnChange { get; set; }


        public string SelectedValue { get; set; }
        public List<string> SelectedValues { get; set; } = new List<string>();


        public SelectListItem FirstItem { get; set; }
        public List<SelectListItem> Items { get; set; }
    }

}