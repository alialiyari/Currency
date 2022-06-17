using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

public enum GridViewFilterType
{
    None,
    String,

    Date,
    Number,
    Enum,
    Select2,
}
public enum GridViewColumnType
{
    String,
    Number,
    Date,
    Time,
    PersianDate,
    Button,
    Checkbox,
    Custom
}

public enum ActionType
{
    CustomColumnGeneration,
    DetailGenerate,
    CardGenerate,
    ExcelExport,
    MenuGenerate
}

public enum ViewTypeEnum
{
    Grid,
    Card,
    ExcelExport
}

public class GridViewAction
{
    public string Title { get; set; }
    public string OnClick { get; set; }
}

public class GridViewColumn
{
    public string Name { get; set; }
    public string Title { get; set; }


    public List<SelectListItem> FilterItems { get; set; }
    public GridViewFilterType FilterType { get; set; } = GridViewFilterType.String;


    public GridViewColumnType Type { get; set; } = GridViewColumnType.String;
    public string ButtonIcon { get; set; }

    public string OnClick { get; set; }
    public string ButtonTypeClass { get; set; }
    public string ButtonTypeText { get; set; }
    public bool DateTypeToShortDate { get; set; }
    public int? Width { get; set; }
    public string ColClass { get; set; }

    public string Select2ResponseUrl { get; set; }
}

public class GridViewRow
{
    public dynamic Data { get; set; } = new object();

    public List<dynamic> Details = new List<dynamic>();
    public bool HasDetail { get; set; } = false;

}

public class GridViewPartialRenderDTO
{
    public string Id { get; set; }
    public long RowIndex { get; set; }
    public ActionType Action { get; set; }
    public GridViewRow Row { get; set; }
    public GridViewColumn Column { get; set; }
    public string CustomizationPartialViewAddress { get; set; }
}

public class GridViewDTO
{
    public string Id { get; set; }
    public long? Count { get; set; }

    public bool FirstLoad { get; set; } = true;



    public List<GridViewColumn> Columns { get; set; }
    public List<GridViewAction> Actions { get; set; }


    public List<GridViewRow> Rows { get; set; } = new List<GridViewRow>();
    public DataSourceRequestDTO DataSouceRequest { get; set; } = new DataSourceRequestDTO();



    public bool MenuGenerationIsInCustom { get; set; } = false;
    public string CustomizationPartialViewAddress { get; set; }

    public ViewTypeEnum ViewType { get; set; }


    public string ColumnKeyName { get; set; }
    public string ExcelRowColumns { get; set; }


    public string ActionsSerialazed { get; set; }
    public string ColumnsSerialazed { get; set; }
}