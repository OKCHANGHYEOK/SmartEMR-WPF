using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Diagnostics;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public enum PayType
{
    Payment,
    Refund,
    Cutting,
    Discount
}

public enum PayMethod
{
    None,
    Cash,
    Card,
    NaverPay
}

public partial class PayInfoViewModel : PayViewModel
{
    public Consultation SelectedCST { get; set; } = new();

    private IConsultationOrderService _consultationOrderService;

    [ObservableProperty]
    private List<PayItem> payItems = new();
    [ObservableProperty]
    private List<ConsultationOrder> consultationOrders = default!;

    private List<ConsultationOrder> _defaultGroupHeaders = new List<ConsultationOrder>
    {
        new ConsultationOrder { CSTO_InsuranceTypeName = "급여", IsVisible = false },
        new ConsultationOrder { CSTO_InsuranceTypeName = "비급여", IsVisible = false }
    };

    public PayInfoViewModel(IPayService payService, IConsultationOrderService consultationOrderService) : base(payService) 
    {
        _consultationOrderService = consultationOrderService;
    }

    protected override Pay GetModel(Pay item)
    {
        SmartMVVM.ModelProperty.SetDefaultPayData(item);
        return item;
    }

    public async Task UpdatePayInfo(Pay item)
    {
        if (item.CST_Idx.GetValueOrDefault(0) == 0 || item.PAY_Idx.GetValueOrDefault(0) == 0) return;

        ClearData();

        SmartMVVM.ModelProperty.SetPayData(Model, item);

        await UpdateSelectedCST(item.CST_Idx.GetValueOrDefault(0));
        await UpdatePayItems();
    }

    public void UpdatePriceData(Pay item)
    {
        SmartMVVM.ModelProperty.SetPayPriceData(Model, item);
    }

    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearCSTData(SelectedCST);
        SmartMVVM.ModelProperty.ClearPAYData(Model);

        ClearPayItems();

