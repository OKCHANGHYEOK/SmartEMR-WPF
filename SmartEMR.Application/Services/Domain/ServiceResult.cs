using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Services.Domain;

public class ServiceResult
{
    public string? Message { get; set; }
    public bool IsSuccess { get; set; } = false;
}

public class ServiceResult<T> : ServiceResult where T : BaseEntity
{
    public T? Item { get; set; }
    public IQueryable<T>? Items { get; set; }
}
