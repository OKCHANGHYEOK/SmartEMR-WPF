using System.Windows;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCP : ModelViewLayout<SmartEMRDeskRCPViewModel>
    {
        protected override void Initialize()
        {
        }

        protected override void SetBindGrid()
        {
            foreach (var item in this.BindGrids[0].BindItems)
            {
                if (item is BindItem bindItem && bindItem.IsBinding)
                {
                    var element = this.BindGrids[0].GetBindItem<ComboBoxEdit>(bindItem.FieldName);
                    if (element != null)
                    {
                        element.Height = 32;
                        element.Margin = new Thickness(2, 2, 2, 0);
                        element.VerticalAlignment = VerticalAlignment.Center;
                    }
                }
            }

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.ItemsSource = vm.arrMUR_DOC;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Status")?.ItemsSource = vm.arrRCP_Status;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Route")?.ItemsSource = vm.arrRCP_Route;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_VisitType")?.ItemsSource = vm.arrRCP_VisitType;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("IRC_Type")?.ItemsSource = vm.arrRCP_InsuranceType;
        }

        public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;

            switch (bindItem.FieldName)
            {
                case "btnSetToday":
                    vm.SetToDay();
                    break;

                case "btnClearFilter":
                    vm.ClearData();
                    break;
            }
        }

        public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            switch (bindItem.FieldName)
            {
                case "MUR_Idx_DOC" or "RCP_Status" or
                     "RCP_Route" or "RCP_VisitType" or "IRC_Type":

                    await vm.FetchDataAsync();

                    break;
            }
        }

        public async void RefreshData()
        {
            await vm.FetchDataAsync();
        }
    }
}
