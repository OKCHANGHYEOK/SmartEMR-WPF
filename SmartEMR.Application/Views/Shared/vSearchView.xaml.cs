using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSearchView.xaml에 대한 상호 작용 논리
/// </summary>
[ObservableObject]
public partial class vSearchView : ModelViewLayout<SearchViewModel>
{
    public static DependencyProperty IsPopupOpenProperty = 
        DependencyProperty.Register(nameof(IsPopupOpen), typeof(bool), typeof(vSearchView), new PropertyMetadata(false));

    public bool IsPopupOpen
    {
        get => (bool)GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }

    private bool isPreventClickEvent = false;

    public vSearchView()
    {
    }

    protected override void Initialize()
    {
        txtSearch.PreviewKeyDown += OnPreviewKeyDown_txtSearch;
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e) {}

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse { IsSuccess = false };

        switch (request.MessageAction)
        {
            case "SetFocusToSearchText":
                SetFocusToSearch();
                break;

            case "SetSelectedPatient":
                var paramItem = request.MessageParameter as Patient;
                if (paramItem == null) return null;

                SetPatientData(paramItem);

                break;

            case "ClearPAT":
                txtSearch.Text = "";
                break;

            case "UpdateSearchItemsSource":
                if (request.MessageParameter != null && request.MessageParameter is IQueryable<Patient> arrPAT)
                {
                    SearchViewResult.UpdateItemsSource(arrPAT);
                    
                    IsPopupOpen = true;
                }
                
                break;
        }

        response.IsSuccess = true;

        return response;
    }

    public void SetFocusToSearch()
    {
        this.txtSearch.Focus();
    }


    #region "Event & Function"

    public override async void SetPatientData(Patient item)
    {
        if (!string.IsNullOrWhiteSpace(item.PAT_ChartNo) && !string.IsNullOrWhiteSpace(item.PAT_Name))
        {
            txtSearch.Text = item.PAT_Name + "(" + item.PAT_ChartNo + ")";
        }

        IsPopupOpen = false;

        await SmartUI.SendMessage("SetSelectedPatient", item, viewType: TargetViewType.PageView);
        
        SmartUI.SetNofification("선택하신 환자정보가 적용되었습니다.", NotificationType.Info);
    }

    public void OnPreviewKeyDown_SearchView(object sender, KeyEventArgs e)
    {
        var element = sender as vSearchView;
        if (element == null) return;

        if (e.Key == Key.Escape)
        {
            IsPopupOpen = false;
        }
    }

    public void OnPreviewKeyDown_Popup(object sender, KeyEventArgs e)
    {
        var popup = sender as Popup;
        if (popup == null) return;

        // 만약 첫 번째 아이템에서 위 방향키를 누르면 다시 검색창으로 포커스 복귀하는 로직만 구현
        if (e.Key == Key.Up && SearchViewResult.SelectedIndex == 0)
        {
            this.txtSearch.Focus();

            SearchViewResult.SetSelectedIndex(-1);

            e.Handled = true;
        }
    }

    private void OnOpened_Popup(object sender, EventArgs e)
    {
        var popup = sender as Popup;
        if (popup == null) return;
        
        if (!isPreventClickEvent)
        {
            isPreventClickEvent = true;
        }
    }

    private void OnClosed_Popup(object sender, EventArgs e)
    {
        var popup = sender as Popup;
        if (popup == null) return;

        if (isPreventClickEvent)
        {
            isPreventClickEvent = false;
        }

        SmartUI.ReturnFocusTovLayout();
    }

    private async void OnPreviewKeyDown_txtSearch(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            e.Handled = true;

            await vm.Search();
        }
        
        if (e.Key == Key.Down && SearchPopup.IsOpen)
        {
            e.Handled = true;

            SearchViewResult.FocusToResultListBox();
        }
    }

    private async void OnClick_Button(object sender, RoutedEventArgs e)
    {
        var element = sender as Button;
        if (element == null) return;

        if (isPreventClickEvent) return;
        
        switch (element.Name)
        {
            case "btnMoveRESInfo":
                break;

            case "btnMovePATInfo":
                await SmartUI.NavigateToPage(new vPatientInfo(), isPopup: true);
                break;
        }
    }

    #endregion
}
