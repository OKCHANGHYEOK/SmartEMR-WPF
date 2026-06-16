using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.Core;

public class Master
{
    private readonly Dictionary<string, List<object>> _arrMasterRequest = new();

    public IReadOnlyDictionary<string, ReadOnlyCollection<object>> arrMasterRequest =>
        _arrMasterRequest.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly());

    private readonly List<MemberUser> _arrMUR = new();

    public Master()
    {
    }

    public async Task Initialize()
    {
        await InitializeByDB();

        SetMasterData();
    }

    private async Task InitializeByDB()
    {
        var MURItem = new MemberUser
        {
            MEM_Idx = SmartMVVM.AppSession.Member?.MEM_Idx
        };

        var retMUR = await SmartMVVM.DataStore.GetItems<MemberUser>(eAPI.MemberUser_GetMemberUser, MURItem);
        if (retMUR == null || retMUR.Any())
        {
            SmartUI.SetNofification("직원 정보를 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        _arrMUR.AddRange(retMUR);
    }

    private void SetMasterData()
    {
        _arrMasterRequest.Clear();

        // PAT_Sex
        AddMasterRequest("PAT_Sex", new Patient { vPAT_Sex = "성별선택", PAT_Sex = "N" });
        AddMasterRequest("PAT_Sex", new Patient { vPAT_Sex = "남", PAT_Sex = "M" });
        AddMasterRequest("PAT_Sex", new Patient { vPAT_Sex = "여", PAT_Sex = "F" });

        // PAT_IsSolar
        AddMasterRequest("PAT_IsSolar", new { attrName = "양력", attrValue = "y" });
        AddMasterRequest("PAT_IsSolar", new { attrName = "음력", attrValue = "n" });

        // PAT_IsForeginer
        AddMasterRequest("PAT_IsForegin", new { attrName = "내국인", attrValue = "n" });
        AddMasterRequest("PAT_IsForegin", new { attrName = "외국인", attrValue = "y" });

        // PAT_IsAgreePersonalInfo
        AddMasterRequest("PAT_IsAgreePersonalInfo", new { attrName = "동의", attrValue = "y" });
        AddMasterRequest("PAT_IsAgreePersonalInfo", new { attrName = "동의안함", attrValue = "n" });

        // IRC_Coperation 
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "삼성화재" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "현대해상" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "DB손해보험" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "KB손해보험" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "메리츠화재" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "삼성생명" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "한화생명" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "NHI/AID", IRC_CoName = "교보생명" });

        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "TAI", IRC_CoName = "삼성화재" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "TAI", IRC_CoName = "현대해상" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "TAI", IRC_CoName = "DB손해보험" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "TAI", IRC_CoName = "KB손해보험" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "TAI", IRC_CoName = "메리츠화재" });
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "TAI", IRC_CoName = "AXA손해보험" });
        
        AddMasterRequest("IRC_Coperation", new Insurance { IRC_Type = "WCI", IRC_CoName = "근로복지공단" });

    }

    public IQueryable<MemberUser> GetMemberUsers(string MUR_JobCode = "", bool isDefault = false, string defaultText = "전체")
    {
        var arrMUR = new List<MemberUser>();

        if (isDefault)
        {
            arrMUR.Add(new MemberUser { MUR_Idx = 0, MUR_Name = defaultText });
        }

        var targetItems = _arrMUR.Where(x => x.MUR_JobCode == MUR_JobCode);
        if (targetItems.Any())
        {
            arrMUR.AddRange(targetItems);
        } 

        return arrMUR.AsQueryable();
    }

    public IQueryable<object> Query(string name)
    {
        if (arrMasterRequest.TryGetValue(name, out var list))
        {
            return list.AsQueryable();
        }

        return default!;
    }

    public IQueryable<T> Query<T>(string name) where T : class
    {
        return Query(name).Cast<T>();
    }

    private void AddMasterRequest(string name, object value)
    {
        if (!_arrMasterRequest.TryGetValue(name, out var list))
        {
            _arrMasterRequest.Add(name, new List<object>() { value });
        }

        list?.Add(value);
    }
}
