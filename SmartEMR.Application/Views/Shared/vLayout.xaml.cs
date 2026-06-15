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

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var vl = MainContent as ViewLayout;
        if (vl == null) return null;

        return await vl.ReceiveMessage(request);
    }

    public override Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        return Task.CompletedTask;
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    private void OnPreviewKeyDown_vLayout(object sender, KeyEventArgs e) 
    {
        if (e.Key == Key.F6)
        {
            SmartUI.SendMessageToSearchView("SetFocusToSearchText");
        }
    }
}
