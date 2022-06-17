using Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
 

namespace TagHelpers
{
    public class NumberboxTagHelper : TagHelper
    {
        private readonly IStringLocalizerFactory localizerFactory;
        public NumberboxTagHelper(IStringLocalizerFactory localizerFactory)
        {
            this.localizerFactory = localizerFactory;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

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





            var attrs = "";
            if (MinLen.HasValue)
            {
                var message = MaxLenMessage;
                if (string.IsNullOrEmpty(message))
                {
                    message = Localizer[$"{Id}MinLenMessage"].Value;
                    if (message == $"{Id}MinLenMessage") message = "";
                }

                attrs += $" data-minlen='{MinLen}' data-minlenmessage='{message}' ";
            }
            if (MaxLen.HasValue)
            {
                var message = MaxLenMessage;
                if (string.IsNullOrEmpty(message))
                {
                    message = Localizer[$"{Id}MaxLenMessage"].Value;
                    if (message == $"{Id}MaxLenMessage") message = "";
                }

                attrs += $" data-maxlen='{MaxLen}' data-maxlenmessage='{message}' ";
            }
            if (IsRequired == true)
            {
                var message = Localizer[$"{Id}Required"].Value;
                if (message == $"{Id}Required") message = "";
                attrs += $" data-isrequired='true' data-isrequiredmessage='{message}' ";
            }






            if (IsDisabled == true) { attrs += " disabled "; }
            if (context.AllAttributes["onchange"] != null) { attrs += $"onchange='{context.AllAttributes["onchange"].Value}' "; }
            attrs += " style='direction:ltr' ";


            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<small class='form-text text-muted text-left'>{HelperText}</small>";
            }

            if (ShowSeprator == true)
            {
                attrs += " data-type='Number' type='text' ";
            }
            else
            {
                attrs += " type='number' ";

            }

            var contents =
                $@"
				<label class='form-label' for='{Id}'>{LabelText}</label>
				<input class='form-control ltr'  {attrs} {(Value != null ? $"value='{Value}'" : "")} 
                    {(IsRequired == true ? "data-isrequired=true" : "")} 
                    {(IsRequired == true ? $"data-isrequiredmessage='{RequiredMessage}'" : "")} 
                    {(!string.IsNullOrEmpty(GroupName) ? $"data-groupname='{GroupName}'" : "")} 
                    id='{Id}' />
                    {HelperText}
                ";


            output.Content.SetHtmlContent(contents);
            if (ViewContext.ViewBag.JSInitInline != null) JSInitInline = ViewContext.ViewBag.JSInitInline;
            if (JSInitInline == true) output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");
        }
        public string Id { get; set; }
        public string Value { get; set; }
        public string DefaultNullValue { get; set; }

        public bool JSInitInline { get; set; } = false;
        public bool ShowSeprator { get; set; } = false;

        public int Min { get; set; }
        public int Max { get; set; }



        public byte? MinLen { get; set; }
        public string MinLenMessage { get; set; }


        public byte? MaxLen { get; set; }
        public string MaxLenMessage { get; set; }



        public string ColClass { get; set; } = "col-12 col-sm-6";

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";

        public bool IsDisabled { get; set; }
        public bool IsRequired { get; set; }

        public string RequiredMessage { get; set; }
        public string GroupName { get; set; }
    }
}