using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public abstract partial class BaseViewModel<T> : ObservableObject, IBaseViewModel, IViewModel<T> where T : BaseEntity, new()
{
    #region "Fields"

    [ObservableProperty]
    private T m_Model;

    // IVIewModel interface 구현 (명시적 구현)
    object IVIewModel.Model => this.Model ?? new T();

    public IAsyncRelayCommand LoadDataCommand { get; }

    #endregion

    #region "Constructors"

    // 1. 매개변수가 없는 생성자는 기본 객체(new T())를 생성하여 아래 생성자로 넘깁니다 (중복 제거)
    public BaseViewModel() : this(new T())
    {
    }

    // 2. 실제 초기화 로직을 담당하는 핵심 생성자
    public BaseViewModel(T item)
    {
        // 커맨드를 먼저 안전하게 생성합니다.
        LoadDataCommand = new AsyncRelayCommand(OnLoadDataAsync);

        // 모델을 설정합니다.
        Model = GetModel(item);

        Initialize();
    }

    #endregion

    #region "Functions"

    public T GetCurrentModel(T item)
    {
        var model = GetModel(item);
        return model ?? new T();
    }

    // 자식 클래스에서 반드시 구현해야 하는 추상 메서드들
    protected abstract T GetModel(T item);

    public abstract void Initialize();

    // 기본 비동기 데이터 로드 동작 (필요 시 자식에서 override)
    protected virtual Task OnLoadDataAsync()
    {
        return Task.CompletedTask;
    }

    #endregion
}