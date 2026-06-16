using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskIRCInfo : ModelViewLayout<SmartEMRIRCInfoViewModel>
    {
        public Patient PATItem { get; set; } = new();

        protected override void Initialize()
        {
            MaskControl.ShowButton = false;
        }

        protected override void SetBindGrid()
        {
        }

        public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
        {

        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            var fieldName = bindItem.FieldName;

        }

        public override async Task SetPatientData(Patient item)
        {

        }

        public void ClearData()
        {

        }
    }
}
