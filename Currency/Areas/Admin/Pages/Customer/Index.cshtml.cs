using Microsoft.AspNetCore.Mvc;

namespace Admin.Customer
{
    public class IndexPageModel : PageModelAbstract
    {
        private readonly CoreChannelClient coreChannelClient;

        public IndexPageModel(CoreChannelClient coreChannelClient)
        {
            this.coreChannelClient = coreChannelClient;
        }

        [BindProperty] public string Title { get; set; }
        [BindProperty] public int? CustomerNo { get; set; }


        public void OnGet()
        {
        }

        //public async Task<ActionResult> OnPostGVResponse(GridViewDTO gridViewDTO)
        //{
        //    var result = await coreChannelClient.Create<global::Customer.ICustomerService>()
        //        .Admin_List(new global::Customer.SearchModel() { Title = Title, CustomerNo = CustomerNo }, gridViewDTO.DataSouceRequest);

        //    if (result.Status == 0) return Json(result);

        //    gridViewDTO.Rows = result.Data.Items.Select(x => new GridViewRow() { Data = x }).ToList();
        //    gridViewDTO.Count = result.Data.TotalCount;

        //    return ViewComponent("GridView", gridViewDTO);
        //}

        //public async Task<JsonResult> OnPostSelect2Response(Select2DTO dto)
        //{
        //    if (dto.PageSize == 0) dto.PageSize = 20;
        //    if (dto.PageNumber == 0) dto.PageNumber = 1;

        //    var dr = new DataSourceRequestDTO() { PageSize = (int)dto.PageSize, PageNumber = (int)dto.PageNumber };
        //    //if (!string.IsNullOrEmpty(dto.Query)) dr.Filters.Add(new DataSourceRequestFilter()
        //    //{ Field = "Title", Operator = FilterOperator.Contains, Value = dto.Query });

        //    var searchModel = new global::Customer.SearchModel() { };

        //    var isNumeric = int.TryParse(dto.Query, out int n);
        //    if (isNumeric) searchModel.CustomerNo = n; else searchModel.Title = dto.Query;


        //    var result = await coreChannelClient.Create<global::Customer.ICustomerService>().Admin_List(searchModel, dr);



        //    dto.Items = result.Data.Items.Select(x => new Select2Item()
        //    {
        //        Id = x.Id.ToString(),
        //        ViewTemplate = $@"<div>{x.Title} - {x.No}</div>",
        //        SelectionTemplate = $"{x.Title}",
        //    }).ToList();

        //    return new JsonResult(dto);
        //}

        //public async Task<IActionResult> OnPostQuickInfo(Guid Id)
        //{
        //    var result = await coreChannelClient.Create<global::Customer.ICustomerService>().Admin_Info(Id);
        //    return Partial("QuickInfo", result);
        //}
    }
}
