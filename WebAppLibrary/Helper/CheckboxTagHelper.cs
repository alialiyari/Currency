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

    public class CheckboxTagHelper : TagHelper
    {

        private readonly IStringLocalizerFactory _localizerFactory;
        public CheckboxTagHelper(IStringLocalizerFactory localizerFactory)
        {
            _localizerFactory = localizerFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IStringLocalizer Localizer = _localizerFactory.Create(ViewContext);

            //form-text text-muted text-left

            output.TagName = "div";
            output.Attributes.SetAttribute("class", $"Checkbox {ColClass}");
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrEmpty(Id)) { Id = Id = For?.Name.Replace(".", "_"); }
            //if (string.IsNullOrEmpty(LabelText)) { LabelText = For?.Metadata.DisplayName; }
            if (string.IsNullOrEmpty(CheckboxtText)) { CheckboxtText = Localizer.GetString(Id); }
            //if (string.IsNullOrEmpty(LabelText)) LabelText = "&nbsp;";

            if (For?.Model != null) { IsChecked = bool.Parse(For.Model.ToString()); }

            string attrs = $" type='checkbox' id='{Id}' {(IsChecked == true ? "checked" : "")}  {(!string.IsNullOrEmpty(OnChange) ? "onchange='" + OnChange + "'" : "")} ";



            var contents = $@"
                {(!string.IsNullOrEmpty(LabelText) ? $"<label class='form-label'>{LabelText}</label>" : "")}
                    <div class='form-check'>
                        <input class='form-check-input' {attrs}  />
                        <label for='{Id}' class='form-check-label'>{CheckboxtText}</label>
                    </div>
            ";

            output.Content.SetHtmlContent(contents);
        }
        public string ColClass { get; set; } = "col-12 col-sm-6";


        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public bool? IsChecked { get; set; } = false;


        public string LabelText { get; set; }
        public string CheckboxtText { get; set; }
        public string OnChange { get; set; }
        public string Id { get; set; }

    }

}