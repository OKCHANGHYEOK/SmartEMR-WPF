using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Controls;
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

    public static DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(Patient), typeof(vSearchView), new PropertyMetadata(null));

    public Patient? SelectedItem
    {
        get => (Patient)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
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

    public void OnPreviewKeyDown_Popup(object sender, KeyEventArgs e)
    {
        var listBox = SearchViewResult.FindName("ResultListBox") as ListBox;
        if (listBox == null) return;

        SelectedItem = listBox.SelectedItem as Patient;

        // 만약 첫 번째 아이템에서 위 방향키를 누르면 다시 검색창으로 포커스 복귀하는 로직만 구현
        if (e.Key == Key.Up && listBox.SelectedIndex == 0)
        {
            SetFocusToSearch();
            e.Handled = true;
        }
    }

    private void txtSearch_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Down && SearchPopup.IsOpen)
        {
            // SearchViewResult 내부의 ListBox를 찾음
            var listBox = SearchViewResult.FindName("ResultListBox") as ListBox;
            if (listBox != null && listBox.Items.Count > 0)
            {
                // 1. ListBox로 포커스를 이동시킴 (WPF가 자동으로 첫 번째 아이템을 선택해 줌)
                listBox.Focus();

                // 2. 필요하다면 첫 번째 아이템을 강제로 선택 상태로 만듦
                if (listBox.SelectedIndex == -1) listBox.SelectedIndex = 0;

                e.Handled = true;
            }
        }
    }

    public void SetFocusToSearch()
    {
        this.txtSearch.Focus();
    }
}
