using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public enum OrderType
{
    NON,
    PRC,
    TRT,
    EXM,
    DOC,
    MED,
    ETC
}

public partial class OrderViewModel : BaseViewModel<Order>
{
    private OrderType _orderType = OrderType.NON;

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
        var getItem = new Order { ORD_IsUse = true, PageSize = Model.PageSize };

        switch (_orderType)
        {
            case OrderType.NON:
                getItem.ORD_IsQuickOrder = true;
                break;

            case OrderType.PRC:
                getItem.ORDC_Cd = "PRC";
                break;

            case OrderType.TRT:
                getItem.ORDC_Cd = "TRT";
                break;

            case OrderType.EXM:
                getItem.ORDC_Cd = "EXM";
                break;

            case OrderType.DOC:
                getItem.ORDC_Cd = "DOC";
                break;

            case OrderType.ETC:
                getItem.ORDC_Cd = "ETC";
                break;
        }

        var ret = await SmartMVVM.DataStore.GetItems<Order>(eAPI.Order_GetOrder, getItem);
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("오더를 불러오는데 실패했습니다.", NotificationType.Error);
            return false;
        }

        DisplayDataMappers.OrderDisplayDataMapper.Map(ret, _orderType);

        Orders = ret.ToList();

        return true;
    }

    protected override Order GetModel(Order item)
    {
        item.ORD_InsuranceType = "";

        return item;
    }

    public void SetOrderType(OrderType orderType)
    {
        _orderType = orderType;
    
        if (_orderType == OrderType.NON)
        {
            Model.PageSize = 100;
        }
        else
        {
            Model.PageSize = 15;
        }
    }

    [RelayCommand]
    public async Task AddORDItem(object paramItem)
    {
        if (paramItem is not Order order) return;

        await SmartUI.SendMessage("AddORDItem", order, TargetViewType.PageView);
    }

    [RelayCommand]
    public async Task Search()
    {
        await FetchDataAsync();
    }

    [RelayCommand]
    public void Reset()
    {
        Model.Keyword = "";
    }
}