        ConsultationOrders = new();
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode saveMode)
    {
        await SmartUI.SendMessage("RefreshPAY", viewType:TargetViewType.PageView);

        if (saveMode == SaveMode.DELETE)
        {
            ClearPayItems();
        }
    }

    private async Task UpdateSelectedCST(int CST_Idx)
    {
        if (CST_Idx == 0) return;

        var ret = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_GetConsultation, new Consultation { CST_Idx = CST_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNotification("진료 정보가 유효하지 않습니다.", NotificationType.Error);
            return;
        }

        SmartMVVM.ModelProperty.SetConsultationData(SelectedCST, ret);

        await UpdateCSTOData();
    }

    private async Task UpdateCSTOData()
    {
        var ret = await _consultationOrderService.GetConsultationOrders(new ConsultationOrder { CST_Idx = SelectedCST.CST_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        ConsultationOrders = [.. _defaultGroupHeaders, .. ret.Items];
    }

    private async Task UpdatePayItems()
    {
        var ret = await _payService.GetPayItems(new PayItem { PAY_Idx = Model.PAY_Idx });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        PayItems = [.. ret.Items];
    }


    [RelayCommand]
    private async Task SetPay()
    {
        if (Model.PAY_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNotification("선택된 수납이 없습니다.", NotificationType.Warning);
            return;
        }

        var ret = await _payService.SetPay(Model);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        SmartMVVM.ModelProperty.SetPayData(Model, ret.Item);

        await NotifyCompletedTaskAsync(SaveMode.SAVE);
    }

    [RelayCommand]
    private async Task CancelPay()
    {
        if (Model.PAY_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNotification("선택된 수납이 없습니다.", NotificationType.Warning);
            return;
        }

        var ret = await _payService.CancelPay(Model.PAY_Idx.GetValueOrDefault(0));
        if (!ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        await NotifyCompletedTaskAsync(SaveMode.DELETE);
    }

    public async Task SetPayItem(PayType type, PayMethod method = PayMethod.None, decimal price = 0)
    {
        ServiceResult<PayItem>? result = null;

        switch (type)
        {
            case PayType.Payment:
                result = await PaymentPriceAsync(method, price);
                break;

            case PayType.Refund:
                result = await RefundPriceAsync(price);
                break;

            case PayType.Cutting:
                result = await CuttingPriceAsync(price);
                break;

            case PayType.Discount:
                result = await DiscountPriceAsync(price);
                break;
        }

        if (result is null || result.Item is null || !result.IsSuccess) return;

        var retPAY = await _payService.GetPay(new Pay { PAY_Idx =  Model.PAY_Idx });
        if (retPAY.Item is null || !retPAY.IsSuccess)
        {
            SmartUI.SetNotification(retPAY.Message ?? "", NotificationType.Error);
            return;
        }

        await UpdatePayInfo(retPAY.Item);
    }

    private async Task<ServiceResult<PayItem>?> PaymentPriceAsync(PayMethod method, decimal price)
    {
        if (price <= 0)
        {
            SmartUI.SetNotification("수납금액은 0원보다 커야합니다.", NotificationType.Warning);
            return null;
        }

        if (SmartUI.MsgYesNo($"{price}원 수납하시겠습니까?") is MessageBoxResult.No)
            return null;

        try
        {
            string PAY_Method = method switch
            {
                PayMethod.Cash => "CAS",
                PayMethod.Card => "CRD",
                PayMethod.NaverPay => "NAV",
                _ => throw new ArgumentOutOfRangeException(nameof(method))
            };

            var item = new PayItem
            {
                PAY_Idx = Model.PAY_Idx,
                PAYI_Type = "PAY",
                PAYI_Method = PAY_Method,
                PAYI_Price = price
            };

            var ret = await _payService.SetPayItem(item);
            if (ret.Item is null || !ret.IsSuccess)
            {
                SmartUI.SetNotification("수납 처리하지 못했습니다.", NotificationType.Error);
                return null;
            }

            return ret;
        }
        catch (ArgumentOutOfRangeException e)
        {
            Debug.WriteLine(e.StackTrace);
            return null;
        }
    }

    private async Task<ServiceResult<PayItem>?> RefundPriceAsync(decimal price)
    {
        if (price <= 0)
        {
            SmartUI.SetNotification("환불금액은 0원보다 커야합니다.", NotificationType.Warning);
            return null;
        }

        if (SmartUI.MsgYesNo($"{price}원 환불하시겠습니까?") is MessageBoxResult.No)
            return null;

        var item = new PayItem
        {
            PAY_Idx = Model.PAY_Idx,
            PAYI_Type = "REF",
            PAYI_Price = price
        };

        var ret = await _payService.SetPayItem(item);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification("환불 처리하지 못했습니다.", NotificationType.Error);
            return null;
        }

        return ret;
    }

    private async Task<ServiceResult<PayItem>?> CuttingPriceAsync(decimal price)
    {
        if (price <= 0)
        {
            SmartUI.SetNotification("절사기준 금액은 0보다 커야합니다.", NotificationType.Warning);
            return null;
        }

        if (SmartUI.MsgYesNo($"{price}단위로 절사하시겠습니까?") is MessageBoxResult.No)
            return null;

        var item = new PayItem
        {
            PAY_Idx = Model.PAY_Idx,
            PAYI_Type = "CUT",
            PAYI_Price = price
        };

        var ret = await _payService.SetPayItem(item);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification("절사 처리하지 못했습니다.", NotificationType.Error);
            return null;
        }

        return ret;
    }

    private async Task<ServiceResult<PayItem>?> DiscountPriceAsync(decimal price)
    {
        if (price <= 0)
        {
            SmartUI.SetNotification("할인금액은 0원보다 커야합니다.", NotificationType.Warning);
            return null;
        }

        if (SmartUI.MsgYesNo($"{price}원 할인하시겠습니까?") is MessageBoxResult.No)
            return null;

        var item = new PayItem
        {
            PAY_Idx = Model.PAY_Idx,
            PAYI_Type = "DIS",
            PAYI_Price = price
        };

        var ret = await _payService.SetPayItem(item);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification("할인 처리하지 못했습니다.", NotificationType.Error);
            return null;
        }

        return ret;
    }

    private void ClearPayItems()
    {
        PayItems = [];
    }
}
