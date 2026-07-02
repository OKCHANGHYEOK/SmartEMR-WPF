using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
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

        public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
            // 클릭 이벤트 구현
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }

        public override async void SetPatientData(Patient item)
        {
            if (Model.PAT_Idx == item.PAT_Idx) return;

            var ret = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
            if (ret == null || SmartMVVM.DataStore.retIsSuccess == false)
            {
                SmartUI.SetNofification("환자정보 로딩중 오류가 발생했습니다. 다시 시도해주세요", NotificationType.Error);
                return;
            } 

            SmartMVVM.ModelProperty.SetPatientData(Model, ret);
        }

        public void ClearData()
        {
            Model.PAT_Idx = 0;
            Model.MUR_Idx_DOC = 0;
            Model.MUR_Idx_STF = 0;
            Model.PAT_ChartNo = "";
            Model.PAT_Name = "";
            Model.PAT_BloodType = "";
            Model.PAT_SourceType = "";
            Model.vPAT_SourceType = "";
            Model.PAT_Sex = "";
            Model.PAT_Age = 0;
            Model.vPAT_Info = "";
            Model.PAT_BirthYear = "";
            Model.PAT_BirthMonth = "";
            Model.PAT_BirthDay = "";
            Model.PAT_BirthDate = "";
            Model.PAT_RegisterNum1 = "";
            Model.PAT_RegisterNum2 = "";
            Model.PAT_Address1 = "";
            Model.PAT_Address2 = "";
            Model.PAT_Address3 = "";
            Model.vPAT_Address = "";
            Model.PAT_Hpp1 = "";
            Model.PAT_Hpp2 = "";
            Model.PAT_Hpp3 = "";
            Model.PAT_PhoneNum = "";
            Model.PAT_Email = "";
            Model.PAT_FirstVisitDate = "";
            Model.PAT_LastVisitDate = "";
            Model.PAT_ImageSource = null;
            Model.PAT_IsSMS = "";
            Model.PAT_IsSolar = "";
            Model.PAT_Bigo = "";
            Model.NOW_CHT_Idx_RCV = 0;
            Model.NOW_CHT_Idx_RES = 0;
            Model.NEXT_CHT_Idx_RES = 0;
            Model.NEXT_CHT_DATE_RES = "";
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
                    await SmartUI.NavigateToPage(new vPatientInfo(new Patient { PAT_Idx = Model.PAT_Idx }) ,isPopup:true);
                    break;
            }
        }
    }
}
