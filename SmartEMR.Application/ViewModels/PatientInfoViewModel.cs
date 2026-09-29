using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class PatientInfoViewModel : PatientViewModel
{
    [ObservableProperty]
    public FromViewType fromViewType = FromViewType.VIEW;

    public PatientInfoViewModel(IPatientService patientService) : base(patientService) { }
    public PatientInfoViewModel(IPatientService patientService, Patient item) : base(patientService, item) { }

    public override void Initialize() { }

    public override async Task InitializeAsync() 
    {
        await base.InitializeAsync();
        
        if (Model.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = await _patientService.GetPatient(new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT.Item is null || !retPAT.IsSuccess)
            {
                SmartUI.SetNotification(retPAT.Message ?? "", NotificationType.Error);
                SmartUI.CloseView(TargetViewType.CurrentView);                
                return;
            }

            SmartMVVM.ModelProperty.SetPatientData(Model, retPAT.Item);
        }
    }

    protected override Patient GetModel(Patient item)
    {
        SmartMVVM.ModelProperty.SetDefaultPatientData(item);
        return item;
    }

    public void SetFromViewType(FromViewType fromViewType)
    {
        FromViewType = fromViewType;
    }

    [RelayCommand]
    public async Task SetPatient(SaveMode saveMode)
    {
        bool isNew = Model.PAT_Idx.GetValueOrDefault(0) == 0;

        string actionName = saveMode switch
        {
            SaveMode.SAVE => isNew ? "등록" : "수정",
            SaveMode.DELETE => "삭제",
            _ => ""
        };

        if (saveMode == SaveMode.DELETE)
        {
            if (!await DeletePatientAsync()) return;
        }
        else
        {
            if (!ValidateInputData()) return;

            var item = SmartMVVM.ModelProperty.GetPatientDataForSave(Model);
            var retPAT = await _patientService.SetPatient(item);

            if (retPAT.Item is null || !retPAT.IsSuccess)
            {
                SmartUI.SetNotification("환자정보 저장에 실패했습니다.", NotificationType.Error);
                return;
            }

            SmartMVVM.ModelProperty.SetPatientData(Model, retPAT.Item);
        }

        await NotifyCompletedTaskAsync(saveMode);

        SmartUI.SetNotification($"{actionName} 되었습니다.", NotificationType.Success);
    }

    private async Task<bool> DeletePatientAsync()
    {
        if (SmartUI.MsgYesNo("삭제하시면 복구가 불가능합니다." + "\n" + "삭제하시겠습니까?") != System.Windows.MessageBoxResult.Yes) return false;

        var ret = await _patientService.SetPatient(new Patient { PAT_Idx = Model.PAT_Idx, PAT_IsValid = false });
        if (!ret.IsSuccess || !string.IsNullOrWhiteSpace(ret.Message))
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return false;
        }

        return true;
    }

    private bool ValidateInputData()
    {
        List<string[]> missingFields = new List<string[]>();
        List<string[]> uncorrectFields = new List<string[]>();

        if (string.IsNullOrWhiteSpace(Model.PAT_Name))
        {
            missingFields.Add(["PAT_Name", "성명"]);
        }

        if (string.IsNullOrWhiteSpace(Model.PAT_RegisterNum1))
        {
            missingFields.Add(["PAT_RegisterNum1", "주민번호앞자리"]);
        }
        
        if (string.IsNullOrWhiteSpace(Model.PAT_RegisterNum2))
        {
            missingFields.Add(["PAT_RegisterNum2", "주민번호뒷자리"]);
        }

        if (missingFields.Any())
        {
            var message = "아래 항목들을 입력해주세요.\n- ";
            message += string.Join(", ", missingFields.Select(field => field[1]));

            SmartUI.SetNotification(message, NotificationType.Warning);

            TextFocusBehavior.SetFocusByName(missingFields[0][0]);

            return false;
        }

        return true;
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode saveMode)
    {
        await SmartUI.SendMessage("CloseView");

        if (saveMode == SaveMode.DELETE)
        {
            await SmartUI.SendMessage("ClearPAT", viewType: TargetViewType.PageView);
            return;
        }

        if (FromViewType == FromViewType.VIEW)
        {
            var response = await SmartUI.SendMessage<Patient>("GetPATItem", viewType: TargetViewType.PageView);

            // 현재 보고 있는 환자가 없거나 보고 있는 환자 == 업데이트된 환자인 경우에만 메시지 전송
            if (response is not null && response.Item is Patient PATItem)
            {
                if (PATItem.PAT_Idx.GetValueOrDefault(0) == 0 || PATItem.PAT_Idx == Model.PAT_Idx)
                {
                    await SmartUI.SendMessageToSearchView("SetSelectedPatient", Model);
                    await SmartUI.SendMessage("SetSelectedPatient", Model, viewType:TargetViewType.PageView);
                }
            }
        }
        else
        {
            await SmartUI.SendMessage("UpdatePatientData", Model);
        }
    }
}
