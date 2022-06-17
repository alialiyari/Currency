
using System.ComponentModel;


public enum ActiveStatusEnum : byte
{
    [Description(" فعال")] Active = 1,
    [Description(" غیر فعال")] InActive = 2
}


public enum VerifyStatusEnum : byte
{
    [Description(" درخواست")] Requested = 1,
    [Description("تایید شده")] Verified = 2,
    [Description("رد شده")] Rejected = 3
}

public enum GenderEnum : byte
{
    [Description("مرد ")] Man = 1,
    [Description("زن ")] Woman = 2
}