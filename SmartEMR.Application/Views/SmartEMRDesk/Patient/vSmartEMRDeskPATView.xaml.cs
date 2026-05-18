using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskPATView : ModelViewLayout<Patient>
    {
        protected override void Initialize()
        {
            Model.PAT_IsAgreePersonalInfo = "y";
            Model.vPAT_IsAgreePersonalInfo = Model.PAT_IsAgreePersonalInfo == "y" ? "개인정보제공 동의" : "개인정보제공 미동의";
        }

        public override async Task OnBindGrid_BindClick(object sender, BindClickEventArgs e)
        {
            // 클릭 이벤트 구현
        }

        private void OnClick_ImageButton(object sender, System.Windows.RoutedEventArgs e)
        {
            var element = sender as ImageButton;
            if (element == null) return;

            switch (element.Name)
            {
                case "btnCopyAddress":
                    Clipboard.SetText(Model.PAT_Address1 ?? "");

                    MessageBox.Show("주소가 복사되었습니다.");

                    break;
            }
        }
    }
}
