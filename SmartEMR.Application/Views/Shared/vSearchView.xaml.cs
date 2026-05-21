using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

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

    public void SetFocusToSearch()
    {
        this.txtSearch.Focus();
    }
}
