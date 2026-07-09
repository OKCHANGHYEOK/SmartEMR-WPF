using SmartEMR.Application.Core;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRIRCInfoViewModel : InsuranceInfoViewModel
{
    public override async Task InitializeAsync()
    {
        await SmartUI.SendMessage("SetIRCItem", Model, viewType:TargetViewType.PageView);
    }
}
