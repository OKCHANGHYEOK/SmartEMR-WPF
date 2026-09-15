using System.Windows;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRIRCInfoViewModel : InsuranceInfoViewModel
{
    public async Task<bool> ExistsCST()
    {
        if (Reception is null || Reception.RCP_Idx.GetValueOrDefault(0) == 0) return false;

        var retCST = await SmartMVVM.DataStore.GetItem<Consultation>(eAPI.Consultation_GetConsultation, new Consultation { RCP_Idx = Reception.RCP_Idx });
        if (retCST is not null)
        {
            return true;
        }

        return false;
    }

    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearIRCData(Model);
    }


    [RelayCommand]
    public async Task SetRecentInsurance()
    {
        var ret = await SmartMVVM.DataStore.GetItem<Insurance>(eAPI.Insurance_GetRecentInsurance, new Insurance { PAT_Idx = Model.PAT_Idx });
        if (ret is null)
        {
            SmartUI.SetNofification("최근보험 정보가 존재하지 않습니다.", NotificationType.Warning);
            return;
        }

        SmartMVVM.ModelProperty.SetInsuranceData(Model, ret, isCopy:true);

        SmartUI.SetNofification("최근보험이 적용되었습니다.", NotificationType.Success);
    }


    [RelayCommand]
    public void ResetIRC()
    {
        if (SmartUI.MsgYesNo("보험정보를 초기화하시겠습니까?") is MessageBoxResult.No) return;

        ClearData();
    }
}
