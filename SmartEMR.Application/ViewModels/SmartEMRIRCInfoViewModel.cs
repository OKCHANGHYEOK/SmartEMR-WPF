using SmartEMR.Application.Core;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRIRCInfoViewModel : InsuranceInfoViewModel
{
    public void ClearData()
    {
        SmartMVVM.ModelProperty.ClearIRCData(Model);
    }
}
