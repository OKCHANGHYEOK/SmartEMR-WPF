using SmartEMR.Domain.Enums;

namespace SmartEMR.Domain.DTOs;

public class DataResponse
{
    public string? Message { get; set; }
    public eResponseCode ResponseCode { get; set; }
    public int? TotalCount { get; set; }
    public bool IsSuccess { get; set; }
}

public class DataResponse<T> : DataResponse where T : class
{
    public T? Item { get; set; }
    public List<T>? Items { get; set; }
}
