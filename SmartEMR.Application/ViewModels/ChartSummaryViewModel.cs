using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ChartSummaryViewModel : BaseViewModel<Chart>
{

    private List<Chart>? _charts = null;

    public IReadOnlyList<Chart>? CHTItems => _charts?.AsReadOnly<Chart>() ?? null;

    public override void Initialize()
    {
    }

    protected override Chart GetModel(Chart item)
    {
        item.CHT_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
        item.PageSize = 10;

        return item;
    }

    protected override async Task OnLoadDataAsync()
    {
        // 더미 데이터
        _charts = new List<Chart>()
        {
            new Chart { CHT_CHTType = "CHT", vCHT_CHTType = "접수", PAT_Name = "송하영", vPAT_Info = "여/29세", CHT_CHTTime = "09:57"},
            new Chart { CHT_CHTType = "CHT", vCHT_CHTType = "접수", PAT_Name = "전승우", vPAT_Info = "남/35세", CHT_CHTTime = "10:26"},
            new Chart { CHT_CHTType = "CHT", vCHT_CHTType = "접수", PAT_Name = "강주희", vPAT_Info = "여/21세", CHT_CHTTime = "10:34"},
            new Chart { CHT_CHTType = "RES", vCHT_CHTType = "예약", PAT_Name = "권수아", vPAT_Info = "여/28세", CHT_CHTTime = "11:00"},
            new Chart { CHT_CHTType = "RES", vCHT_CHTType = "예약", PAT_Name = "김희수", vPAT_Info = "남/24세", CHT_CHTTime = "16:00" },
        };

        //var retCHT = await SmartMVVM.DataStore.GetItems<Chart>(eAPI.Chart_GetChart, this.Model);

        //if (retCHT != null) 
        //{ 
        //    _charts = retCHT.ToList();
        //}
    }
}
