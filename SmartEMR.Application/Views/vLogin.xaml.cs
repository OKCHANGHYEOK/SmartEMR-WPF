using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

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
            return Model as MemberUser;
        }
    }

    public vLogin() : base()
    {
    }

    protected override void Initialize()
    {
    }

    public override async void OnBindGrid_BindClick(object sender, BindClickEventArgs e)
    {
        if (sender is BindItem bindItem == false) return;

        switch (bindItem.FieldName)
        {
            case "btnLogin":
                if (string.IsNullOrWhiteSpace(MURItem?.MUR_Id))
                {
                    var txtMUR_Id = this.BindGrids[0].GetBindItem<StyleTextBox>("MUR_Id");

                    txtMUR_Id.Focus();

                    SmartUI.ShowRequiredMessage(txtMUR_Id, "아이디를 입력해주세요.");

                    return;
                }
                else if (string.IsNullOrWhiteSpace(MURItem?.MUR_PassWord))
                {
                    var txtMUR_PassWord = this.BindGrids[0].GetBindItem<StyleTextBox>("MUR_PassWord");

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

