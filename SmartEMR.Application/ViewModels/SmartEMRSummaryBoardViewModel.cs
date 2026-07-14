using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class SmartEMRSummaryBoardViewModel : BaseViewModel<SmartEMRSummaryItem>
{
    public List<SmartEMRSummaryItem> arrSUM { get; set; } = new();

    public override void Initialize()
    {
    }

    protected override SmartEMRSummaryItem GetModel(SmartEMRSummaryItem item)
    {
        return item;
    }
}
