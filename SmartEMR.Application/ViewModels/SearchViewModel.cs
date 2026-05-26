using CommunityToolkit.Mvvm.Input;
using DevExpress.Spreadsheet.Formulas;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.SmartEMRDesk;
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

        await SmartUI.SendMessage("UpdateSearchItemsSource", retPAT);
    }

    [RelayCommand]
    public void Click(string action)
    {
        switch (action)
        {
            case "MoveRES":
                SmartUI.NavigateToPage<vSmartEMRRESInfo>(isPopup:true);
                break;
        }
    }
}
