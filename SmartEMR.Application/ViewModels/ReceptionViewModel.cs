using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ReceptionViewModel : BaseViewModel<Reception>
{
    public override void Initialize()
    {
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }
}
