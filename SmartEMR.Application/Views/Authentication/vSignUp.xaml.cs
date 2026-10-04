using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Authentication;

/// <summary>
/// vSignUp.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSignUp : ModelViewLayout<MemberViewModel>
{
    public vSignUp() { }

    protected override void Initialize()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }

    private void OnEditValueChanged_PasswordEdit(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
    {

    }
}