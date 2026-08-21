using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public enum OrderViewType
{
    NON,
    PRC,
    TRT,
    EXM,
    DOC,
    ETC
}

public partial class OrderViewModel : BaseViewModel<Order>
{
    private OrderViewType _orderViewType = OrderViewType.NON;

    [ObservableProperty]
    private List<Order>? orders;

    public override void Initialize()
    {
        
    }

    public override async Task InitializeAsync()
    {
    }

    public override async Task<bool> FetchDataAsync()
    {
        var getItem = new Order { ORD_IsUse = true };

        switch (_orderViewType)
        {
            case OrderViewType.NON:
                getItem.ORD_IsQuickOrder = true;
                break;

            case OrderViewType.PRC:
                getItem.ORDC_Cd = "PRC";
                break;

            case OrderViewType.TRT:
                getItem.ORDC_Cd = "TRT";
                break;

            case OrderViewType.EXM:
                getItem.ORDC_Cd = "EXM";
                break;

            case OrderViewType.DOC:
                getItem.ORDC_Cd = "DOC";
                break;

            case OrderViewType.ETC:
                getItem.ORDC_Cd = "ETC";
                break;
        }

        var ret = await SmartMVVM.DataStore.GetItems<Order>(eAPI.Order_GetOrder, getItem);
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("오더를 불러오는데 실패했습니다.", NotificationType.Error);
            return false;
        }

        DisplayDataMappers.OrderDisplayDataMapper.Map(ret, _orderViewType);

        Orders = ret.ToList();

        return true;
    }

    protected override Order GetModel(Order item)
    {
        return item;
    }

    public void SetOrderViewType(OrderViewType orderViewType)
    {
        _orderViewType = orderViewType;
    }

    [RelayCommand]
    public async Task AddORDItem(object paramItem)
    {
        if (paramItem is not Order order) return;

        await SmartUI.SendMessage("AddORDItem", order, TargetViewType.PageView);
    }
}
