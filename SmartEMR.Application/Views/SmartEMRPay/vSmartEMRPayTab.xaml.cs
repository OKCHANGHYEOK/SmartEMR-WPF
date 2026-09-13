using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRPay.Views
{
    /// <summary>
    /// vSmartEMRPayTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRPayTab : ModelViewLayout<PayViewModel>
    {
        public vSmartEMRPayTab() { }

        protected override void Initialize()
        {
        }

        public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }
    }
}