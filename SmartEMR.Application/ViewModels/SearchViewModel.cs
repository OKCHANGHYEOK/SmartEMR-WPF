using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;
using System.Diagnostics;

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
    public async Task Search(string? keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
        {
            SmartUI.SetNofification("검색어를 1글자 이상 입력해주세요", NotificationType.Warning);
            
            await SmartUI.SendMessage("SetFocusToSearchText", viewType:TargetViewType.RootView);

            return;
        }
    }
}
