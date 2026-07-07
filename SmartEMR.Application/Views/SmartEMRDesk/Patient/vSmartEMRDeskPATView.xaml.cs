using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskPATInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskPATView : ModelViewLayout<PatientViewModel>
    {
        private Patient PATItem => vm.Model;

        protected override void Initialize()
        {
           
        }

        public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            // 클릭 이벤트 구현
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }

        public override async void SetPatientData(Patient item)
        {
            if (PATItem.PAT_Idx == item.PAT_Idx) return;

            var ret = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
            if (ret == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("환자정보 로딩중 오류가 발생했습니다. 다시 시도해주세요", NotificationType.Error);
                return;
            } 

            SmartMVVM.ModelProperty.SetPatientData(PATItem, ret);
        }

        public void ClearData()
        {
            vm.ClearData();
        }

        private void OnClick_ImageButton(object sender, System.Windows.RoutedEventArgs e)
        {
            var element = sender as ImageButton;
            if (element == null) return;

            switch (element.Name)
            {
                case "btnCopyAddress":
                    Clipboard.SetText(PATItem.PAT_Address1 ?? "");

                    MessageBox.Show("주소가 복사되었습니다.");

                    break;
            }
        }

        private async void OnClick_Button(object sender, RoutedEventArgs e)
        {
            var element = sender as Button;
            if (element == null) return;

            switch (element.Name)
            {
                case "btnClear":
                    await SmartUI.SendMessage("ClearPatient", viewType:TargetViewType.PageView);
                    break;

                case "btnMovePAT":
                    await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = PATItem.PAT_Idx }) ,isPopup:true);
                    break;
            }
        }
    }
}
