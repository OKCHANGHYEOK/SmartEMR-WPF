using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class PayInfoViewModel : PayViewModel
{
    public PayInfoViewModel() { }

    protected override Pay GetModel(Pay item)
    {
        item.PAY_InsuredPrice = 0;
        item.PAY_NonInsuredPrice = 0;
        item.PAY_OwnPatientPrice = 0;

        return item;
    }

    public void UpdatePriceData(Pay item)
    {
        SmartMVVM.ModelProperty.SetPayData(Model, item);
    }
}
