using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

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
            case "GetPATItem":
                response.Item = vm.PATItem;
                break;

            case "SetSelectedPatient":
                {
                    var paramItem = request.MessageParameter as Patient;
                    if (paramItem == null) return null;

                    SetPatientData(paramItem);

                    break;
                }

            case "SetPatientByRCB":
                {
                    var paramItem = request.MessageParameter as ReceptionBoard;
                    if (paramItem != null)
                    {
                        Patient item = new Patient
                        {
                            PAT_Idx = paramItem.PAT_Idx,
                            PAT_Name = paramItem.PAT_Name,
                            PAT_ChartNo = paramItem.PAT_ChartNo
                        };

                        await SmartUI.SendMessageToSearchView("SetSelectedPatient", item);

                        if (paramItem.RCP_Idx.GetValueOrDefault(0) > 0 || paramItem.RES_Idx.GetValueOrDefault(0) > 0)
                        {
                            Reception RCPItem = SmartMVVM.ModelProperty.GetReceptionDataFromRCB(paramItem);
                            
                            SmartEMRDeskRCPInfo.UpdateReceptionData(RCPItem);
                        }
                    }

                    break;
                }

            case "SetReception":
                {
                    var parameter = (SaveMode?)request.MessageParameter;

                    if (parameter is SaveMode operation)
                    {
                        await vm.SaveDataAsync(operation);
                    }

                    break;
                }

            case "SetRCPItem":
                {
                    var paramItem = request.MessageParameter as Reception;
                    if (paramItem != null)
                    {
                        vm.SetRCPItem(paramItem);
                    }

                    break;
                }

            case "SetInsurance":
                {
                    var paramItem = request.MessageParameter as Insurance;
                    if (paramItem == null) return null;

                    SmartEMRDeskIRCInfo.SetInsurance(paramItem);

                    break;
                }

            case "SetIRCItem":
                {
                    var paramItem = request.MessageParameter as Insurance;
                    if (paramItem != null)
                    {
                        vm.SetIRCItem(paramItem);
                    }

                    break;
                }

            case "SetInsuranceType":
                {
                    var paramItem = request.MessageParameter?.ToString();
                    if (paramItem == null) return null;

                    SmartEMRDeskIRCInfo.SetInsuranceType(paramItem);
                    break;
                }

            case "UpdateRCPInfo":
                {
                    var paramItem = request.MessageParameter as Reception;
                    if (paramItem != null && paramItem.RCP_Idx == SmartEMRDeskRCPInfo.RCPItem.RCP_Idx)
                    {
                        SmartEMRDeskRCPInfo.UpdateReceptionData(paramItem);
                    } 

                    break;
                }

            case "RefreshRCB":
                SmartEMRDeskRCB.RefreshData();
                break;

            case "ClearPAT":
                ClearData();
                break;

            case "ClearRCP":
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

    public override async Task SetPatientData(Patient item)
    {
        var ret = await SmartMVVM.DataStore.GetItem<Patient>(eAPI.Patient_GetPatient, new Patient { PAT_Idx = item.PAT_Idx });
        if (ret == null || SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("환자정보 로딩중 오류가 발생했습니다. 다시 시도해주세요", NotificationType.Error);
            return;
        }

        vm.SetPatientData(ret);

        await SmartEMRDeskPATView.SetPatientData(ret);
        await SmartEMRDeskRCPInfo.SetPatientData(ret);
        await SmartEMRDeskPATHistory.SetPatientData(ret);
    }

    private async void ClearData(bool isClearPAT = true)
    {
        if (isClearPAT) 
        {
            await SmartUI.SendMessageToSearchView("ClearPAT");

            SmartEMRDeskPATView.ClearData();
            SmartEMRDeskPATHistory.ClearData();
        }

        SmartEMRDeskRCPInfo.ClearData();
        SmartEMRDeskIRCInfo.ClearData();
    }

    public override async void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
    {
    }

    public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
    {
    }
}
