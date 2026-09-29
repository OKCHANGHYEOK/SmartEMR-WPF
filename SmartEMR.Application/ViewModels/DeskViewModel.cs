using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public class DeskViewModel : ReceptionViewModel
{
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
        var ret = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, new Reception { RCP_Idx = RCP_Idx });
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNotification("접수 데이터를 불러오는데 실패했습니다.", NotificationType.Error);
            return null;
        }

        return ret;
    }
}
