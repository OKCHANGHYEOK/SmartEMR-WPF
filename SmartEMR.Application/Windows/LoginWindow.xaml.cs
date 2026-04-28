using SmartEMR.Application.Xpf;
using SmartEMR.Application.ViewModels;
using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;

namespace SmartEMR.Application.Windows;

/// <summary>
/// LoginWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class LoginWindow : Window, IViewLayout
{

    private LoginViewModel vm = new LoginViewModel();

    private readonly List<BindGrid> _bindGrids = new();
    public IReadOnlyList<BindGrid> BindGrids => _bindGrids;

    public LoginWindow() : base()
    {
        InitializeComponent();
        Initialize();
    }

    protected void Initialize()
    {
        this.Title = "SmartEMR - 로그인";
        this.DataContext = vm;
    }

    public void AddBindGrid(BindGrid bindGrid)
    {
        if (!_bindGrids.Contains(bindGrid))
        {
            _bindGrids.Add(bindGrid);
            bindGrid.BindGrid_BindClickEvent += OnBindGrid_BindClick;
        }
    }

    public async void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        if (sender is BindItem bindItem == false) return;

        switch (bindItem.FieldName)
        {
            case "btnLogin":
                var retLogin = await vm.AttemptLogin();

                if (!retLogin.IsSuccess)
                {
                    SmartUI.MsgConfirm("로그인 실패", retLogin.Message ?? "");
                    return;
                };

                this.DialogResult = true;
                this.Close();

                break;
        };
    }
}
