using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Diagnostics;

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

    private readonly IConsultationService _consultationService;
    private readonly IConsultationOrderService _consultationOrderService;

    [ObservableProperty]
    private List<PayItem> payItems = new();
    [ObservableProperty]
    private List<ConsultationOrder> consultationOrders = default!;

    private List<ConsultationOrder> _defaultGroupHeaders = new List<ConsultationOrder>
    {
        new ConsultationOrder { CSTO_InsuranceTypeName = "급여", IsVisible = false },
        new ConsultationOrder { CSTO_InsuranceTypeName = "비급여", IsVisible = false }
    };

    public PayInfoViewModel(IPayService payService, IConsultationService consultationService, IConsultationOrderService consultationOrderService) : base(payService) 
    {
        _consultationService = consultationService;
        _consultationOrderService = consultationOrderService;
    }

    protected override Pay GetModel(Pay item)
    {
        SmartMVVM.ModelProperty.SetDefaultPayData(item);
        return item;
    }

    public async Task UpdatePayInfo(Pay item, bool isClear = true)
    {
        if (item.CST_Idx.GetValueOrDefault(0) == 0 || item.PAY_Idx.GetValueOrDefault(0) == 0) return;

        if (isClear)
        {
            ClearData();
        }

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

        ConsultationOrders = [];
        PayItems = [];
    }

    public void SetAllPrice()
    {
        Model.PAY_PriceForPay = (int?)Model.PAY_RemainPrice.GetValueOrDefault(0);

        SmartUI.SetNotification("전액입력되었습니다.", NotificationType.Info);
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode saveMode)
    {
        await SmartUI.SendMessage("RefreshPAY", viewType:TargetViewType.PageView);

        SmartUI.SetNotification($"수납{(saveMode == SaveMode.SAVE ? "완료" : "취소" )}되었습니다.", NotificationType.Success);
    }

    private async Task UpdateSelectedCST(int CST_Idx)
    {
        if (CST_Idx == 0) return;

        var ret = await _consultationService.GetConsultation(new Consultation { CST_Idx = CST_Idx });
        if (ret.Item is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        SmartMVVM.ModelProperty.SetConsultationData(SelectedCST, ret.Item);

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
        var ret = await _payService.GetPayItems(new PayItem { PAY_Idx = Model.PAY_Idx, SortField = "PAYI_Idx", SortDir = "desc" });
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        PayItems = [.. ret.Items];
    }

    [RelayCommand]
    private async Task CompletePay()
    {
        if (Model.PAY_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNotification("선택된 수납이 없습니다.", NotificationType.Warning);
            return;
        }

        if (Model.PAY_RemainPrice > 0)
        {
            var inlines = new List<Inline>
            {
                new Run("미수납금이 남아있습니다.\n수납완료 처리하시겠습니까?") { FontSize = 16 },
                new LineBreak(),
                new Run("(청구시 문제가 발생할 수 있습니다.)") { FontSize = 14, Foreground = Brushes.IndianRed }
            };

            if (SmartUI.MsgYesNo(inlines) is MessageBoxResult.No) return;
        }

        Model.PAY_Status = "END";

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

        var inlines = new List<Inline>
        {
            new Run("수납취소하시겠습니까?") { FontSize = 16 },
            new LineBreak(),
            new Run("(모든 수납 이력이 삭제되고 수납대기 상태로 돌아갑니다.)") { FontSize = 14, Foreground = Brushes.IndianRed }
        };

        if (SmartUI.MsgYesNo(inlines) is MessageBoxResult.No) return;

        var ret = await _payService.CancelPay(Model.PAY_Idx.GetValueOrDefault(0));
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return;
        }

        await NotifyCompletedTaskAsync(SaveMode.DELETE);
        await UpdatePayInfo(ret.Item, isClear:false);
    }

    public async Task SetPayItem(PayType type, PayMethod method = PayMethod.None)
    {
        var price = type switch
        {
            PayType.Payment => Model.PAY_PriceForPay,
            PayType.Discount => Model.PAY_DiscountPrice,
            PayType.Cutting => Model.PAY_CutUnit,
            PayType.Refund => Model.PAY_PaidPrice,
            _ => 0
        };

        if (price is not decimal finalPrice) return;

        if (!CanPayment(type, finalPrice))
            return;

        ServiceResult<PayItem>? result = null;

        switch (type)
        {
            case PayType.Payment:
                result = await PaymentPriceAsync(method, finalPrice);
                break;

            case PayType.Refund:
                result = await RefundPriceAsync(finalPrice);
                break;

            case PayType.Cutting:
                result = await CuttingPriceAsync(finalPrice);
                break;

            case PayType.Discount:
                result = await DiscountPriceAsync(finalPrice);
                break;
        }

        if (result is null || result.Item is null || !result.IsSuccess) return;

        var retPAY = await _payService.GetPay(new Pay { PAY_Idx =  Model.PAY_Idx });
        if (retPAY.Item is null || !retPAY.IsSuccess)
        {
            SmartUI.SetNotification(retPAY.Message ?? "", NotificationType.Error);
            return;
        }

        string payTypeName = type switch
        {
            PayType.Payment => "수납",
            PayType.Cutting => "절사",
            PayType.Discount => "할인",
            PayType.Refund => "환불",
            _ => ""
        };

        SmartUI.SetNotification($"{payTypeName}처리되었습니다.", NotificationType.Success);

        await UpdatePayInfo(retPAY.Item);
        await SmartUI.SendMessage("RefreshPAY", viewType:TargetViewType.PageView);
    }

    private async Task<ServiceResult<PayItem>?> PaymentPriceAsync(PayMethod method, decimal price)
    {
        try
        {
            string methodName = method switch
            {
                PayMethod.Cash => "현금",
                PayMethod.Card => "카드",
                PayMethod.NaverPay => "네이버페이",
                _ => throw new ArgumentOutOfRangeException(nameof(method))
            };

            if (SmartUI.MsgYesNo($"{price}원 {methodName}결제 처리하시겠습니까?") is MessageBoxResult.No)
                return null;

            // 네이버페이 API 요청 로직
            if (method == PayMethod.NaverPay)
            {
                if (!await RequestNaverPayment(price))
                {
                    SmartUI.SetNotification("결제 실패했습니다.", NotificationType.Warning);
                    return null;
                }
            }

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

    private bool CanPayment(PayType type, decimal price)
    {
        if (type == PayType.Refund)
        {
            if (price <= 0) 
            { 
                SmartUI.SetNotification("환불할 금액이 없습니다.", NotificationType.Warning);
                return false;
            }

            return true;
        }

        if (Model.PAY_Status == "END")
        {
            SmartUI.SetNotification("이미 완료된 수납입니다.\n수납취소후 다시 시도해주세요.", NotificationType.Warning);
            return false;
        }

        if (Model.PAY_RemainPrice == 0)
        {
            SmartUI.SetNotification("미수납금이 0원입니다.\n수납취소후 다시 시도해주세요.", NotificationType.Warning);
            return false;
        }

        switch (type)
        {
            case PayType.Payment:
                {
                    if (price <= 0)
                    {
                        SmartUI.SetNotification("수납금액은 0원보다 커야합니다.", NotificationType.Warning);
                        return false;
                    }

                    if (price > Model.PAY_RemainPrice)
                    {
                        SmartUI.SetNotification("수납금액은 미수납금보다 클 수 없습니다.", NotificationType.Warning);
                        return false;
                    }

                    break;
                }

            case PayType.Cutting:
                if (price <= 0)
                {
                    SmartUI.SetNotification("절사단위는 0원보다 커야합니다.", NotificationType.Warning);
                    return false;
                }

                if (Model.PAY_RemainPrice % price == 0)
                {
                    SmartUI.SetNotification($"미수납금이 절사 단위와 일치합니다.\n절사 처리할 수 없습니다.", NotificationType.Warning);
                    return false;
                }

                break;

            case PayType.Discount:
                if (price <= 0)
                {
                    SmartUI.SetNotification("할인금액은 0원보다 커야합니다.", NotificationType.Warning);
                    return false;
                }

                if (price > Model.PAY_RemainPrice)
                {
                    SmartUI.SetNotification("할인금액은 미수납금보다 클 수 없습니다.", NotificationType.Warning);
                    return false;
                }

                break;
        }

        return true;
    }

    private async Task<bool> RequestNaverPayment(decimal price)
    {
        return true;
    }

    private async Task<ServiceResult<PayItem>?> RefundPriceAsync(decimal price)
    {
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
        if (SmartUI.MsgYesNo($"{price}원단위 절사하시겠습니까?") is MessageBoxResult.No)
            return null;

        var item = new PayItem
        {
            PAY_Idx = Model.PAY_Idx,
            PAYI_Type = "CUT",
            PAYI_Price = Model.PAY_RemainPrice % price
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
}
