using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRRCPInfoViewModel : ReceptionViewModel
{
    public SmartEMRRCPInfoViewModel() { }
    public SmartEMRRCPInfoViewModel(Reception item) : base(item) { }

    public override void Initialize()
    {
        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "의사선택");
        arrMUR_STF = SmartMVVM.Master.GetMemberUsers("STF", true, "직원선택");
        arrRCP_Subject = SmartMVVM.Common.GetCommonCode("RCP","Subject");
        arrRCP_VisitType = SmartMVVM.Common.GetCommonCode("RCP", "VisitType");
        arrRCP_Route = SmartMVVM.Common.GetCommonCode("RCP", "Route");
        arrRCP_InsuranceType = SmartMVVM.Common.GetCommonCode("RCP", "InsuranceType");
    }

    public override async Task InitializeAsync()
    {
        await SmartUI.SendMessage("SetRCPItem", Model, viewType: TargetViewType.PageView);
    }

    protected override Reception GetModel(Reception item)
    {
        if (item.RCP_Idx.GetValueOrDefault(0) == 0)
        {
            item.MUR_Idx_DOC = 0;
            item.MUR_Idx_STF = 0;
            item.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
            item.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
        }

        return item;
    }

    [RelayCommand]
    public async Task RequestSetRCP(string operation)
    {
        await SmartUI.SendMessage("SetReception", operation, viewType:TargetViewType.PageView);
    }

    public async void SetReceptionData(Reception? item = null)
    {
        if (item == null)
        {
            var getRCP = new Reception
            {
                PAT_Idx = PATItem.PAT_Idx,
                RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, getRCP);
            if (retRCP != null)
            {
                item = retRCP;
            }
        }

        if (item == null) return;

        SmartMVVM.ModelProperty.SetReceptionData(Model, item);
    }

    public void SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(PATItem, item);

        Model.PAT_Idx = item.PAT_Idx;
    }

    public void ClearData()
    {
        Model.RCP_Idx = 0;
        Model.MUR_Idx_DOC = 0;
        Model.MUR_Idx_STF = 0;
        Model.RES_Idx = 0;
        Model.RCP_VisitType = "FIR";
        Model.RCP_Status = "";
        Model.RCP_Route = "DSK";
        Model.RCP_Subject = "GNR";
        Model.RCP_SubjectName = "";
        Model.RCP_InsuranceType = "NON";
        Model.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
        Model.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
        Model.RCP_StartTreatTime = "";
        Model.RCP_EndTreatTime = "";
        Model.RCP_Memo = "";

        PATItem = new();
    }
}
