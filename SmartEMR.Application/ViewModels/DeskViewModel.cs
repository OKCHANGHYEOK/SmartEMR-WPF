using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class DeskViewModel : ReceptionViewModel
{
    public DeskViewModel(IPatientService patientService, IReceptionService receptionService) : base(patientService, receptionService)
    {
    }

    public override void Initialize()
    {
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }

    public void SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(PATItem, item);
    }

    public async Task<Reception?> GetReception(int RCP_Idx)
    {
        var ret = await _receptionService.GetReception(new Reception { RCP_Idx = RCP_Idx });
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification("접수 데이터를 불러오는데 실패했습니다.", NotificationType.Error);
            return null;
        }

        return ret.Item;
    }
}
