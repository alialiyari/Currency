using Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System.Threading;

namespace TagHelpers
{


    public class TextboxTagHelper : TagHelper
    {
        // نیمه کاره مونده، سمت جاوااسکریپتش مونده
        public enum AlphabetEnum
        {
            Both =0,
            EnglishAlphabet = 1,
            PersianAlphabet = 2,
            ByLanguage = 3,

        }

        private readonly IStringLocalizerFactory _localizerFactory;
        public TextboxTagHelper(IStringLocalizerFactory localizerFactory)
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


            if (string.IsNullOrEmpty(Value)) { Value = For?.Model?.ToString(); }
            if (Value == null) { Value = DefaultNullValue; }


            //if (string.IsNullOrEmpty(LabelText)) { LabelText = For?.Metadata.DisplayName; }
            if (string.IsNullOrEmpty(LabelText)) { LabelText = Localizer.GetString(Id); }


            if (AlphabetType == AlphabetEnum.ByLanguage)
            {
                AlphabetType = AlphabetEnum.EnglishAlphabet;
                if (Thread.CurrentThread.CurrentCulture.Name == "fa") AlphabetType = AlphabetEnum.PersianAlphabet;
            }



            var attrs = $" type='text' data-alphabettype='{AlphabetType}' ";

            if (!string.IsNullOrEmpty(OnChange)) attrs += $" onchange='{OnChange}' ";
            if (!string.IsNullOrEmpty(OnEnter)) attrs += $" data-onenter='{OnEnter}' ";

            attrs += !string.IsNullOrEmpty(GroupName) ? $" data-groupname='{GroupName}' " : "";


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
                var message = RequiredMessage;
                if (string.IsNullOrEmpty(message))
                {
                    message = Localizer[$"{Id}Required"].Value;
                    if (message == $"{Id}Required") message = "";
                }
                attrs += $" data-isrequired='true' data-isrequiredmessage='{message}' ";
            }



            if (IsDisabled) { attrs += " disabled='disabled' "; }
            if (!string.IsNullOrEmpty(PlaceHolder)) { attrs += $" placeholder='{PlaceHolder}' "; }


            if (!string.IsNullOrEmpty(HelperText))
            {
                HelperText = $"<small class='form-text text-muted text-end'>{HelperText}</small>";
            }

            var contents = $@" 
				<label class='form-label' for='{Id}'>{LabelText}</label>
                <input class='form-control {InputExtraClass}' {attrs} {(Value != null ? $"value='{Value}'" : "")}  id='{Id}' /> 
                {HelperText}
             ";

            output.Content.SetHtmlContent(contents);
            if (ViewContext.ViewBag.JSInitInline != null) JSInitInline = ViewContext.ViewBag.JSInitInline;
            if (JSInitInline == true) output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");

        }



        public string Id { get; set; }
        public string Value { get; set; }
        public string DefaultNullValue { get; set; }

        public AlphabetEnum AlphabetType { get; set; } = AlphabetEnum.Both;
        public string ColClass { get; set; } = "col-12 col-sm-6";


        public string PlaceHolder { get; set; } = "";
        public string InputExtraClass { get; set; } = "";



        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }

        public string LabelText { get; set; }
        public string HelperText { get; set; } = "";
        public bool? JSInitInline { get; set; }

        public bool IsDisabled { get; set; }
        public bool IsRequired { get; set; }
        public string RequiredMessage { get; set; }


        public byte? MinLen { get; set; }
        public string MinLenMessage { get; set; }


        public byte? MaxLen { get; set; }
        public string MaxLenMessage { get; set; }


        public string GroupName { get; set; }
        public string OnEnter { get; set; }
        public string OnChange { get; set; }
    }
}