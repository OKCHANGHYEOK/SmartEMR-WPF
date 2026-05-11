using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSmartEMRChartSummary.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRChartSummary : CustomControl
{
    public vSmartEMRChartSummary()
    {
        InitializeComponent();
        this.DataContext = new ChartSummaryViewModel();
    }
}