using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public class ReceptionViewModel : BaseViewModel<Reception>
{
    public IQueryable<Reception>? arrRCP { get; set; }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        arrRCP = await SmartMVVM.DataStore.GetItems<Reception>(eAPI.Reception_GetReception, new Reception { RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd") });
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }
}
