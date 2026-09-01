using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class PatientInfoViewModel : PatientViewModel
{
    [ObservableProperty]
    public FromViewType fromViewType =FromViewType.VIEW;

    public PatientInfoViewModel() {}
    public PatientInfoViewModel(Patient item) : base(item) { }

    public override void Initialize() { }

    public override async Task InitializeAsync() 
    {
        await base.InitializeAsync();
        
        if (Model.PAT_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("존재하지 않거나 삭제된 회원입니다.", NotificationType.Error);
                SmartUI.CloseView(TargetViewType.CurrentView);                
                return;
            }

            SmartMVVM.ModelProperty.SetPatientData(Model, retPAT);
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
    public async Task SetPatient(SaveMode operation)
    {
        bool isNew = Model.PAT_Idx.GetValueOrDefault(0) == 0;

        string actionName = operation switch
        {
            SaveMode.SAVE => isNew ? "등록" : "수정",
            SaveMode.DELETE => "삭제",
            _ => ""
        };

        if (operation == SaveMode.DELETE)
        {
            if (!await DeletePatientAsync()) return;
        }
        else
        {
            if (!ValidateInputData()) return;

            var item = SmartMVVM.ModelProperty.GetPatientDataForSave(Model);
            var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_SetPatient, item);

            if (retPAT == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("환자정보 저장에 실패했습니다.", NotificationType.Error);
                return;
            }

            SmartMVVM.ModelProperty.SetPatientData(Model, retPAT);
        }

        await NotifyCompletedTaskAsync(operation);

        SmartUI.SetNofification($"{actionName} 되었습니다.", NotificationType.Success);
    }

    private async Task<bool> DeletePatientAsync()
    {
        if (SmartUI.MsgYesNo("삭제하시면 복구가 불가능합니다." + "\n" + "삭제하시겠습니까?") != System.Windows.MessageBoxResult.Yes) return false;

        await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_SetPatient, new Patient { PAT_Idx = Model.PAT_Idx, PAT_IsValid = false });

        if (SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("환자정보 삭제에 실패했습니다.", NotificationType.Error);
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

            SmartUI.SetNofification(message, NotificationType.Warning);

            TextFocusBehavior.SetFocusByName(missingFields[0][0]);

            return false;
        }

        return true;
    }

    protected override async Task NotifyCompletedTaskAsync(SaveMode operation)
    {
        await SmartUI.SendMessage("CloseView");

        if (operation == SaveMode.DELETE)
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
