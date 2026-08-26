using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ConsultationOrderViewModel : BaseViewModel<ConsultationOrder>
{
    public override void Initialize()
    {
    }

    protected override ConsultationOrder GetModel(ConsultationOrder item)
    {
        return item;
    }
}
