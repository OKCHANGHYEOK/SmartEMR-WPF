using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class PayViewModel : BaseViewModel<Pay>
{
    public string NowYYYYMMDD { get; set; } = DateTime.Now.ToString("yyyy.MM.dd");

    public override void Initialize()
    {
    }

    protected override Pay GetModel(Pay item)
    {
        return item;
    }
}
