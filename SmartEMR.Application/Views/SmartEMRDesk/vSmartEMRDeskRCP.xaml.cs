using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCP : ModelViewLayout<ReceptionViewModel>
    {
        protected override void Initialize()
        {
        }

        public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }
    }
}
