using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ConsultationViewModel : BaseViewModel<Consultation>
{
    public override void Initialize()
    {
    }

    protected override Consultation GetModel(Consultation item)
    {
        return item;
    }
}
