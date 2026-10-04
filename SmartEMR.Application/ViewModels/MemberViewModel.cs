using CommunityToolkit.Mvvm.Input;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class MemberViewModel : BaseViewModel<Member>
{
    public MemberUser MemberUser { get; set; } = new();

    public string passwordCheckString { get; set; } = "";

    private bool isUseableId { get; set; } = false;

    public override void Initialize()
    {
    }

    protected override Member GetModel(Member item)
    {
        return item;
    }

    [RelayCommand]
    private async Task CheckDuplicate()
    {

    }
}
