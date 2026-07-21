using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class InsuranceInfoViewModel : InsuranceViewModel
{
    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await SmartUI.SendMessage("SetIRCItem", Model, viewType: TargetViewType.PageView);
    }

    protected override Insurance GetModel(Insurance item)
    {
        if (item.IRC_Idx.GetValueOrDefault(0) == 0)
        {
            item.IRC_CoName = "삼성화재";
            item.IRC_EffectiveYYMMDD = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            item.IRC_ExpiredYYMMDDD = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
        }

        return item;
    }

    public void SetData(Insurance item)
    {
        SmartMVVM.ModelProperty.SetInsuranceData(Model, item);
    }

    public void ClearData(bool isClearIRCType)
    {
        SmartMVVM.ModelProperty.ClearIRCData(Model, isClearIRCType);
    }
}
