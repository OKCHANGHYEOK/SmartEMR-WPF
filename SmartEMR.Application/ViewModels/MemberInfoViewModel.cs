using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartEMR.Application.Core;
using SmartEMR.Application.Services.Domain;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.ViewModels;

public partial class MemberInfoViewModel : MemberViewModel
{
    [ObservableProperty]
    private bool usableMediNo = true;
    [ObservableProperty]
    private bool usableBizNum = true;

    private bool isEditedMediNum
    {
        get
        {
            return !string.Equals(SmartMVVM.AppSession.Member?.MEM_MediNo, Model.MEM_MediNo);
        }
    }

    private bool isEditedBizNum
    {
        get
        {
            return !string.Equals(SmartMVVM.AppSession.Member?.MEM_BizNum, Model.MEM_BizNum);
        }
    }

    public MemberInfoViewModel(IMemberService memberService) : base(memberService) { }

    public MemberInfoViewModel(IMemberService memberService, Member item) : base(memberService, item) { }

    protected override Member GetModel(Member item)
    {
        SmartMVVM.ModelProperty.SetMemberData(item, SmartMVVM.AppSession.Member ?? new Member());
        return item;
    }

    public void UpdateUsableFields()
    {
        UsableBizNum = !isEditedBizNum ? true: false;
        UsableMediNo = !isEditedMediNum ? true : false;
    }

    [RelayCommand]
    private async Task CheckDuplicateMediNo()
    {
        if (string.IsNullOrWhiteSpace(Model.MEM_MediNo))
        {
            SmartUI.SetNotification("요양기관번호를 입력해주세요.", NotificationType.Warning);
            return;
        }

        if (!isEditedMediNum)
        {
            SmartUI.SetNotification("기존 요양기관번호와 동일합니다.", NotificationType.Warning);
            return;
        }

        var ret = await _memberService.GetMemberByDuplicateMediNo(Model.MEM_MediNo);
        if (ret.Item is null)
        {
            UsableMediNo = true;

            SmartUI.SetNotification("사용 가능한 요양기관번호입니다.", NotificationType.Success);
        }
        else
        {
            SmartUI.SetNotification("사용중인 요양기관번호입니다.", NotificationType.Warning);
        }
    }

    [RelayCommand]
    private async Task CheckDuplicateBizNum()
    {
        if (string.IsNullOrWhiteSpace(Model.MEM_BizNum))
        {
            SmartUI.SetNotification("사업자등록번호를 입력해주세요.", NotificationType.Warning);
            return;
        }

        if (!isEditedBizNum)
        {
            SmartUI.SetNotification("기존 사업자등록번호와 동일합니다.", NotificationType.Warning);
            return;
        }

        var ret = await _memberService.GetMemberByDuplicateBizNum(Model.MEM_BizNum);
        if (ret.Item is null)
        {
            UsableBizNum = true;

            SmartUI.SetNotification("사용 가능한 사업자등록번호입니다.", NotificationType.Success);
        }
        else
        {
            SmartUI.SetNotification("사용중인 사업자등록번호입니다.", NotificationType.Warning);
        }
    }

    [RelayCommand]
    private async Task SetMember()
    {
        if (!CanSaveData()) return;

        var item = Model.Clone();
        item.MEM_IsValid = true;

        var ret = await _memberService.SetMember(item);
        if (ret.Item is null || !ret.IsSuccess)
        {
            SmartUI.SetNotification($"회원정보 수정에 실패했습니다.\n{ret.Message}", NotificationType.Error);
            return; 
        }

        SmartMVVM.AppSession.SetMember(ret.Item);

        SmartUI.SetNotification("회원 정보 수정되었습니다.", NotificationType.Success);
        SmartUI.CloseView();
    }

    [RelayCommand]
    private void ClearData()
    {
        if (SmartUI.MsgYesNo("기관 정보를 초기화하시겠습니까?\n 기존 정보로 초기화됩니다.") is System.Windows.MessageBoxResult.Yes)
        {
            SmartMVVM.ModelProperty.SetMemberData(Model, SmartMVVM.AppSession.Member ?? new Member());
        }
    }

    private bool CanSaveData()
    {
        if (!SmartMVVM.AppSession.IsAdmin)
        {
            SmartUI.SetNotification("대표자 이외에는 기관정보를 수정할 수 없습니다.", NotificationType.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(Model.MEM_MediNo))
        {
            SmartUI.SetNotification("요양기관번호를 입력해주세요.", NotificationType.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(Model.MEM_BizNum))
        {
            SmartUI.SetNotification("사업자등록번호를 입력해주세요.", NotificationType.Warning);
            return false;
        }

        if (isEditedMediNum && !UsableMediNo)
        {
            SmartUI.SetNotification("요양기관번호 중복체크를 완료해주세요.", NotificationType.Warning);
            return false;
        }

        if (isEditedBizNum && !UsableBizNum)
        {
            SmartUI.SetNotification("사업자등록번호 중복체크를 완료해주세요.", NotificationType.Warning);
            return false;
        }

        return true;
    }
}
