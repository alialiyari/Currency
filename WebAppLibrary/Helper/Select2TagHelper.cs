using Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace TagHelpers
{
    public class Select2TagHelper : TagHelper
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        public Select2TagHelper(IStringLocalizerFactory localizerFactory)
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


            string attrs = $@" id='{Id}'  class='form-select Select2' style='width: 100%;' ";
            if (!string.IsNullOrEmpty(ResponseUrl)) attrs += $" data-responseurl='{ResponseUrl}'";

            attrs += !string.IsNullOrEmpty(GroupName) ? $" data-groupname='{GroupName}' " : "";
            attrs += IsRequired == true ? $" data-isrequired=true data-isrequiredmessage='{RequiredMessage}' " : "";

            if (Multiple == true) attrs += " multiple ";

            if (!string.IsNullOrEmpty(OnChange)) attrs += $" onchange='{OnChange}' ";
            if (!string.IsNullOrEmpty(CascadeRefreshUrl)) { attrs += $"data-cascaderefreshurl='{CascadeRefreshUrl}' "; }




            if (string.IsNullOrEmpty(SelectedValue))
            {
                if (For?.Metadata.IsEnum == true)
                {
                    SelectedValue = For.Model?.Adapt<int?>().ToString();
                }
                else
                {
                    SelectedValue = For?.Model?.ToString();
                }
            }



            if (string.IsNullOrEmpty(SelectedValue) == false)
            {
                SelectedItem = new SelectListItem() { Text = SelectedText, Value = SelectedValue };
            }



            if (string.IsNullOrEmpty(DefaultNullSelectedValue) == false)
            {
                DefaultNullSelectedItem = new SelectListItem() { Text = DefaultNullSelectedText, Value = DefaultNullSelectedValue };
            }

            if (SelectedItem == null) SelectedItem = DefaultNullSelectedItem;


            if (SelectedItems == null) SelectedItems = new List<SelectListItem>();
            if (SelectedItem != null) SelectedItems.Add(SelectedItem);


            attrs += $" data-selecteditems='{JsonConvert.SerializeObject(SelectedItems.Select(x => new { value = x.Value, text = x.Text }))}' ";

            string toReturn = $@"<label class='form-label'>{LabelText}</label><select {attrs}>";

            if (Items != null)
            {
                string s = "";
                foreach (var item in Items)
                {
                    if (SelectedItems.Any(x => x.Value == item.Value)) { s = "selected='selected'"; };
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
        public string ResponseUrl { get; set; }

        public string CascadeRefreshUrl { get; set; }



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


        public string SelectedText { get; set; }
        public string SelectedValue { get; set; }


        public string DefaultNullSelectedText { get; set; }
        public string DefaultNullSelectedValue { get; set; }
        public SelectListItem DefaultNullSelectedItem { get; set; }



        public List<SelectListItem> Items { get; set; }

        public SelectListItem SelectedItem { get; set; }
        public List<SelectListItem> SelectedItems { get; set; }





        public string OnChange { get; set; }
    }

}