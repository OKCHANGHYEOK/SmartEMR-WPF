using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Views.SmartEMRRES;

/// <summary>
/// vSmartEMRRESInfo_PatientInfo.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRRESInfo_PatientInfo : UserControl
{
    public vSmartEMRRESInfo_PatientInfo()
    {
        InitializeComponent();
    }

    private void OnClick_Button(object sender, RoutedEventArgs e)
    {
        var element = sender as Button;
        if (element == null) return;

        var PATItem = this.DataContext as Patient;
        if (PATItem is null) return;

        switch (element.Name)
        {
            case "btnSelectImage":
                var fileResult = SelectImage();
                if (fileResult == null) return;

                PATItem.PAT_ImageSource = fileResult;

                break;

            case "btnClearImage":
                if (PATItem.PAT_ImageSource != null && PATItem.PAT_ImageSource.Length > 0)
                {
                    PATItem.PAT_ImageSource = null;
                }

                break;

            case "btnFindAddress":
                SmartUI.SetNofification("현재 지원하지 않는 기능입니다.", NotificationType.Warning);
                break;
        }
    }
}
