using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public class ServiceResult<T> where T : BaseEntity
{
    public T? Item { get; set; }
    public IQueryable<T>? Items { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; } = false;
}
