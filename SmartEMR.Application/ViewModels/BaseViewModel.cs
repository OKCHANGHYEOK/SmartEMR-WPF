using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SmartEMR.Application.ViewModels;

public abstract partial class BaseViewModel<T> : ObservableObject where T : BaseEntity, new()
{

    #region "Fields"

    [ObservableProperty]
    private T? m_Model = null;

    #endregion

    #region "Functions"
    public T? GetCurrentModel(T? item)
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

    protected abstract T? GetModel(T? item);


    public BaseViewModel()
    {
        Initialize();

        if (Model == null)
        {
            Model = GetModel(new T());
        }
    }

    public BaseViewModel(T? item)
    {
        Initialize();

        if (Model == null)
        {
            Model = GetModel(item);
        }
    }
}
