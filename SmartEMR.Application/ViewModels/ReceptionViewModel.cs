using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public class ReceptionViewModel : BaseViewModel<Reception>
{
    private List<Reception> _arrRCP = default!;

    public List<Reception> arrRCP
    {
        get => _arrRCP;
        set
        {
            OnPropertyChanged(nameof(arrRCP));
        }
    }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await FetchDataAsync();
    }

    protected override Reception GetModel(Reception item)
    {
        item.RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
        return item;
    }


    public override async Task FetchDataAsync()
    {
        var getRCP = new Reception
        {
            MUR_Idx_DOC = Model.MUR_Idx_DOC,

            RCP_Status = Model.RCP_Status,
            RCP_Route = Model.RCP_Route,
            RCP_VisitType = Model.RCP_VisitType,
            RCP_YYMMDD = Model.RCP_YYMMDD,

            IRC_Type = Model.IRC_Type,

            Keyword = Model.Keyword,
            PageSize = Model.PageSize,
            PageIndex = Model.PageSize,
            SortField = Model.SortField,
            SortDir = Model.SortDir
        };

        var retRCP = await SmartMVVM.DataStore.GetItems<Reception>(eAPI.Reception_GetReception, getRCP);

        if (retRCP != null && retRCP.Any())
        {
            arrRCP = retRCP.ToList();
        }
        else
        {
            arrRCP = new List<Reception>();
        }
    }
}
