using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vLogin.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vLogin : ModelViewLayout<LoginViewModel>
{
    private MemberUser? MURItem
    {
        get
        {
            var vm = this.DataContext as LoginViewModel;

            if (vm != null)
            {
                return vm.Model;
            }
            else
            {
                return null;
            }
        }
    }

    public vLogin() : base()
    {
    }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
        var bindGrid = sender as BindGrid;
        if (bindGrid == null) return;

        var bindItem = e.BindItem;
        if (bindItem == null) return;

    
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    public void SetMemberUserById(MemberUser item)
    {
        vm.SetMemberUserById(item);
    }

    private async void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not Button element) return;

        switch (element.Tag)
        {
            case "btnLogin":
                if (await LoginAsync())
                {
                    await SmartUI.SendMessage("SuccessLogin", viewType:TargetViewType.ParentView);
                } 

                break;

            case "btnSignUp":
                await SmartUI.SendMessage("ShowSignUp", viewType: TargetViewType.ParentView);
                break;
        }
    }

    private async Task<bool> LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(MURItem?.MUR_Id))
        {
            var txtMUR_Id = this.BindGrids[0].GetBindItem<StyleTextBox>("MUR_Id");
            if (txtMUR_Id == null) 
                return false;

            txtMUR_Id.Focus();

            SmartUI.ShowRequiredMessage(txtMUR_Id, "아이디를 입력해주세요.");

            return false;
        }
        else if (string.IsNullOrWhiteSpace(MURItem?.MUR_PassWord))
        {
            var txtMUR_PassWord = this.BindGrids[0].GetBindItem<StyleTextBox>("MUR_PassWord");
            if (txtMUR_PassWord == null) 
                return false;

            txtMUR_PassWord.Focus();

            SmartUI.ShowRequiredMessage(txtMUR_PassWord, "비밀번호를 입력해주세요.");

            return false;
        }

        var retLogin = await vm.AttemptLogin();

        if (!retLogin.IsSuccess)
        {
            SmartUI.MsgConfirm($"로그인 실패 {retLogin.Message}");
            return false;
        }

        return true;
    }
}

