using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class DeskViewModel : ReceptionViewModel
{
    public override void Initialize()
    {
    }

    protected override Reception GetModel(Reception item)
    {
        return item;
    }

    public void SetPatientData(Patient item)
    {
        SmartMVVM.ModelProperty.SetPatientData(PATItem, item);
    }
}
