using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class OrderViewModel : BaseViewModel<Order>
{
    [ObservableProperty]
    private List<Order>? procedures;            // 시술
    [ObservableProperty]
    private List<Order>? treatments;            // 처치
    [ObservableProperty]
    private List<Order>? examinations;          // 검사
    [ObservableProperty]
    private List<Order>? documents;             // 문서
    [ObservableProperty]
    private List<Order>? others;                // 기타

    public override void Initialize()
    {
        
    }

    public override async Task InitializeAsync()
    {
        var ret = await SmartMVVM.DataStore.GetItems<Order>(eAPI.Order_GetOrder, new Order { ORD_IsQuickOrder = true });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("기본 오더를 불러오는데 실패했습니다.", NotificationType.Error);
            return;
        }


    }

    protected override Order GetModel(Order item)
    {
        return item;
    }
}
