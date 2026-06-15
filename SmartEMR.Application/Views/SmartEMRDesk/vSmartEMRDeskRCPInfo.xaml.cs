using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCPInfo : ModelViewLayout<SmartEMRRCPInfoViewModel>
    {
        public Reception RCPItem
        {
            get
            {
                return vm.Model;
            }
        }

        public Patient PATItem { get; set; } = new();

        protected override void Initialize()
        {
            MaskControl.ShowButton = false;
        }

        protected override void SetBindGrid()
        {
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.Height = 32;
            this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.Margin = new Thickness(2, 0, 2, 0);

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
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            var fieldName = bindItem.FieldName;
            switch (fieldName)
            {
                case "RCP_Subject":
                    var newValue = e.NewValue?.ToString();
                    if (!string.IsNullOrWhiteSpace(newValue) && newValue == "ETC")
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.IsEnabled = true;
                    }
                    else
                    {
                        this.BindGrids[0].GetBindItem<StyleTextBox>("RCP_SubjectName")?.IsEnabled = true;
                    }

                    break;
            }
        }

        public override async Task SetPatientData(Patient item)
        {
            if (item.PAT_Idx.GetValueOrDefault(0) == 0) return;

            // 환자 정보 세팅
            SmartMVVM.ModelProperty.SetPatientData(PATItem, item);

            // 오늘 날짜의 접수정보 조회
            var getRCP = new Reception
            {
                PAT_Idx = item.PAT_Idx,
                RCP_YYMMDD = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var retRCP = await SmartMVVM.DataStore.GetItem<Reception>(eAPI.Reception_GetReception, getRCP);
            if (SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("접수 정보를 불러오지 못했습니다.", NotificationType.Error);
            }

            SetReceptionData(retRCP);
        }

        private void SetReceptionData(Reception? item)
        {
            if (item == null)
            {
                item = new Reception();
            }

            SmartMVVM.ModelProperty.SetReceptionData(RCPItem, item);

            if (RCPItem.RCP_Idx == 0)
            {
                MaskControl.MaskText = "오늘 날짜의 접수내역이 없습니다.";
                MaskControl.ShowButton = true;
            }
            else
            {
                MaskControl.MaskVisibility = Visibility.Collapsed;
            }
        }
    }
}
