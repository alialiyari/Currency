using Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System;

namespace TagHelpers
{ 
    // data-toggle => data-bs-toggle
    // <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
    // <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.rtl.min.css" />
    // <link href="~/lib/bootstrap-icon/font/bootstrap-icons.min.css" rel="stylesheet" />
    // <link href="~/lib/PersianDatePicker/jquery.md.bootstrap.datetimepicker.style.css" rel="stylesheet" />

    //<script src="~/lib/jquery/dist/jquery.min.js"></script>
    //<script src="~/lib/bootstrap/js/bootstrap.bundle.min.js"></script>

    //<script src="~/lib/PersianDatePicker/jquery.md.bootstrap.datetimepicker.js"></script>

    // 1400/11/26
    // مودال مود رو اضافه کردم

    public class DateboxTagHelper : TagHelper
    {
        private readonly IStringLocalizerFactory _localizerFactory;
        public DateboxTagHelper(IStringLocalizerFactory localizerFactory)
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


            if ( Value == null)
            {
                Value = (DateTime?)For?.Model;
            }
            if (Value == null) { Value = DefaultNullValue; }

            if (string.IsNullOrEmpty(LabelText)) { LabelText = Localizer.GetString(Id); }
            if (string.IsNullOrEmpty(RequiredMessage)) RequiredMessage = $"{LabelText} Is Required";



           

            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<div class='form-text'>{HelperText}</div>";
            }


            var attrs = $@" 
                data-modalMode='{ModalMode.ToString().ToLower()}' 
                data-isGregorian='{IsGregorian.ToString().ToLower()}' 
                data-enableTimePicker='{EnableTimePicker.ToString().ToLower()}' 
                data-disableAfterToday='{DisableAfterToday.ToString().ToLower()}' 
                data-disableBeforeToday='{DisableBeforeToday.ToString().ToLower()}' 
            ";


            attrs += Value != null ? $" value='{Value?.ToString("yyyy-MM-dd")}' " : "";

            attrs += IsRequired == true ? " data-isrequired=true " : "";
            attrs += IsRequired == true ? $" data-isrequiredmessage='{RequiredMessage}' " : "";
            attrs += !string.IsNullOrEmpty(GroupName) ? $" data-groupname='{GroupName}' " : "";

            attrs += DisableAfterDate.HasValue == true ? $" data-disableAfterDate='{DisableAfterDate?.ToShortDateString()}' " : "";
            attrs += DisableBeforeDate.HasValue == true ? $" data-disableBeforeDate='{DisableBeforeDate?.ToShortDateString()}' " : "";


            string contents;

            contents = $@"
            <label class='form-label' for='{Id}'>{LabelText}</label>
             <div class='input-group'>
                <input id='{Id}Text' type='text' class='form-control text-end' >
                <input id='{Id}' type='text' style='display:none' data-type='Date' {attrs} />
                <span class='input-group-text rounded-0' id='{Id}Icon'><i class='bi-calendar2-date'></i></span>
            </div>
            {HelperText}";




            output.Content.SetHtmlContent(contents);
            if (ViewContext.ViewBag.JSInitInline != null) JSInitInline = ViewContext.ViewBag.JSInitInline;
            if (JSInitInline == true) output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");


        }
        public string Id { get; set; }
        public DateTime? Value { get; set; }
        public DateTime? DefaultNullValue { get; set; }



        public bool ModalMode { get; set; } = false;
        public bool IsGregorian { get; set; } = false;
        public bool EnableTimePicker { get; set; } = false;


        public bool DisableAfterToday { get; set; } = false;
        public bool DisableBeforeToday { get; set; } = false;


        public DateTime? DisableAfterDate { get; set; }
        public DateTime? DisableBeforeDate { get; set; }



        public bool? JSInitInline { get; set; }

        public string ColClass { get; set; } = "col-12 col-sm-6";

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";
        public bool IsRequired { get; set; }
        public string RequiredMessage { get; set; }
        public string GroupName { get; set; }
    }
}