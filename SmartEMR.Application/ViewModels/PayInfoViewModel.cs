using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class PayInfoViewModel : PayViewModel
{
    public Consultation SelectedCST { get; set; } = new();

    private IConsultationOrderService _consultationOrderService;

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

        await UpdateSelectedCST(item.CST_Idx.GetValueOrDefault(0));

        UpdatePriceData(item);
    }

    public void UpdatePriceData(Pay item)
    {
        SmartMVVM.ModelProperty.SetPayData(Model, item);
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

    private void ClearData()
    {
        SmartMVVM.ModelProperty.ClearCSTData(SelectedCST);
        SmartMVVM.ModelProperty.ClearPAYData(Model);
    }
}
