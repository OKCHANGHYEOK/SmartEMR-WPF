using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class InsuranceViewModel : BaseViewModel<Insurance>
{
    public InsuranceViewModel() { }
    public InsuranceViewModel(Insurance item) : base(item) { }

    public override void Initialize()
    {
    }

    protected override Insurance GetModel(Insurance item)
    {
        return item;
    }
}
