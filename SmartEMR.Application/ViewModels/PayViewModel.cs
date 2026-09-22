using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class PayViewModel : BaseViewModel<Pay>
{
    public string NowYYYYMMDD { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

    protected IPayService _payService;

    private bool _isinitliazed = false;

    [ObservableProperty]
    private List<Pay> pays = default!;

    public PayViewModel(IPayService payService)
    {
        _payService = payService;
    }

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        await FetchDataAsync();
     }

    protected override Pay GetModel(Pay item)
    {
        item.CST_Status = "";
        item.PAY_Status = "RDY";
        item.PAY_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");

        _isinitliazed = true;

        return item;
    }

    public override async Task<bool> FetchDataAsync()
    {
        if (!_isinitliazed) return false;

        var result = await _payService.GetPays(Model);

        if (result.Items is null || !result.IsSuccess)
        {
            SmartUI.SetNotification(result.Message ?? "", NotificationType.Error);
            return false;
        }

        Pays = [.. result.Items];

        return true;
    }

    public void SetToday()
    {
        Model.PAY_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd");
    }

    [RelayCommand]
    public async Task Search()
    {
        await FetchDataAsync();
    }

    [RelayCommand]
    public async Task Reset()
    {
        Model.PAY_Status = "RDY";
        Model.Keyword = "";

        await FetchDataAsync();
    }
}
