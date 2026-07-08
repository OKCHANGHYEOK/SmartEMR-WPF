using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public class ReceptionViewModel : BaseViewModel<Reception>
{
    public ReceptionViewModel() { }
    public ReceptionViewModel(Reception item) : base(item) { }

    public Patient PATItem { get; set; } = new();
    public Insurance IRCItem { get; set; } = new();

    public List<MemberUser> arrMUR_DOC { get; set; } = default!;
    public List<MemberUser> arrMUR_STF { get; set; } = default!;

    public List<CommonCode> arrRCP_Status { get; set; } = default!;
    public List<CommonCode> arrRCP_Subject { get; set; } = default!;
    public List<CommonCode> arrRCP_VisitType { get; set; } = default!;
    public List<CommonCode> arrRCP_Route { get; set; } = default!;
    public List<CommonCode> arrRCP_InsuranceType { get; set; } = default!;

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        if (Model.RCP_Idx.GetValueOrDefault(0) > 0)
        {
            var retPAT = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = Model.PAT_Idx });
            if (retPAT == null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("삭제됐거나 존재하지 않는 환자입니다.", NotificationType.Error);
                return;
            }

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, new Reception { RCP_Idx = Model.RCP_Idx });
            if (retRCP == null || !SmartMVVM.DataStore.retIsSuccess)
            {
                SmartUI.SetNofification("삭제됐거나 존재하지 않는 접수입니다.", NotificationType.Error);
                return;
            }

            var IRCItem = SmartMVVM.ModelProperty.GetInsuranceDataFromRCP(retRCP);

            SmartMVVM.ModelProperty.SetPatientData(PATItem, retPAT);
            SmartMVVM.ModelProperty.SetReceptionData(Model, retRCP);
            SmartMVVM.ModelProperty.SetInsuranceData(this.IRCItem, IRCItem);
        }
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }

}
