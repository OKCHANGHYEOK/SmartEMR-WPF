using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class SmartEMRRCPInfoViewModel : BaseViewModel<Reception>
{
    public IQueryable<MemberUser> arrMUR_DOC { get; set; } = default!;
    public IQueryable<MemberUser> arrMUR_STF { get; set; } = default!;

    public override void Initialize()
    {
        arrMUR_DOC = SmartMVVM.Master.GetMemberUsers("DOC", true, "의사선택");
        arrMUR_STF = SmartMVVM.Master.GetMemberUsers("STF", true, "직원선택");

    }

    protected override Reception GetModel(Reception item)
    {
        item.RCP_ReceiptDate = DateTime.Now.ToString("yyyy-MM-dd");
        item.RCP_ReceiptTime = DateTime.Now.ToString("HH:mm");
        item.MUR_Idx_DOC = 0;;
        item.MUR_Idx_STF = 0;

        return item;
    }

    [RelayCommand]
    public async Task SetReception(string operation)
    {

    }
}
