namespace SmartEMR.Application.ViewModels;

public interface IVIewModel
{
    object? Model { get; }
}

public interface IViewModel<out T> : IVIewModel
{
    new T? Model { get; }
}
