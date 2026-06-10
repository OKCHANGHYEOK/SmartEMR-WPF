using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class PatientInfoViewModel : PatientViewModel
{
    public PatientInfoViewModel() {}
    public PatientInfoViewModel(Patient item) : base(item) { }

    public override void Initialize()
    {
        if (Model.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("존재하지 않거나 삭제된 회원입니다.", NotificationType.Error);
                SmartUI.CloseView(TargetViewType.CurrentView);                
                return;
            }
        }
    }

    protected override Patient GetModel(Patient item)
    {
        item.PAT_Sex = "N";
        item.PAT_SourceType = "WRK";
        item.PAT_IsSolar = "y";
        item.PAT_IsForegin = "n";
        item.PAT_IsAgreePersonalInfo = "n";
        item.PAT_IsSMS = "n";

        return item;
    }

    [RelayCommand]
    public async Task SavePatient(string opreation)
    {
        if (!Enum.TryParse<OperationType>(opreation, out var operationType)) return;

        if (operationType == OperationType.DELETE)
        {
            await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_SetPatient, new Patient { PAT_Idx = Model.PAT_Idx, PAT_IsValid = false });

            if (SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("환자정보 삭제에 실패했습니다.", NotificationType.Error);
                return;
            }

            SmartUI.SetNofification($"삭제되었습니다.", NotificationType.Success);
        }

        if (!ValidateInputData()) return;

        var item = SmartMVVM.ModelProperty.GetPatientDataForSave(Model);
        var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_SetPatient, item);

        if (retPAT == null || SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("환자정보 저장에 실패했습니다.", NotificationType.Error);
            return;
        }

        Model.PAT_ChartNo = retPAT.PAT_ChartNo;

        var msg = "환자" + ((operationType == OperationType.CREATE) ? "등록" : "수정");

        SmartUI.SetNofification($"{msg} 되었습니다.", NotificationType.Success);
        
        await SmartUI.SendMessage("CloseView");
    }

    private bool ValidateInputData()
    {
        var missingField = "";
        var missingFieldName = "";

        if (string.IsNullOrWhiteSpace(Model.PAT_Name))
        {
            missingField = "PAT_Name";
            missingFieldName = "성명";
        }
        else if (string.IsNullOrWhiteSpace(Model.PAT_RegisterNum1))
        {
            missingField = "PAT_RegisterNum1";
            missingFieldName = "주민번호앞자리";
        }
        else if (string.IsNullOrWhiteSpace(Model.PAT_RegisterNum2))
        {
            missingField = "PAT_RegisterNum2";
            missingFieldName = "주민번호뒷자리";
        }

        if (!string.IsNullOrWhiteSpace(missingField))
        {
            SmartUI.SetNofification($"{missingFieldName}을/를 입력해주세요.", NotificationType.Warning);
            
            TextFocusBehavior.SetFocusByName(missingField);

            return false;
        }

        return true;
    }
}
