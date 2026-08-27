using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.ViewModels;

public partial class ConsultationViewModel : BaseViewModel<Consultation>
{
    [ObservableProperty]
    private List<Consultation>? consultations;

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await UpdateConsultationsByRCP();
    }

    protected override Consultation GetModel(Consultation item)
    {
        if (item.CST_Idx.GetValueOrDefault(0) == 0)
        {
            SmartMVVM.ModelProperty.SetDefaultConsultationData(item);
        }

        return item;
    }

    public void SetSelectedCST(Consultation item)
    {
        SmartMVVM.ModelProperty.SetConsultationData(Model, item);
    }

    [RelayCommand]
    public async Task UpdateConsultationsByRCP()
    {
        var item = new Consultation
        {
            PAT_Idx = Model.PAT_Idx,
            MUR_Idx_DOC = Model.MUR_Idx_DOC,

            CST_InsuranceType = Model.CST_InsuranceType,
            CST_Status = Model.CST_Status,
            CST_PayStatus = Model.CST_PayStatus,
            CST_Subject = Model.CST_Subject,
            CST_YYMMDD = Model.CST_YYMMDD,

            Keyword = Model.Keyword,
            SortField = Model.SortField,
            SortDir = Model.SortDir,
            PageSize = Model.PageSize,
            PageIndex = Model.PageIndex
        };

        var ret = await SmartMVVM.DataStore.GetItems<Consultation>(eAPI.Consultation_GetConsultationByRCP, item);
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNofification("진료현황을 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        DisplayDataMappers.ConsultationDisplayDataMapper.Map(ret);

        Consultations = ret.ToList();
    }

    [RelayCommand]
    public async Task SetConsultation()
    {
        if (Model.CST_Idx.GetValueOrDefault(0) == 0)
        {
            SmartUI.SetNofification("선택된 진료가 없습니다.", NotificationType.Warning);
            return;
        }


    }

    [RelayCommand]
    public void ClearData(bool isClearFilter = false)
    {
        SmartMVVM.ModelProperty.ClearCSTData(Model, isClearFilter);
    }

    [RelayCommand]
    public void ClearDataByCSTInfo()
    {
        if (SmartUI.MsgYesNo("입력된 데이터가 초기화됩니다. 초기화하시겠습니까?") is MessageBoxResult.No) return;

        ClearData();
    }
}
