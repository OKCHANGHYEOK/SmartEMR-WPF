using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SmartEMR.Application.ViewModels;

public partial class ConsultationOrderViewModel : BaseViewModel<ConsultationOrder>
{
    [ObservableProperty]
    private ObservableCollection<ConsultationOrder> consultationOrderItems = new();
    private List<ConsultationOrder> deletedItems = new();

    private Consultation SelectedCST = new();

    public override void Initialize()
    {
        ConsultationOrderItems.CollectionChanged += OnConsultationOrderItemsChanged;
    }

    protected override ConsultationOrder GetModel(ConsultationOrder item)
    {
        return item;
    }

    public async Task UpdateDataBySelectedCST(Consultation item)
    {
        if (item.CST_Idx.GetValueOrDefault(0) == 0) return;

        var ret = await SmartMVVM.DataStore.GetItems<ConsultationOrder>(eAPI.ConsultationOrder_GetConsultationOrder, new ConsultationOrder { CST_Idx = item.CST_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("처방내역을 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        foreach (var cItem in ret)
        {
            ConsultationOrderItems.Add(cItem);
        }
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
            CSTO_Amount = 1
        };

        addItem.vORDC_Cd = SmartMVVM.Master.Query<Order>("ORDC_Cd").FirstOrDefault(x => x.ORDC_Cd == item.ORDC_Cd)?.vORDC_Cd;
        addItem.vCSTO_InsuranceType = SmartMVVM.Common.GetCommonCodeName("ORD", "InsuranceType", addItem.CSTO_InsuranceType);

        ConsultationOrderItems.Add(addItem);
    }

    public void DeleteCSTO(ConsultationOrder item)
    {
        if (SmartUI.MsgYesNo("삭제하시겠습니까?") is System.Windows.MessageBoxResult.No) return;

        var delCSTO = ConsultationOrderItems.FirstOrDefault(x => x.ORD_Idx == item.ORD_Idx && x.ViewIndex == item.ViewIndex);
        if (delCSTO is not null)
        {
            ConsultationOrderItems.Remove(delCSTO);
        }
    }

    [RelayCommand]
    public void ResetCSTO()
    {
        if (SmartUI.MsgYesNo("처방내역을 초기화하시겠습니까?") is System.Windows.MessageBoxResult.No) return;

        foreach (var item in ConsultationOrderItems.Reverse())
        {
            ConsultationOrderItems.Remove(item);
        }
    }

    private void OnConsultationOrderItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (sender is not ObservableCollection<ConsultationOrder> consultationOrders) return;

        foreach (var item in consultationOrders)
        {
            item.ViewIndex = consultationOrders.IndexOf(item);
        }

        if (e.Action == NotifyCollectionChangedAction.Remove && e.NewItems != null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is ConsultationOrder cItem)
                {
                    deletedItems.Add(cItem);
                }
            }
        }
    }
}
