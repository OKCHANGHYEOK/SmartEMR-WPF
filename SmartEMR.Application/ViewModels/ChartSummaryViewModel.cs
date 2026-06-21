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
}
