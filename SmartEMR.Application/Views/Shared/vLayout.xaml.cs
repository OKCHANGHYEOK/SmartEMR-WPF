using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
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
    public override IReadOnlyList<DataGrid> DataGrids => default!;

    public vLayout()
    {
        this.Loaded += async (s, e) =>
        {
            this.Focus();
        };
    }

    public vLayout(Type T) : this()
    {
        MainContent = Activator.CreateInstance(T) as IViewLayout ?? default!;
    }

    public void SetIndicatorVisibility(bool visibility)
    {
        LayoutWaitIndicator.DeferedVisibility = visibility;
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var vl = MainContent as ViewLayout;
        if (vl == null) return null;

        return await vl.ReceiveMessage(request);
    }

    public override Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        return Task.CompletedTask;
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e) {}

    public override void OnDataGrid_DataItemChanged(object? sender, DataItemChangedEventArgs e) {}

    public override void OnDataGrid_ContextMenuItemClicked(object? sender, ContextMenuItemClickedEventArgs e) {}

    private async void OnPreviewKeyDown_vLayout(object sender, KeyEventArgs e) 
    {
        var vl = sender as vLayout;
        if (vl == null) return;

        switch (e.Key)
        {
            case Key.F5:
                await SmartUI.RefreshCurrentPage();
                break;

            case Key.F6:
                await SmartUI.SendMessageToSearchView("SetFocusToSearchText");
                break;
        }
    }
}
