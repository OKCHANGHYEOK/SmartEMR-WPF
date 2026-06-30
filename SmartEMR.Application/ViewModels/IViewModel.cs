using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public interface IViewModel
{
    void Initialize() { }
}

public interface IViewModel<T> : IViewModel where T : BaseEntity, new()
{
    T Model { get; set; }
}
