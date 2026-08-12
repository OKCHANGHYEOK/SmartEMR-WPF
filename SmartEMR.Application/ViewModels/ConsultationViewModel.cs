using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ConsultationViewModel : BaseViewModel<Consultation>
{
    public override void Initialize()
    {
    }

    protected override Consultation GetModel(Consultation item)
    {
        if (item.CST_Idx.GetValueOrDefault(0) == 0)
        {
            SmartMVVM.ModelProperty.SetDefaultConsultationData(item);
        }

        return item;
    }
}
