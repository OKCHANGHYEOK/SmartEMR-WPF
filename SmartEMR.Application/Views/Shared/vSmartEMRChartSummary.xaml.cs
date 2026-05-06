using SmartEMR.Application.ViewModels;
using System.Windows.Controls;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSmartEMRChartSummary.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRChartSummary : UserControl
{
    public vSmartEMRChartSummary()
    {
        InitializeComponent();
        this.DataContext = new ChartSummaryViewModel();
    }
}