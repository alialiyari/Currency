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
    public class FileUploaderTagHelper : TagHelper
    {

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AcceptFile == AcceptEnum.Images) Accept = "image/*";
            if (string.IsNullOrEmpty(Id)) { Id = For?.Name.Replace(".", "_"); }
            if (string.IsNullOrEmpty(Id)) { Id = context.UniqueId; }

            if (PresetFiles == null)
            {

                if (For?.Model?.GetType().IsGenericType == true)
                {
                    PresetFiles = (List<string>)For?.Model;
                }
                else
                {
                    string Url = For?.Model?.ToString();
                    if (!string.IsNullOrEmpty(Url)) { PresetFiles = new List<string>() { Url }; }
                }

            }

            output.TagName = "div";
            output.Attributes.SetAttribute("class", ColClass);
            output.TagMode = TagMode.StartTagAndEndTag;

            string attrs = MultipleFileSelect == true ? "multiple" : "";
            attrs += !string.IsNullOrEmpty(GroupName) ? $" data-groupname='{GroupName}'" : "";

            attrs += MaxFileCount.HasValue == true ? $" data-maxfilecount='{MaxFileCount}' " : "";
            attrs += PresetFiles != null ? $" data-presetfiles='{Newtonsoft.Json.JsonConvert.SerializeObject(PresetFiles.ToArray())}' " : "";

            attrs += IsRequired == true ? " data-isrequired=true" : "";
            attrs += IsRequired == true ? $" data-isrequiredmessage='{RequiredMessage}' " : "";

            if (MaxSize.HasValue)
            {
                MaxSize *= 1024;
                attrs += $" data-maxsize='{MaxSize}'  data-maxsizemessage='{RequiredMessage}'";
            }

            //<input type='hidden' name='MAX_FILE_SIZE' value='{MaxSize}' />

            if (string.IsNullOrEmpty(Id)) { Id = context.UniqueId; }
            var contents = $@"
<div class='custom-file-container' data-upload-id='{Id}'>
    <label class='form-label d-flex'>{LabelText} <a href='javascript:void(0)' class='custom-file-container__image-clear ms-auto' ><i class='bi bi-trash text-danger'></i></a></label>
    <label class='custom-file-container__custom-file'>
        <input type='file' class='custom-file-container__custom-file__custom-file-input' accept='{Accept}' {attrs} aria-label='انتخاب فایل' id='{Id}' name='{Id}' />
       
        <span class='custom-file-container__custom-file__custom-file-control text-end'></span>
    </label>
    <div class='custom-file-container__image-preview mb-0'></div>
</div>";



            output.Content.SetHtmlContent(contents);



            if (ViewContext.ViewBag.JSInitInline != null) JSInitInline = ViewContext.ViewBag.JSInitInline;
            if (JSInitInline == true) output.Content.AppendHtml($"<script>ValidationEventBind($('#{Id}'))</script>");
        }

        public string Id { get; set; }

        [HtmlAttributeName("for")]
        public ModelExpression For { get; set; }



        public bool? JSInitInline { get; set; }


        /// <summary>
        /// بر اساس کیلوبایت
        /// </summary>
        public int? MaxSize { get; set; }
        public string MaxSizeMessaage { get; set; }



        public byte? MaxFileCount { get; set; }

        public List<string> PresetFiles { get; set; }

        private string Accept { get; set; } = "*";
        public AcceptEnum AcceptFile { get; set; } = AcceptEnum.All;


        public bool MultipleFileSelect { get; set; } = false;


        public string ColClass { get; set; } = "col-12 col-sm-6";

        public string LabelText { get; set; }
        public string GroupName { get; set; }

        public bool IsRequired { get; set; }
        public string RequiredMessage { get; set; }

        public enum AcceptEnum
        {
            All,
            Images,
        }
    }
}