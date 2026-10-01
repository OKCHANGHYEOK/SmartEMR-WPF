using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class DeskViewModel : ReceptionViewModel
{
    public DeskViewModel(IPatientService patientService, 
                         IReceptionService receptionService) : base(patientService, receptionService)
    {
    }

    public DeskViewModel(IPatientService patientService, 
                         IReceptionService receptionService,
                         Reception item) : base(patientService, receptionService, item)
    {
    }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        if (Model.RCP_Idx > 0)
        {
            var ret = await GetReception(Model.RCP_Idx.GetValueOrDefault(0));
            if (ret is not null)
            {
                SmartUI.BeginInvoke(async () =>
                {
                    await SmartUI.SendMessage("SetSelectedRCP", ret, viewType: TargetViewType.PageView);
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }

    public void SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(PATItem, item);
    }

    public async Task<Patient?> GetPatient(int PAT_Idx)
    {
        var ret = await _patientService.GetPatient(new Patient { PAT_Idx = PAT_Idx });
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return null;
        }

        return ret.Item;
    }

    public async Task<Reception?> GetReception(int RCP_Idx)
    {
        var ret = await _receptionService.GetReception(new Reception { RCP_Idx = RCP_Idx });
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return null;
        }

        return ret.Item;
    }
}
