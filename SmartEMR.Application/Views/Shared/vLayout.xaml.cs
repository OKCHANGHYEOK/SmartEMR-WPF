using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Shared;

[ObservableObject]
/// <summary>
/// vLayout.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLayout : ViewLayout
{
    private IViewLayout _mainContent = default!;

    public IViewLayout MainContent
    {
        get => _mainContent;
        set => SetProperty(ref _mainContent, value, nameof(MainContent));
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
            case "SetFocusToSearchText":
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
        var searchView = SmartUI.GetPageView<vSearchView>();

        if (searchView != null)
        {
            searchView.SetFocusToSearch();
        }
    }
}
