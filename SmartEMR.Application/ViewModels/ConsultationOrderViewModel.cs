using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.ViewModels;

public partial class ConsultationOrderViewModel : BaseViewModel<ConsultationOrder>
{
    [ObservableProperty]
    private ObservableCollection<ConsultationOrder> consultationOrderItems = new();

    private Consultation SelectedCST = new();

    public override void Initialize()
    {
    }

    protected override ConsultationOrder GetModel(ConsultationOrder item)
    {
        return item;
    }

    public void AddCSTO(Order item)
    {
        if (ConsultationOrderItems.Count > 0 && ConsultationOrderItems.Any(x => x.ORD_Idx == item.ORD_Idx))
        {
            if (SmartUI.MsgYesNo("동일한 오더가 이미 입력되어있습니다. 계속하시겠습니까?") is System.Windows.MessageBoxResult.No) return;
        }

        var addItem = new ConsultationOrder
        {
            MUR_Idx_DOC = SmartMVVM.AppSession.MemberUser?.MUR_Idx,
            ORD_Idx = item.ORD_Idx,
            PAT_Idx = SelectedCST.PAT_Idx,
            CST_Idx = SelectedCST.CST_Idx,

            ORDC_Cd = item.ORDC_Cd,
            ORDG_Cd = item.ORDG_Cd,
            ORDI_Cd = item.ORDI_Cd,

            CSTO_SugaCode = item.ORD_SugaCode,
            CSTO_ClassCode = item.ORD_ClassCode,
            CSTO_InsuranceType = SmartMVVM.Common.GetOrderInsuranceType(item, SelectedCST),
            CSTO_Status = "RDY",
            CSTO_Name = item.ORD_Name,
            CSTO_Price = item.ORD_Price,
            CSTO_TotalPrice = item.ORD_Price,
            CSTO_Day = 1,
            CSTO_Count = 1,
            CSTO_Amount = 1,
        };

        addItem.vORDC_Cd = SmartMVVM.Master.Query<Order>("ORDC_Cd").FirstOrDefault(x => x.ORDC_Cd == item.ORDC_Cd)?.vORDC_Cd;
        addItem.vCSTO_InsuranceType = SmartMVVM.Common.GetCommonCodeName("ORD", "InsuranceType", addItem.CSTO_InsuranceType);

        ConsultationOrderItems.Add(addItem);
    }
}
