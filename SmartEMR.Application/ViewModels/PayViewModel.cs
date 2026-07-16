using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class PayViewModel : BaseViewModel<Pay>
{
    public List<CommonCode>? arrPAY_Status = new();

    public string NowYYYYMMDD { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

    public override void Initialize()
    {
    }

    public override async Task InitializeAsync()
    {
        arrPAY_Status = SmartMVVM.Common.GetCommonCode("PAY", "Status")?.ToList();
    }

    protected override Pay GetModel(Pay item)
    {
        item.PAY_Status = "RDY";
        return item;
    }
}
