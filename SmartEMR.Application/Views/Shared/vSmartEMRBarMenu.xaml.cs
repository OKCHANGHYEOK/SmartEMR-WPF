using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views;

public partial class vSmartEMRBarMenu : ModelViewLayout<BarMenuViewModel> 
{
    public vSmartEMRBarMenu() : base()
    {
    }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
    }
}
