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

    public vSearchView()
    {
    }

    protected override void Initialize()
    {
        txtSearch.PreviewKeyDown += txtSearch_PreviewKeyDown;
    }

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
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

            case "UpdateSearchItemsSource":
                if (request.MessageParameter != null && request.MessageParameter is IQueryable<Patient> arrPAT)
                {
                    SearchViewResult.UpdateItemsSource(arrPAT);
                    
                    IsPopupOpen = true;
                }
                
                break;

            case "ClosePopup":
                IsPopupOpen = false;
                break;
        }

        response.IsSuccess = true;

        return response;
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

    public void OnClosed_Popup(object sender, EventArgs e)
    {
        var popup = sender as Popup;
        if (popup == null) return;

        this.Focus();
    }

    private void txtSearch_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down && SearchPopup.IsOpen)
        {
            SearchViewResult.FocusToResultListBox();
            e.Handled = true;
        }
    }

    public void SetFocusToSearch()
    {
        this.txtSearch.Focus();
    }
}
