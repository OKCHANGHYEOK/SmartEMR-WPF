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
        item.PAT_Sex = "N";
        item.PAT_SourceType = "WRK";
        item.PAT_IsSolar = "y";
        item.PAT_IsForegin = "n";
        item.PAT_IsAgreePersonalInfo = "n";
        item.PAT_IsSMS = "n";

        return item;
    }

    [RelayCommand]
    public async Task SetPatient(string opreation)
    {
        if (!Enum.TryParse<OperationType>(opreation, out var operationType)) return;

        try
        {
            if (operationType == OperationType.DELETE)
            {
                if (SmartUI.MsgYesNo("삭제하시면 복구가 불가능합니다." + "\n" + "삭제하시겠습니까?") != System.Windows.MessageBoxResult.Yes) return;

                await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_SetPatient, new Patient { PAT_Idx = Model.PAT_Idx, PAT_IsValid = false });

                if (SmartMVVM.DataStore.retIsSuccess == false)
                {
                    SmartUI.SetNofification("환자정보 삭제에 실패했습니다.", NotificationType.Error);
                    return;
                }

                SmartUI.SetNofification($"삭제되었습니다.", NotificationType.Success);
                await SmartUI.SendMessage("ClearPAT", viewType: TargetViewType.PageView);

                return;
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

            var msg = "환자" + (Model.PAT_Idx.GetValueOrDefault(0) == 0 ? "등록" : "수정");

            SmartUI.SetNofification($"{msg} 되었습니다.", NotificationType.Success);

            await SmartUI.SendMessageToSearchView("SetSelectedPatient", retPAT);
            await SmartUI.SendMessage("SetSelectedPatient", retPAT, TargetViewType.PageView);
        }
        finally
        {
            if (SmartMVVM.DataStore.retIsSuccess)
            {
                await SmartUI.SendMessage("CloseView");
            }
        }
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
}
