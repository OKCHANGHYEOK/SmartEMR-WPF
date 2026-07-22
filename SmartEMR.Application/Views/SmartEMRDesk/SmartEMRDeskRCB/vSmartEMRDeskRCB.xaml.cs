using System.Windows;
using System.Windows.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Views.SmartEMRRCP;
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

            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("MUR_Idx_DOC")?.ItemsSource = vm.arrMUR_DOC;

            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_Status")?.ItemsSource = vm.arrRCP_Status;
            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCP_InsuranceType")?.ItemsSource = vm.arrRCP_InsuranceType;

            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("RES_Status")?.ItemsSource = vm.arrRES_Status;

            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCB_Subject")?.ItemsSource = vm.arrRCB_Subject;
            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCB_Route")?.ItemsSource = vm.arrRCB_Route;
            //this.BindGrids[0].GetBindItem<ComboBoxEdit>("RCB_VisitType")?.ItemsSource = vm.arrRCB_VisitType;

            this.BindGrids[0].GetBindItem<DateEdit>("RCB_YYMMDD")?.ShowToday = false;
            this.BindGrids[0].GetBindItem<DateEdit>("RCB_YYMMDD")?.ShowClearButton = false;
        }

        protected override void SetDataGrid()
        {
 
        }

        public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
        {
            var response = new ViewMessageResponse { IsSuccess = false };

            switch (request.MessageAction)
            {
                case "SetFocusToSearch":
                    this.BindGrids[0].GetBindItem<SearchEdit>("Keyword")?.Focus();
                    break;

                case "RefreshDataList":
                    await vm.FetchDataAsync();
                    break;
            }

            return response;
        }

        public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;

            switch (bindItem.FieldName)
            {
                case "btnSetToday":
                    vm.SetRCB_YYMMDD(DateTime.Now.ToString("yyyy-MM-dd"));
                    break;
            }
        }

        public override async void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
            var bindGrid = sender as BindGrid;
            if (bindGrid == null) return;

            var bindItem = e.BindItem;
            if (bindItem == null) return;

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
                await SmartUI.SendMessage("SetPatientByRCB", dataItem, TargetViewType.PageView);
                return;
            }

            switch (fieldName)
            {
                case "PAT_Name":
                    await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = dataItem.PAT_Idx }), isPopup:true);
                    break;
            }
        }

        public override void OnDataGrid_PopupMenuOpening(object? sender, PopupMenuOpeningEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null) return;

            var popup = e.PopupMenu;
            if (popup == null) return;

            var dataItem = e.DataItem as ReceptionBoard;
            if (dataItem == null) return;

            popup.AddMenu(new PopupMenuItem { MenuAction = "SearchPAT", Content = $"{dataItem.PAT_Name}님으로 검색", Glyph = GlyphImage("Images/smartemr_find_glasses.png") });
            popup.AddMenu(new PopupMenuItem { MenuAction = "EditPAT", Content = "환자수정", Glyph = GlyphImage("Images/smartemr_edit_patient.png") });
            popup.AddSeperator();
            popup.AddMenu(new PopupMenuItem { MenuAction ="EditRCP", Content = "접수수정", Glyph = GlyphImage("Images/smartemr_edit_paper.png") });
            popup.AddMenu(new PopupMenuItem { MenuAction = "CancelRCP", Content = "접수취소", Glyph = GlyphImage("Images/smartemr_cancel_paper.png") });
        }

        public override async void OnDataGridPopupMenu_PopupMenuItemClicked(object? sender, PopupMenuItemClickEventArgs e)
        {
            var popup = sender as PopupMenu;
            if (popup == null) return;

            var dataItem = e.DataItem as ReceptionBoard;
            if (dataItem == null) return;

            switch (e.MenuAction)
            {
                case "SearchPAT":
                    await SmartUI.SendMessageToSearchView("SetSelectedPatient", new Patient { PAT_Idx = dataItem.PAT_Idx });
                    break;

                case "EditPAT":
                    await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = dataItem.PAT_Idx }), isPopup:true);
                    break;

                case "EditRCP":
                    await SmartUI.NavigateToPage(new vSmartEMRRCPInfoTab(new Reception { RCP_Idx = dataItem.RCP_Idx }), isPopup:true);
                    break;

                case "CancelRCP":
                    await vm.CancelRCP(new Reception { RCP_Idx = dataItem.RCP_Idx });
                    break;
            }
        }

        public async void RefreshData()
        {
            await vm.FetchDataAsync();
        }
    }
}
