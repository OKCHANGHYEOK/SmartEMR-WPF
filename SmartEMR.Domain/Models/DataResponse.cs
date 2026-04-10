namespace SmartEMR.Domain.Models;
public class DataResponse<T> where T : class
{
    public T? Item { get; set; }
    public List<T>? Items { get; set; }
    public string? Message { get; set; }
    public int ResponseCode { get; set; }
    public int? TotalCount { get; set; }
    public bool? IsSuccess { get; set; }
}
