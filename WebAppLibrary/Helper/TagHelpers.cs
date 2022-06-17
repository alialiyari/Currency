using Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TagHelpers
{
    public class TimeboxTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", ColClass);
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrEmpty(Id)) { Id = For?.Name.Replace(".", "_"); }
            if (string.IsNullOrEmpty(Id)) { Id = context.UniqueId; }


            if (string.IsNullOrEmpty(Value)) { Value = For?.Model?.ToString(); }
            if (Value == null) { Value = DefaultNullValue; }

            if (string.IsNullOrEmpty(LabelText)) { LabelText = For?.Metadata.DisplayName; }




            string ValueStr = "";
            if (DateTime.TryParse(Value, out DateTime ValueDate))
            {
                ValueStr = ValueDate.ToString("HH:mm");
            }



            var attrs = "";
            attrs += ValueStr != null ? $"value='{ValueStr}'" : "";
            attrs += $"  id='{Id}' type='text' style='direction:ltr' ";
            attrs += IsRequired == true ? " data-isrequired=true" : "";
            attrs += IsRequired == true ? $" data-isrequiredmessage='{RequiredMessage}'" : "";
            attrs += !string.IsNullOrEmpty(GroupName) ? $"data-groupname='{GroupName}'" : "";

            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<div class='form-control-feedback'>{HelperText}</div>";
            }

            var contents =
                $@"<div class='form-group'>
			    <label for='{Id}'>{LabelText}</label>
                <div class='controls'>
				    <input type='time' class='form-control' {attrs} />
                </div>
            </div>";

            output.Content.SetHtmlContent(contents);
            if (JSInit == true)
                output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");

        }
        public string Id { get; set; }
        public string Value { get; set; }
        public string DefaultNullValue { get; set; }

        public string ColClass { get; set; } = "col-12 col-sm-6";

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";
        public bool IsRequired { get; set; }
        public bool JSInit { get; set; } = false;
        public string RequiredMessage { get; set; }
        public string GroupName { get; set; }
    }

    public class TimeboxOldTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", ColClass);
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrEmpty(Id)) { Id = For?.Name.Replace(".", "_"); }
            if (string.IsNullOrEmpty(Id)) { Id = context.UniqueId; }


            if (string.IsNullOrEmpty(Value)) { Value = For?.Model?.ToString(); }
            if (Value == null) { Value = DefaultNullValue; }

            if (string.IsNullOrEmpty(LabelText)) { LabelText = For?.Metadata.DisplayName; }



            //if (For?.Metadata.IsRequired == true)
            //{
            //    IsRequired = true;
            //    RequiredAttribute attr = (RequiredAttribute)For.Metadata.ValidatorMetadata.Where(x => x.GetType() == typeof(RequiredAttribute)).FirstOrDefault();
            //    RequiredMessage = attr.ErrorMessage.Replace("{0}", LabelText);
            //    LabelText += "<span class='text-danger'>*</span>";
            //}



            string ValueStr = "";
            DateTime ValueDate;
            if (System.DateTime.TryParse(Value, out ValueDate))
            {
                ValueStr = ValueDate.ToShortTimeString();
            }



            var attrs = "";
            if (context.AllAttributes["readonly"] != null) { attrs += "readonly "; }
            if (context.AllAttributes["onchange"] != null) { attrs += $"onchange='{context.AllAttributes["onchange"].Value}' "; }
            attrs += "type='text' style='direction:ltr' ";


            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<div class='form-control-feedback'>{HelperText}</div>";
            }

            var contents = $@"<div class='form-group'>
								<label for='{Id}'>{LabelText}</label>
                                <div class='controls'>
								    <input   class='form-control timepicker'  {attrs} {(ValueStr != null ? $"value='{ValueStr}'" : "")} 
                                        {(IsRequired == true ? "data-isrequired=true" : "")} 
                                        {(IsRequired == true ? $"data-isrequiredmessage='{RequiredMessage}'" : "")} 
                                        {(!string.IsNullOrEmpty(GroupName) ? $"data-groupname='{GroupName}'" : "")} 
                                        id='{Id}'>
                                </div>
                            </div>";

            output.Content.SetHtmlContent(contents);
            output.Content.AppendHtml($"<script>document.addEventListener('DOMContentLoaded', function () {{ $('#{Id}').pickatime({{twelvehour: true,}}); }})</script>");

        }
        public string Id { get; set; }
        public string Value { get; set; }
        public string DefaultNullValue { get; set; }

        public string ColClass { get; set; } = "col-md-6";

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";
        public bool IsRequired { get; set; }
        public string RequiredMessage { get; set; }
        public string GroupName { get; set; }
    }

    public class RadioboxListTagHelper : TagHelper
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        public RadioboxListTagHelper(IStringLocalizerFactory localizerFactory)
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



            string attrs = $" id='{Id}' data-type='radio' ";
            attrs += IsRequired == true ? $" data-isrequired=true data-isrequiredmessage='{RequiredMessage}' " : "";
            attrs += !string.IsNullOrEmpty(GroupName) ? $" data-groupname='{GroupName}' " : "";


            if (string.IsNullOrEmpty(SelectedValue)) { SelectedValue = For?.Model?.ToString(); }






            int index = 1;
            string Radioes = "";
            foreach (var item in Items)
            {
                Radioes +=
                    $@"<div class='form-check form-check-inline'>
                        <input class='form-check-input' name='{Id}Name' type='radio' id='{Id}{index}'
                            value='{item.Value}'  {(SelectedValue == item.Value ? "checked='checked'" : "") }>
                        <label class='form-check-label' for='{Id}{index}'>{item.Text}</label>
                 </div>";
                index += 1;
            }
            if (string.IsNullOrEmpty(LabelText))
            {
                LabelText = Localizer.GetString(Id); if (Id == LabelText) LabelText = "";
            }
            if (!string.IsNullOrEmpty(LabelText)) LabelText = $"<label>{LabelText}</label>";
            if (!string.IsNullOrEmpty(HelperText)) HelperText = $"<div class='form-text text-muted text-left'>{HelperText}</div>";



            string toReturn =
            $@"<div class='form-group'>{LabelText}
            <div class='controls'>
                <input type='hidden' {attrs} />
                <div class='form-control'>{Radioes}</div>
                {HelperText} 
           </div>
        </div>";
            output.Content.SetHtmlContent(toReturn);

            if (ViewContext.ViewBag.JSInitInline != null) JSInitInline = ViewContext.ViewBag.JSInitInline;
            if (JSInitInline == true) output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");

        }

        public string ColClass { get; set; } = "col-12 col-sm-6";

        public string Id { get; set; }

        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";

        public string GroupName { get; set; }

        public string RequiredMessage { get; set; }

        public bool IsRequired { get; set; }
        public bool? JSInitInline { get; set; }

        public string SelectedValue { get; set; }

        public List<SelectListItem> Items { get; set; }
    }
}