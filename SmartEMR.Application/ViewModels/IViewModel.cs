using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public interface IVIewModel
{
    void Initialize() { }
}

public interface IViewModel<T> : IVIewModel where T : BaseEntity, new()
{
    T Model { get; set; }
}
