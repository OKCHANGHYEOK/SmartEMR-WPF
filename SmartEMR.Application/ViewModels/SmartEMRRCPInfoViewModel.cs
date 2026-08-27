using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
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
    public async Task RequestSetRCP(SaveMode operation)
    {
        await SmartUI.SendMessage("SetReception", operation, viewType:TargetViewType.PageView);
    }

    public async Task SetReceptionData(Reception? item = null)
    {
        if (item is null)
        {
            var getRCP = new Reception
            {
                PAT_Idx = PATItem.PAT_Idx,
                RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, getRCP);
            if (retRCP is null) return;

            item = retRCP;
        }

        if (item is null) return;

        item.RCP_VisitType = await SmartMVVM.Common.GetVisitType(item.PAT_Idx.GetValueOrDefault(0));

        SmartMVVM.ModelProperty.SetReceptionData(Model, item);
    }

    public async Task SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(PATItem, item);

        Model.PAT_Idx = item.PAT_Idx;
    }
}
