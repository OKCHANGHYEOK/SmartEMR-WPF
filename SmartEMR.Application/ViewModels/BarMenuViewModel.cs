using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class BarMenuViewModel : BaseViewModel<Patient>
{
    public override void Initialize()
    {
    }

    protected override Patient GetModel(Patient item)
    {
        return item;
    }
}
