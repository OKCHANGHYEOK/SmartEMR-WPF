using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCVInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCB : ModelViewLayout<SmartEMRDeskRCBViewModel>
    {
        protected override async void Initialize()
        {
            MaskControl.ShowButton = false;

            await vm.FetchDataAsync();
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
                        element.Height = 23;
                        element.Margin = new Thickness(2, 0, 2, 0);
                        element.VerticalAlignment = VerticalAlignment.Center;
                    }
                }
            }

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.ItemsSource = vm.arrMUR_DOC;

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Status")?.ItemsSource = vm.arrRCP_Status;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_InsuranceType")?.ItemsSource = vm.arrRCP_InsuranceType;

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RES_Status")?.ItemsSource = vm.arrRES_Status;

            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCB_Subject")?.ItemsSource = vm.arrRCB_Subject;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCB_Route")?.ItemsSource = vm.arrRCB_Route;
            this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCB_VisitType")?.ItemsSource = vm.arrRCB_VisitType;

            this.BindGrids[0].GetBindItem<DateEdit>("RCB_YYMMDD")?.ShowToday = false;
            this.BindGrids[0].GetBindItem<DateEdit>("RCB_YYMMDD")?.ShowClearButton = false;

            this.BindGrids[0].GetBindItem<SearchEdit>("Keyword")?.MinHeight = 23;
        }

        public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
        {
            var response = new ViewMessageResponse { IsSuccess = false };

            switch (request.MessageAction)
            {
                case "SearchData":
                    await vm.SearchData();
                    break;

                case "ClearFilter":
                    vm.ClearData();

                    SmartUI.SetNofification("필터 초기화되었습니다.", NotificationType.Info);

                    break;

                case "SetFocusToSearch":
                    this.BindGrids[0].GetBindItem<SearchEdit>("keyword")?.Focus();
                    break;
            }

            return response;
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
            }
        }

        public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

            if (string.IsNullOrWhiteSpace(bindItem.FieldName)) return;

            await vm.FetchDataAsync();
        }

        public override async void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null) return;

            var fieldName = e.Column.FieldName;
            var dataItem = e.DataItem as ReceptionBoard;
            if (dataItem == null) return;
            
            if (dataGrid.IsDoubleClicked)
            {
                var item = new Patient
                {
                    PAT_Idx = dataItem.PAT_Idx,
                    PAT_Name = dataItem.PAT_Name,
                    PAT_ChartNo = dataItem.PAT_ChartNo
                };

                await SmartUI.SendMessageToSearchView("SetSelectedPatient", item);
                return;
            }

            switch (fieldName)
            {
                case "PAT_Name":
                    await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = dataItem.PAT_Idx }), isPopup:true);
                    break;
            }
        }

        public async void RefreshData()
        {
            await vm.FetchDataAsync();
        }
    }
}
