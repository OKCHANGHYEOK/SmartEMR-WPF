using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class InsuranceInfoViewModel : InsuranceViewModel
{
    public Reception? Reception = null;
    private Insurance? receptionInsurance = null;

    public InsuranceInfoViewModel() { }

    public InsuranceInfoViewModel(Insurance item) : base(item) { }

    public override void Initialize()
    {
        SetData(Model);
    }

    public override async Task InitializeAsync()
    {
        await SmartUI.SendMessage("SetIRCItem", Model, viewType: TargetViewType.PageView);

        if (Model.RCP_Idx > 0)
        {
            var retIRC = await SmartMVVM.DataStore.GetItem<Insurance>(eAPI.Insurance_GetInsurance, new Insurance { RCP_Idx = Model.RCP_Idx });
            if (retIRC is not null)
            {
                receptionInsurance = retIRC.Clone();
            }
        }
    }

    protected override Insurance GetModel(Insurance item)
    {
        if (item.RCP_Idx.GetValueOrDefault(0) == 0 || item.CST_Idx.GetValueOrDefault(0) == 0)
        {
            item.IRC_CoName = "삼성화재";
            item.IRC_EffectiveYYMMDD = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
            item.IRC_ExpiredYYMMDD = DateTime.Now.AddYears(1).ToString("yyyy-MM-dd");
        }

        return item;
    }

    public void SetData(Insurance item, bool isCopy = false)
    {
        SmartMVVM.ModelProperty.SetInsuranceData(Model, item, isCopy);
    }

    public void SetDataByRCP()
    {
        if (receptionInsurance is null) return;

        SetData(receptionInsurance, isCopy:true);
    }

    public void SetReception(Reception item)
    {
        Reception = item;
    }

    public bool IsIRCFromRCP()
    {
        if (receptionInsurance != null && receptionInsurance.IRC_Idx.GetValueOrDefault(0) == Model.IRC_Idx_From)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [RelayCommand]
    public async Task ApplyInsurance()
    {
        SmartUI.CloseView();

        await SmartUI.SendMessage("UpdateCSTByIRC", Model.Clone(), viewType:TargetViewType.PageView);
    }

    [RelayCommand]
    public void ClearData(bool isClearIRCType)
    {
        SmartMVVM.ModelProperty.ClearIRCData(Model, isClearIRCType);
    }
}
