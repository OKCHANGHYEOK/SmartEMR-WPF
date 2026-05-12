using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public abstract partial class BaseViewModel<T> : ObservableObject, IBaseViewModel, IViewModel<T> where T : BaseEntity, new()
{

    #region "Fields"

    [ObservableProperty]
    private T m_Model;

    object IVIewModel.Model => this.Model ?? new T();

    public IAsyncRelayCommand LoadDataCommand { get; }

    #endregion

    #region "Functions"
    public T GetCurrentModel(T item)
    {
        var model = GetModel(item);

        if (model == null)
        {
            model = new T();
        }

        return model;
    }
    #endregion


    public virtual void Initialize()
    {
        if (LoadDataCommand.CanExecute(this.Model))
        {
            LoadDataCommand.Execute(this.Model);
        }
    }

    protected abstract T GetModel(T item);


    public BaseViewModel()
    {
        Initialize();

        Model = GetModel(new T());

        LoadDataCommand = new AsyncRelayCommand(OnLoadDataAsync);
    }

    public BaseViewModel(T item)
    {
        Initialize();

        Model = GetModel(item);

        LoadDataCommand = new AsyncRelayCommand(OnLoadDataAsync);
    }

    protected virtual Task OnLoadDataAsync()
    {
        return Task.CompletedTask;
    }
}
