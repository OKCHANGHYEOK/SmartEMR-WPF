using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskPATHistory.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskPATHistory : ModelViewLayout<PatientHistoryViewModel>
    {
        public vSmartEMRDeskPATHistory() { }

        protected override void Initialize()
        {
        }

        public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }

        private async void OnTabControl_SelectionChanged(object sender, TabControlSelectionChangedEventArgs e)
        {
            var element = sender as DXTabControl;
            if (element is null) return;

            var selectedItem = element.SelectedItem as DXTabItem;
            if (selectedItem is null) return;

            var targetHistoryType = selectedItem.Tag.ToString();
            if (string.IsNullOrWhiteSpace(targetHistoryType)) return;

            SmartUI.BeginInvoke(async () =>
            {
                await vm.UpdateHistoryBySelection(targetHistoryType);
            }, System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}