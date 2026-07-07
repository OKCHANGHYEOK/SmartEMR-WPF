using System.Windows;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Views;

/// <summary>
/// vSmartEMRDeskTab.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSmartEMRDeskTab : ModelViewLayout<DeskViewModel>
{

    public vSmartEMRDeskTab() { }

    protected override void Initialize()
    {
    }

    public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
    {
        var response = new ViewMessageResponse() { IsSuccess = false};

        switch (request.MessageAction)
        {
            case "GetIRCItem":
                response.Item = SmartEMRDeskIRCInfo.IRCItem;
                break;

            case "SetSelectedPatient":
                {
                    var paramItem = request.MessageParameter as Patient;
                    if (paramItem == null) return null;

                    SmartEMRDeskPATView.SetPatientData(paramItem);
                    SmartEMRDeskRCPInfo.SetPatientData(paramItem);

                    break;
                }

            case "SetInsurance":
                {
                    var paramItem = request.MessageParameter as Insurance;
                    if (paramItem == null) return null;

                    SmartEMRDeskIRCInfo.SetInsurance(paramItem);

                    break;
                }

            case "SetInsuranceType":
                {
                    var paramItem = request.MessageParameter?.ToString();
                    if (paramItem == null) return null;

                    SmartEMRDeskIRCInfo.SetInsuranceType(paramItem);
                    break;
                }

            case "MoveInsurance":
                SmartEMRDeskIRCInfo.SetMaskVisibility(Visibility.Collapsed);
                break;

            case "RefreshReception":
                SmartEMRDeskRCP.RefreshData();
                break;

            case "ClearPatient":
                ClearData();
                break;

            case "ClearReception":
                {
                    var paramItem = request.MessageParameter as Reception;
                    if (paramItem != null && paramItem.RCP_Idx == SmartEMRDeskRCPInfo.RCPItem.RCP_Idx)
                    {
                        ClearData(false);
                    }

                    break;
                }
        }

        response.IsSuccess = true;

        return response;
    }

    private async void ClearData(bool isClearPAT = true)
    {
        if (isClearPAT) 
        {
            await SmartUI.SendMessageToSearchView("ClearPatient");

            SmartEMRDeskPATView.ClearData();
        }

        SmartEMRDeskRCPInfo.ClearData();
        SmartEMRDeskIRCInfo.ClearData();
    }

    public override async Task OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}
