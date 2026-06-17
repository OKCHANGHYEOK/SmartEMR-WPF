using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRRESInfo : ModelViewLayout<Chart>
    {
        protected override void Initialize()
        {
            this.ViewTitle = "예약등록";
            this.ViewSize = new System.Windows.Size(500, 500);
        }

        public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            if (sender is BindGrid bg)
            {

            }
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }
    }
}
