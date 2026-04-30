using DevExpress.Mvvm;
using DevExpress.Xpf.Core;

namespace SmartEMR.Application.Views;

/// <summary>
/// Interaction logic for vSmartEMRWaitingView.xaml
/// </summary>
public partial class vSmartEMRWaitingView : SplashScreenWindow, ISplashScreen
{
    private DXSplashScreenViewModel? viewModel = null;

    public vSmartEMRWaitingView()
    {
        InitializeComponent();
        Initialize();
    }

    private void Initialize()
    {
        viewModel = this.DataContext as DXSplashScreenViewModel;

        if (viewModel != null)
        {
            viewModel.Title = "SmartEMR";
            viewModel.Subtitle = "Loading...";
            viewModel.Logo = new Uri("Images/Svg/logo.svg", UriKind.Relative);
        }
    }

    public void CloseSplashScreen()
    {
    }

    public void Progress(double value)
    {
    }

    public void SetProgressState(bool isIndeterminate)
    {
    }
}
