using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRSummaryBoardViewModel : BaseViewModel<ReceptionBoard>
{
    [ObservableProperty]
    private List<ReceptionBoard> boards = new();

    private readonly IReceptionService _receptionService;

    public SmartEMRSummaryBoardViewModel(IReceptionService receptionService)
    {
        _receptionService = receptionService;
    }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await FetchDataAsync();
    }

    protected override ReceptionBoard GetModel(ReceptionBoard item)
    {
        return item;
    }

    public override async Task<bool> FetchDataAsync()
    {
        var getItem = new ReceptionBoard
        {
            RCB_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd"),

            SortField = "RCB_Date",
            SortDir = "desc"
        };

        var ret = await _receptionService.GetReeptionBoards(getItem);
        if (ret.Items is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification(ret.Message ?? "", NotificationType.Error);
            return false;
        }

        Boards = [.. ret.Items];

        return true;
    }
}
