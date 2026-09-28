using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Schemas;

public enum NaverPayResultCode
{
    Success,
    UserCancel,
    TimeExpired,
    UnderAgeAmountLimit
}

public class NaverPayRequest
{
    public string? merchantPayKey { get; set; }
    public string? productName { get; set; }
    public int? productCount { get; set; }
    public int? totalPayAmount { get; set; }
    public int? taxScopeAmount { get; set; }
    public int? taxExScopeAmount { get; set; }
    public string? returnUrl { get; set; }
}

public class NaverPayResponse
{
    public NaverPay Item { get; set; } = new();
    public string? Message { get; set; }
    public bool IsSuccess { get; set; } = false;
}