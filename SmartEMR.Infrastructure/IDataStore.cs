using SmartEMR.Domain.Enums;

namespace SmartEMR.Infrastructure;

public interface IDataStore
{
    public string? retMessage { get; set; }
    public int? retStatusCode { get; set; }
    public int? retCount { get; set; }
    public bool retIsSuccess { get; set; }

    public Task<T?> GetItem<T>(eAPI path, object? paramItem = null) where T : class;
    public Task<IQueryable<T>> GetItems<T>(eAPI path, object? paramItem = null) where T : class;
}
