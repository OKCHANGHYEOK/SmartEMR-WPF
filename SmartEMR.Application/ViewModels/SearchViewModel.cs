using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;

namespace SmartEMR.Application.ViewModels;

public partial class SearchViewModel : BaseViewModel<Patient>
{
    public override void Initialize()
    {
    }

    protected override Patient GetModel(Patient item)
    {
        return item;
    }

    [RelayCommand]
    public async Task Search()
    {
        string? keyword = Model.Keyword;

        if (string.IsNullOrWhiteSpace(keyword))
        {
            SmartUI.SetNofification("검색어를 1글자 이상 입력해주세요", NotificationType.Warning);

            await SmartUI.SendMessageToSearchView("SetFocusToSearchText");

            return;
        }

        var getItem = new Patient
        {
            PageSize = 10,
            PAT_Name = new string(keyword.Where(char.IsLetter).ToArray()),
            PAT_ChartNo = new string(keyword.Where(char.IsDigit).ToArray())
        };

        var retPAT = await SmartMVVM.DataStore.GetItems<Patient>(eAPI.Patient_GetPatient, getItem);
        if (retPAT == null || !retPAT.Any() || SmartMVVM.DataStore.retIsSuccess == false)
        {
            SmartUI.SetNofification("조건에 해당하는 검색결과가 없습니다.", NotificationType.Warning);
            return;
        }

        foreach (Patient item in retPAT)
        {
            item.vPAT_Sex = item.PAT_Sex == "M" ? "남" : "여";
            item.vPAT_Info = item.vPAT_Sex + "/" + item.PAT_Age + "세";
            item.PAT_PhoneNum = item.PAT_Hpp1 + item.PAT_Hpp2 + item.PAT_Hpp3;
            item.vPAT_Address = string.IsNullOrWhiteSpace(item.PAT_Address1) ? "주소지미입력" : item.PAT_Address1;
            item.PAT_Bigo = string.IsNullOrWhiteSpace(item.PAT_Bigo) ? "비고없음" : item.PAT_Bigo;
        } 

        await SmartUI.SendMessage("UpdateSearchItemsSource", retPAT);
    }
}
