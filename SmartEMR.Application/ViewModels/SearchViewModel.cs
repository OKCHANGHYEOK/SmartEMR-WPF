using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

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
        SmartUI.SetNofification("메세지 출력 테스트", NotificationType.Info);
    }


}
