
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace TagHelpers
{
    public class GridViewTagHelper : TagHelper
    {
        private readonly IViewComponentHelper _viewComponentHelper;

        public GridViewTagHelper(IViewComponentHelper viewComponentHelper)
        {
            _viewComponentHelper = viewComponentHelper;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            if (string.IsNullOrEmpty(Id)) { Id = "GV"; }
            if (string.IsNullOrEmpty(ColumnKeyName)) { ColumnKeyName = "Id"; }


            GridViewDTO DTO = new()
            {
                FirstLoad = false,
                ViewType = ViewType,
                ExcelRowColumns = ExcelRowColumns,
            };

            DTO.DataSouceRequest.PageNumber = 1;
            DTO.DataSouceRequest.PageSize = 20;

            output.Attributes.SetAttribute("id", Id);

            output.Attributes.SetAttribute("data-pagesize", DTO.DataSouceRequest.PageSize);
            output.Attributes.SetAttribute("data-responseurl", ResponseUrl);


            output.Attributes.SetAttribute("data-columnkeyname", ColumnKeyName);
            output.Attributes.SetAttribute("data-autoFillOnClientSide", AutoFillOnClientSide.ToString().ToLower());

            output.Attributes.SetAttribute("data-menugenerationisinCustom", MenuGenerationIsInCustom);
            output.Attributes.SetAttribute("data-customizationpartialviewaddress", CustomizationPartialViewAddress);




            DTO.ColumnKeyName = ColumnKeyName;
            DTO.CustomizationPartialViewAddress = CustomizationPartialViewAddress;

            context.Items.Add("TranslateFormName", TranslateFormName);
            context.Items.Add("Columns", new List<GridViewColumn>());
            context.Items.Add("GridViewActions", new List<GridViewAction>());

            _ = await output.GetChildContentAsync();

            DTO.Id = Id;
            DTO.Columns = context.Items["Columns"] as List<GridViewColumn>;
            DTO.Actions = context.Items["GridViewActions"] as List<GridViewAction>;

            output.Attributes.SetAttribute("data-viewtype", ViewType);
            output.Attributes.SetAttribute("data-excelrowcolumns", ExcelRowColumns);
            output.Attributes.SetAttribute("data-actions", Newtonsoft.Json.JsonConvert.SerializeObject(DTO.Actions));
            output.Attributes.SetAttribute("data-columns", Newtonsoft.Json.JsonConvert.SerializeObject(DTO.Columns));


            ((IViewContextAware)_viewComponentHelper).Contextualize(ViewContext);
            var content = await _viewComponentHelper.InvokeAsync(typeof(GridViewViewComponent), DTO);

            output.Content.AppendHtml(content);
            output.Content.AppendHtml($"<script>document.addEventListener('DOMContentLoaded', function () {{ gv('{Id}').Init() }})</script>");
        }

        public string Id { get; set; }
        public string ExcelRowColumns { get; set; } = "row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-4 row-cols-xl-5 g-1";
        public string TranslateFormName { get; set; }



        public string ResponseUrl { get; set; }
        public string ColumnKeyName { get; set; }
        public ViewTypeEnum ViewType { get; set; } = ViewTypeEnum.Grid;




        public string CustomizationPartialViewAddress { get; set; }

        public bool AutoFillOnClientSide { get; set; } = true;
        public bool MenuGenerationIsInCustom { get; set; } = false;
    }


    public class GridViewActionTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            List<GridViewAction> list = (List<GridViewAction>)context.Items["GridViewActions"];
            list.Add(new GridViewAction()
            {
                Title = Title,
                OnClick = OnClick,
            });


            context.Items.Remove("GridViewActions");
            context.Items.Add("GridViewActions", list);

            output.SuppressOutput();
        }

        public string Title { get; set; }

        public string OnClick { get; set; }
    }

    public class GridViewColumnTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (Type == GridViewColumnType.Button && string.IsNullOrEmpty(ButtonTypeText))
            {
                ButtonTypeText = Title;
            }


            //Template = await output.GetChildContentAsync();

            List<GridViewColumn> list = (List<GridViewColumn>)context.Items["Columns"];
            list.Add(new GridViewColumn()
            {
                Title = Title,
                Name = Name,
                Type = Type,
                FilterType = FilterType,
                FilterItems = FilterItems,
                //Template = Template.GetContent(),
                ButtonTypeClass = ButtonTypeClass,
                OnClick = OnClick,
                ButtonIcon = ButtonIcon,
                ButtonTypeText = ButtonTypeText,
                DateTypeToShortDate = DateTypeToShortDate,
                ColClass = ColClass,
                Width = Width,
                Select2ResponseUrl = Select2ResponseUrl
            });


            context.Items.Remove("Columns");
            context.Items.Add("Columns", list);

            output.SuppressOutput();
        }

        public string Name { get; set; }
        public string Title { get; set; }

        public List<SelectListItem> FilterItems { get; set; }
        public GridViewFilterType FilterType { get; set; } = GridViewFilterType.String;


        public GridViewColumnType Type { get; set; } = GridViewColumnType.String;

        public Func<dynamic, object> Template2;

        public TagHelperContent Template;

        public string OnClick { get; set; }
        public string ButtonIcon { get; set; }
        public string ButtonTypeClass { get; set; }
        public bool DateTypeToShortDate { get; set; }
        public string ButtonTypeText { get; set; }
        public byte? Width { get; set; }
        public string ColClass { get; set; } 
        public string Select2ResponseUrl { get; set; }
    }
}