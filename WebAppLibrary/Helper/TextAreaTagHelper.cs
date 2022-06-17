using Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TagHelpers
{
    public class TextAreaTagHelper : TagHelper
    {

        public TextAreaTagHelper(IStringLocalizerFactory localizerFactory)
        {
            this.localizerFactory = localizerFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        private readonly IStringLocalizerFactory localizerFactory;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IStringLocalizer Localizer = this.localizerFactory.Create(ViewContext);


            output.TagName = "div";
            output.Attributes.SetAttribute("class", ColClass);
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrEmpty(Id)) { Id = For?.Name.Replace(".", "_"); }
            if (string.IsNullOrEmpty(Id)) { Id = context.UniqueId; }


            if (string.IsNullOrEmpty(Value)) { Value = For?.Model?.ToString(); }
            if (Value == null) { Value = DefaultNullValue; }

            if (string.IsNullOrEmpty(LabelText)) { LabelText = Localizer.GetString(Id); }



            if (For?.Metadata.IsRequired == true)
            {
                IsRequired = true;
                RequiredAttribute attr = (RequiredAttribute)For.Metadata.ValidatorMetadata.Where(x => x.GetType() == typeof(RequiredAttribute)).FirstOrDefault();
                RequiredMessage = attr.ErrorMessage.Replace("{0}", LabelText);
                LabelText += "<span class='text-danger'>*</span>";
            }


            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<div class='form-control-feedback'>{HelperText}</div>";
            }


            var attrs = "";
            if (IsReadOnly) { attrs += "readonly "; }


            var contents = $@"<label class='form-label' for='{Id}'>{LabelText}</label>
							  <textarea   class='form-control'  {attrs} 
                                {(IsRequired == true ? "data-isrequired=true" : "")} 
                                {(IsRequired == true ? $"data-isrequiredmessage='{RequiredMessage}'" : "")} 
                                {(!string.IsNullOrEmpty(GroupName) ? $"data-groupname='{GroupName}'" : "")} 
                                id='{Id}' >{Value}</textarea>";

            output.Content.SetHtmlContent(contents);
        }
        public string Id { get; set; }
        public string Value { get; set; }
        public string DefaultNullValue { get; set; }

        public string ColClass { get; set; } = "col-12 col-sm-6";

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public bool IsReadOnly { get; set; } = false;

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";
        public bool IsRequired { get; set; }
        public string RequiredMessage { get; set; }
        public string GroupName { get; set; }
    }
}