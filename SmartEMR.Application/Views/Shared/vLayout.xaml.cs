using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.Views.Shared;

[ObservableObject]
/// <summary>
/// vLayout.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLayout : ViewLayout
{
    public static DependencyProperty MainContentProperty =
        DependencyProperty.Register("MainContent", typeof(IViewLayout), typeof(vLayout), new PropertyMetadata(null));

    public IViewLayout MainContent
    {
        get => (IViewLayout)GetValue(MainContentProperty);
        set => SetValue(MainContentProperty, value);
    }

    public override IReadOnlyList<BindGrid> BindGrids => default!;

    public vLayout()
    {
    }

    public vLayout(Type T) : this()
    {
        MainContent = Activator.CreateInstance(T) as IViewLayout ?? default!;
    }


    public override Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        return Task.CompletedTask;
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        switch (request.MessageAction) 
        {
            case "vSearchView_SetFocusToSearchText":
                SetFocusToSearchView();
                break;
        }

        return null;
    }

    private void OnPreviewKeyDown_vLayout(object sender, KeyEventArgs e) 
    {
        if (e.Key == Key.F6)
        {
            SetFocusToSearchView();
        }
    }

    private void SetFocusToSearchView()
    {
        var searchView = SmartUI.GetViewLayout<vSearchView>();

        if (searchView != null)
        {
            searchView.SetFocusToSearch();
        }
    }
}
