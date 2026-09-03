using CommunityToolkit.Mvvm.Input;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class PayViewModel : BaseViewModel<Pay>
{
    public string NowYYYYMMDD { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

    public override void Initialize()
    {
    }

    protected override Pay GetModel(Pay item)
    {
        item.PAY_Status = "RDY";
        return item;
    }

    public override async Task<bool> FetchDataAsync()
    {
        return true;
    }

    [RelayCommand]
    public async Task Search()
    {
        // 추후 수납 검색 로직 구현
    }

    [RelayCommand]
    public async Task Reset()
    {
        Model.PAY_Status = "RDY";
        Model.Keyword = "";

        await FetchDataAsync();
    }
}
