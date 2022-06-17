using MessagePack;
using System.Collections.Generic;
using System.ComponentModel;


[MessagePackObject(true)]
public class DataSourceRequestDTO
{
    public int PageSize { get; set; } = 20;
    public int PageNumber { get; set; } = 1;


    public List<DataSourceRequestFilter> Filters { get; set; } = new List<DataSourceRequestFilter>();
    public List<DataSourceRequestScopeFilter> ScopesFilter { get; set; } = new List<DataSourceRequestScopeFilter>();
}

[MessagePackObject(true)]
public class DataSourceRequestFilter
{
    public string Field { get; set; }

    public bool IsIsNullConditional { get; set; } = false;
    public FilterOperator Operator { get; set; } = FilterOperator.IsEqualTo;

    public string Value { get; set; }
    public string ValueTitle { get; set; }
}


[MessagePackObject(true)]
public class DataSourceRequestScopeFilter
{
    public FilterConditionEnum Condition { get; set; }
    public List<DataSourceRequestFilter> Filters { get; set; } = new List<DataSourceRequestFilter>();
    public List<DataSourceRequestScopeFilter> Scopes { get; set; } = new List<DataSourceRequestScopeFilter>();
}


public enum FilterConditionEnum
{
    Or,
    And
}

public enum Select2FilterOperator
{
    [Description("برابر باشد")] IsEqualTo = 3,
    [Description("برابر نباشد")] IsNotEqualTo = 4,
}

public enum StringFilterOperator
{
    [Description("برابر باشد")] IsEqualTo = 3,
    [Description("برابر نباشد")] IsNotEqualTo = 4,

    [Description("شامل")] Contains = 9,
    [Description("شامل نباشد")] NotContains = 10,
}


public enum NumberFilterOperator
{
    [Description("برابر باشد")]IsEqualTo = 3,
    [Description("برابر نباشد")] IsNotEqualTo = 4,

    [Description("کمتر از")] IsLessThan = 1,
    [Description("کمتر یا برابر")] IsLessThanOrEqualTo = 2,

    [Description("بیشتر یا برابر")] IsGreaterThanOrEqualTo = 5,
    [Description("بیشتر از")] IsGreaterThan = 6,
}


public enum FilterOperator
{

    [Description("کمتر از")] IsLessThan = 1,
    [Description("کمتر یا برابر")] IsLessThanOrEqualTo = 2,
   
    [Description("برابر باشد")] IsEqualTo = 3,
    [Description("برابر نباشد")] IsNotEqualTo = 4,

    [Description("بیشتر یا برابر")] IsGreaterThanOrEqualTo = 5,
    [Description("بیشتر از")] IsGreaterThan = 6,


    //StartsWith = 7,
    //EndsWith = 8,
    [Description("شامل")] Contains = 9,
    [Description("شامل نباشد")] NotContains = 10,

    //IsContainedIn = 10,
    //DoesNotContain = 11,
    //IsNull = 12,
    //IsNotNull = 13,
    //IsEmpty = 14,
    //IsNotEmpty = 15,
    //IsNullOrEmpty = 16,
    //IsNotNullOrEmpty = 17
}
