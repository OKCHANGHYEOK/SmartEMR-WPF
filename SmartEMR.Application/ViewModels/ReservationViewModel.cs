using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public class ReservationViewModel : BaseViewModel<Reservation>
{
    public override void Initialize()
    {
    }

    protected override Reservation GetModel(Reservation item)
    {
        return item;
    }
}
