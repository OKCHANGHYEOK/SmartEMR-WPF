using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views.MemberUsers;

/// <summary>
/// vMemberUserInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vMemberUserInfo : ModelViewLayout<MemberUserInfoViewModel>
{
    private bool _isUpdatedLayoutByMUR = false;

    protected override void Initialize()
    {
    }

    protected override void SetBindGrid()
    {
    }

    public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
        if (sender is not BindGrid) return;


    }

    public void UpdateLayoutByMUR()
    {
        if (_isUpdatedLayoutByMUR) return;

        if (!SmartMVVM.AppSession.IsMED)
        {
            var txtMUR_LicenseNo = this.BindGrids[1].GetBindItem<StyleTextBox>("MUR_LicenseNo");
            if (txtMUR_LicenseNo != null)
            {
                txtMUR_LicenseNo.IsReadOnly = true;
                txtMUR_LicenseNo.IsEnabled = false;
            }
        }

        if (!SmartMVVM.AppSession.IsAdmin)
        {
            var cmbMUR_Department = this.BindGrids[1].GetBindItem<ComboBoxEdit>("MUR_Department");
            if (cmbMUR_Department != null)
            {
                cmbMUR_Department.IsReadOnly = true;
                cmbMUR_Department.IsEnabled = false;
            }

            var cmbMUR_JobCode = this.BindGrids[1].GetBindItem<ComboBoxEdit>("MUR_JobCode");
            if (cmbMUR_JobCode != null)
            {
                cmbMUR_JobCode.IsReadOnly = true;
                cmbMUR_JobCode.IsEnabled = false;
            }
        }

        _isUpdatedLayoutByMUR = true;
    }

    private void OnClick_SimpleButton(object sender, System.Windows.RoutedEventArgs e)
    {

    }
}
