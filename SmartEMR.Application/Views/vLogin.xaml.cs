using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views;

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

    public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        if (sender is BindItem bindItem == false) return;

        switch (bindItem.FieldName)
        {
            case "btnLogin":
                if (string.IsNullOrWhiteSpace(MURItem?.MUR_Id))
                {
                    var txtMUR_Id = this.BindGrids[0].GetBindItem<StyleTextBox>("MUR_Id");
                    if (txtMUR_Id == null) return;

                    txtMUR_Id.Focus();

                    SmartUI.ShowRequiredMessage(txtMUR_Id, "아이디를 입력해주세요.");

                    return;
                }
                else if (string.IsNullOrWhiteSpace(MURItem?.MUR_PassWord))
                {
                    var txtMUR_PassWord = this.BindGrids[0].GetBindItem<StyleTextBox>("MUR_PassWord");
                    if (txtMUR_PassWord == null) return;

                    txtMUR_PassWord.Focus();

                    SmartUI.ShowRequiredMessage(txtMUR_PassWord, "비밀번호를 입력해주세요.");
                    
                    return;
                }   

                var retLogin = await vm.AttemptLogin();

                if (!retLogin.IsSuccess)
                {
                    SmartUI.MsgConfirm("로그인 실패", retLogin.Message ?? "");
                    return;
                }

                SmartUI.UIManager.CloseWindow(TargetWindowType.CurrentWindow);

                break;
        }
    }
}

