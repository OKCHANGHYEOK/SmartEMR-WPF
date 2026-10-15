using DevExpress.Xpf.Core;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Members;

/// <summary>
/// vMemberInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vMemberInfo : ModelViewLayout<MemberInfoViewModel>
{
    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    private void OnPreviewTextInput_TextEdit(object sender, System.Windows.Input.TextCompositionEventArgs e)
    {
        if (sender is not TextEdit element) return;

        switch (element.Name)
        {
            case nameof(txtMEM_MediNo) or 
                 nameof(txtMEM_Tel) or 
                 nameof(txtMEM_Address3):

                if (!SmartMVVM.Common.CanInputTextByNum(e.Text))
                {
                    e.Handled = true;
                    return;
                }

                break;
        }
    }

    private void OnClick_Button(object sender, System.Windows.RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Name)
        {
            case nameof(btnClose):
                SmartUI.CloseView();
                break;
        }
    }
}
