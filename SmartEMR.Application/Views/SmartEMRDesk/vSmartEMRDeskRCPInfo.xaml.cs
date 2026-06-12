using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCVInfo : ModelViewLayout<SmartEMRRCPInfoViewModel>
    {
        public Reception RCPItem
        {
            get
            {
                return vm.Model;
            }
        }

        protected override void Initialize()
        {

        }

        protected override void SetBindGrid()
        {
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.ItemsSource = vm.arrMUR_DOC;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.SelectedIndex = 0;

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_STF")?.ItemsSource = vm.arrMUR_STF;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_STF")?.SelectedIndex = 0;
        }

        public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
        {
            // 클릭 이벤트 구현
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }
    }
}
