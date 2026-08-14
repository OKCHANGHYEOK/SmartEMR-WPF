using System.Windows;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRIRCInfoViewModel : InsuranceInfoViewModel
{
    [RelayCommand]
    public async Task SetRecentinsurance()
    {
        var ret = await SmartMVVM.DataStore.GetItem<Insurance>(eAPI.Insurance_GetRecentInsurance, new Insurance { PAT_Idx = Model.PAT_Idx });
        if (ret is null)
        {
            SmartUI.SetNofification("최근보험 정보가 존재하지 않습니다.", NotificationType.Warning);
            return;
        }

        SmartMVVM.ModelProperty.SetInsuranceData(Model, ret);

        SmartUI.SetNofification("최근보험이 적용되었습니다.", NotificationType.Success);
    }

    [RelayCommand]
    public new void ClearData(bool isClickedClearButton = false)
    {
        if (isClickedClearButton && SmartUI.MsgYesNo("보험정보를 초기화하시겠습니까?") is MessageBoxResult.No) return;

        SmartMVVM.ModelProperty.ClearIRCData(Model);
    }
}
