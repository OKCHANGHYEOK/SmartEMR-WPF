using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRWindowModel : ObservableObject
{
    [ObservableProperty]
    private string m_WindowTitle;

    [ObservableProperty]
    private Patient m_PATItem;


    public SmartEMRWindowModel()
    {
        WindowTitle = "";
        PATItem = new Patient();

        UpdateWindowTitle();
    }

    public void UpdateWindowTitle()
    {
        var member = SmartMVVM.AppSession.Member;
        var memberUser = SmartMVVM.AppSession.MemberUser;
        var patient = PATItem;

        var loginInfoTitle = $"{member?.MEM_Name} {memberUser?.MUR_Name}님으로 로그인 중";
        var patinetInfoTitle = "";

        //if (PATItem.PAT_Idx > 0)
        //{
        //    PatientInfoTitle = $"{patient.PAT_Name} 님 {patient.vPAT_Info}   {patient.PAT_PhoneNum}    최초내원일 : {patient.PAT_FirstVisitDate}  최종내원일 : {patient.PAT_LastVisitDate}";
        //}
        //else
        //{
        //    PatientInfoTitle = "";
        //}

        WindowTitle = loginInfoTitle + "       " + patinetInfoTitle;
    }

    public void UpdatePatientData(Patient item)
    {
        PATItem = item;

        UpdateWindowTitle();
    }
}
