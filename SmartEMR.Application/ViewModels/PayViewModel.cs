using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class PayViewModel : BaseViewModel<Pay>
{
    [ObservableProperty]
    private List<Pay> pays = default!;

    public string NowYYYYMMDD { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

    public PayViewModel() { }

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

        return item;
    }

    public override async Task<bool> FetchDataAsync()
    {
        var getPAY = new Pay
        {
            PAY_Idx = Model.PAY_Idx,

            CST_Status = Model.CST_Status,
            PAY_Status = Model.PAY_Status,
            PAY_YYMMDD = Model.PAY_YYMMDD,

            sDay = Model.sDay,
            eDay = Model.eDay,

            Keyword = Model.Keyword,
            SortField = Model.SortField,
            SortDir = Model.SortDir ?? "desc",
            PageSize = Model.PageSize.GetValueOrDefault(0) == 0 ? 20 : Model.PageSize,
            PageIndex = Model.PageIndex,
        };

        var ret = await SmartMVVM.DataStore.GetItems<Pay>(eAPI.Pay_GetPay, getPAY);
        if (ret is null || !SmartMVVM.DataStore.retIsSuccess)
        {
            SmartUI.SetNotification("수납내역 조회에 실패했습니다.", NotificationType.Error);
            return false;
        }

        DisplayDataMappers.PayDisplayDataMapper.Map(ret);

        Pays = [.. ret];

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
