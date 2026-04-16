using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public abstract partial class BaseViewModel<T> : ObservableObject, IViewModel<T> where T : BaseEntity, new()
{

    #region "Fields"

    [ObservableProperty]
    private T m_Model;

    object IVIewModel.Model => this.Model ?? new T();

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


    public abstract void Initialize();

    protected abstract T GetModel(T item);


    public BaseViewModel()
    {
        Initialize();

        Model = GetModel(new T());
    }

    public BaseViewModel(T item)
    {
        Initialize();

        Model = GetModel(item);
    }
}
