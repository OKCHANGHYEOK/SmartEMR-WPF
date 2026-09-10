using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.ViewModels;

public partial class ConsultationOrderViewModel : BaseViewModel<ConsultationOrder>
{
    [ObservableProperty]
    private ObservableCollection<ConsultationOrder> consultationOrderItems = new();
    private List<ConsultationOrder> deletedItems = new();

    private Patient SelectedPAT = new();
    private Consultation SelectedCST = new();

    private CopaymentType copaymentType
    {
        get
        {
            if (SelectedPAT is not null)
            {
                return SmartMVVM.Common.GetCopaymentType(SelectedPAT);
            }

            return CopaymentType.General;
        }
    }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await SmartUI.SendMessage("SetConsultationOrders", parameters: new IEnumerable<ConsultationOrder>[] { ConsultationOrderItems, deletedItems }, viewType:TargetViewType.PageView);
    }

    protected override ConsultationOrder GetModel(ConsultationOrder item)
    {
        return item;
    }

    public void SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(SelectedPAT, item);
    }

    public async Task UpdateDataBySelectedCST(Consultation item)
    {
        if (item.RCP_Idx.GetValueOrDefault(0) == 0) return;

        SmartMVVM.ModelProperty.SetConsultationData(SelectedCST, item);

        if (item.CST_Idx.GetValueOrDefault(0) > 0)
        {
           await SetCSTOData(item);
        }
    }

    public void AddCSTO(Order item, int MUR_Idx_DOC)
    {
        if (ConsultationOrderItems.Count > 0 && ConsultationOrderItems.Any(x => x.ORD_Idx == item.ORD_Idx))
        {
            if (SmartUI.MsgYesNo("동일한 오더가 이미 입력되어있습니다. 계속하시겠습니까?") is System.Windows.MessageBoxResult.No) return;
        }

        var addItem = new ConsultationOrder
        {
            MUR_Idx_DOC = SmartMVVM.AppSession.MemberUser?.MUR_JobCode == "DOC" ? SmartMVVM.AppSession.MemberUser?.MUR_Idx.GetValueOrDefault(0) : MUR_Idx_DOC,
            ORD_Idx = item.ORD_Idx,
            PAT_Idx = SelectedCST.PAT_Idx,
            CST_Idx = SelectedCST.CST_Idx,

            ORDC_Cd = item.ORDC_Cd,
            ORDG_Cd = item.ORDG_Cd,
            ORDI_Cd = item.ORDI_Cd,

            CSTO_SugaCode = item.ORD_SugaCode,
            CSTO_ClassCode = item.ORD_ClassCode,
            CSTO_InsuranceType = SmartMVVM.Common.GetOrderInsuranceType(item.ORD_InsuranceType ?? "", SelectedCST.CST_InsuranceType ?? ""),
            CSTO_Status = "RDY",
            CSTO_Name = item.ORD_Name,
            CSTO_Price = item.ORD_Price,
            CSTO_TotalPrice = item.ORD_Price,
            CSTO_Day = 1,
            CSTO_Count = 1,
            CSTO_Amount = 1,
            CSTO_ViewIndex = ConsultationOrderItems.Count,
            CSTO_IsValid = true
        };

        addItem.vORDC_Cd = SmartMVVM.Master.Query<Order>("ORDC_Cd").FirstOrDefault(x => x.ORDC_Cd == item.ORDC_Cd)?.vORDC_Cd;
        addItem.vCSTO_InsuranceType = SmartMVVM.Common.GetCommonCodeName("ORD", "InsuranceType", addItem.CSTO_InsuranceType)?[..1];

        ConsultationOrderItems.Add(addItem);

        UpdatePriceData();
    }

    public async void DeleteCSTO(ConsultationOrder item)
    {
        var delItem = ConsultationOrderItems.FirstOrDefault(x => x.ORD_Idx == item.ORD_Idx && x.CSTO_ViewIndex == item.CSTO_ViewIndex);
        if (delItem is not null)
        {
            ConsultationOrderItems.Remove(delItem);

            delItem.CSTO_IsValid = false;
            deletedItems.Add(delItem);

            await SmartUI.SendMessage("DeSelectOrder", new Order { ORD_Idx = item.ORD_Idx }, viewType:TargetViewType.PageView);
        }

        foreach (var order in ConsultationOrderItems)
        {
            order.CSTO_ViewIndex = ConsultationOrderItems.IndexOf(order);
        }

        UpdatePriceData();
    }

    public void UpdateCSTOData(ConsultationOrder item)
    {
        item.CSTO_Amount = item.CSTO_Day * item.CSTO_Count;
        item.CSTO_TotalPrice = item.CSTO_Price * item.CSTO_Amount;

        UpdatePriceData();
    }

    public async void UpdatePriceData()
    {
        var insuredTotal = ConsultationOrderItems.Where(x => x.CSTO_InsuranceType == "INS").Sum(x => x.CSTO_TotalPrice);
        var ownPatientTotal = SmartMVVM.Common.CalculateOwnPatientPrice(insuredTotal.GetValueOrDefault(0), copaymentType);
        var nonInsuredTotal = ConsultationOrderItems.Where(x => x.CSTO_InsuranceType == "NON").Sum(x => x.CSTO_TotalPrice);

        var sendItem = new Pay
        {
            PAY_InsuredPrice = insuredTotal,
            PAY_OwnPatientPrice = ownPatientTotal,
            PAY_NonInsuredPrice = nonInsuredTotal,
            PAY_TotalPrice = insuredTotal + nonInsuredTotal
        };

        await SmartUI.SendMessage("UpdatePayInfo", sendItem, viewType:TargetViewType.PageView);
    }

    public async void ClearData()
    {
        ConsultationOrderItems.Clear();
        deletedItems.Clear();

        UpdatePriceData();

        await SmartUI.SendMessage("ClearSelectedOrder", viewType: TargetViewType.PageView);
    }

    [RelayCommand]
    public async Task ResetCSTO()
    {
        if (SmartUI.MsgYesNo("처방내역을 초기화하시겠습니까?") is System.Windows.MessageBoxResult.No) return;

        ClearData();
    }

    private async Task SetCSTOData(Consultation item)
    {
        ClearData();

        if (item.CST_Idx > 0)
        {
            var getItem = new ConsultationOrder
            {
                CST_Idx = item.CST_Idx,

                SortField = "CSTO_ViewIndex",
                SortDir = "desc"
            };

            var ret = await SmartMVVM.DataStore.GetItems<ConsultationOrder>(eAPI.ConsultationOrder_GetConsultationOrder, getItem);
            if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("처방내역을 불러오지 못했습니다.", NotificationType.Error);
                return;
            }

            DisplayDataMappers.ConsultationOrderDisplayDataMapper.Map(ret);

            foreach (var cItem in ret)
            {
                ConsultationOrderItems.Add(cItem);
            }

            UpdatePriceData();

            await SmartUI.SendMessage("SetSelectedOrders", ret, viewType:TargetViewType.PageView);
        }
    }
}
