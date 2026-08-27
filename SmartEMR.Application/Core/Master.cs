using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.Core;

public class Master
{
    private readonly Dictionary<string, List<object>> _masterItems = new();

    public IReadOnlyDictionary<string, ReadOnlyCollection<object>> masterItems =>
        _masterItems.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly());

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
            MEM_Idx = SmartMVVM.AppSession.Member?.MEM_Idx,
            MUR_Role = "USR"
        };

        var retMUR = await SmartMVVM.DataStore.GetItems<MemberUser>(eAPI.MemberUser_GetMemberUser, MURItem);
        if (retMUR == null || !retMUR.Any())
        {
            SmartUI.SetNofification("직원 정보를 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        _arrMUR.AddRange(retMUR);
    }

    private void SetMasterData()
    {
        _masterItems.Clear();

        // PAT_Sex
        AddMasterItem("PAT_Sex", new Patient { vPAT_Sex = "성별선택", PAT_Sex = "N" });
        AddMasterItem("PAT_Sex", new Patient { vPAT_Sex = "남", PAT_Sex = "M" });
        AddMasterItem("PAT_Sex", new Patient { vPAT_Sex = "여", PAT_Sex = "F" });

        // PAT_IsSolar
        AddMasterItem("PAT_IsSolar", new { attrName = "양력", attrValue = "y" });
        AddMasterItem("PAT_IsSolar", new { attrName = "음력", attrValue = "n" });

        // PAT_IsForeginer
        AddMasterItem("PAT_IsForegin", new { attrName = "내국인", attrValue = "n" });
        AddMasterItem("PAT_IsForegin", new { attrName = "외국인", attrValue = "y" });

        // PAT_IsAgreePersonalInfo
        AddMasterItem("PAT_IsAgreePersonalInfo", new { attrName = "동의", attrValue = "y" });
        AddMasterItem("PAT_IsAgreePersonalInfo", new { attrName = "동의안함", attrValue = "n" });

        // IRC_CoName 
        foreach (string type in new[] { "GUN", "MED" }) 
        {
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "SSH", IRC_CoName = "삼성화재" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "HDH", IRC_CoName = "현대해상" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "DBS", IRC_CoName = "DB손해보험" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "KBS", IRC_CoName = "KB손해보험" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "MRT", IRC_CoName = "메리츠화재" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "SSS", IRC_CoName = "삼성생명" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "HAS", IRC_CoName = "한화생명" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "KYB", IRC_CoName = "교보생명" });
            AddMasterItem("IRC_CoName", new Insurance { IRC_Type = type, IRC_CoCd = "ETC", IRC_CoName = "기타" });
        }

        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "SSH", IRC_CoName = "삼성화재" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "HDH", IRC_CoName = "현대해상" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "DBS", IRC_CoName = "DB손해보험" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "KBS", IRC_CoName = "KB손해보험" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "MRT", IRC_CoName = "메리츠화재" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "AXA", IRC_CoName = "AXA손해보험" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "CAR", IRC_CoCd = "ETC", IRC_CoName = "기타" });

        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "SAN", IRC_CoCd = "GUN", IRC_CoName = "근로복지공단" });
        AddMasterItem("IRC_CoName", new Insurance { IRC_Type = "SAN", IRC_CoCd = "ETC", IRC_CoName = "기타" });

        AddMasterItem("ORDC_Cd", new Order { ORDC_Cd = "PRC", vORDC_Cd = "시술" });
        AddMasterItem("ORDC_Cd", new Order { ORDC_Cd = "TRT", vORDC_Cd = "처치" });
        AddMasterItem("ORDC_Cd", new Order { ORDC_Cd = "EXM", vORDC_Cd = "검사" });
        AddMasterItem("ORDC_Cd", new Order { ORDC_Cd = "DOC", vORDC_Cd = "문서" });
        AddMasterItem("ORDC_Cd", new Order { ORDC_Cd = "MED", vORDC_Cd = "투약" });
        AddMasterItem("ORDC_Cd", new Order { ORDC_Cd = "ETC", vORDC_Cd = "기타" });
    }

    public List<MemberUser> GetMemberUsers(string MUR_JobCode = "", bool isDefault = false, string defaultText = "전체")
    {
        var arrMUR = new List<MemberUser>();

        if (isDefault)
        {
            arrMUR.Add(new MemberUser { MUR_Idx = 0, MUR_Name = defaultText });
        }

        var targetItems = _arrMUR.Where(x => x.MUR_JobCode == MUR_JobCode).AsQueryable();
        if (targetItems.Any())
        {
            arrMUR.AddRange(targetItems);
        } 

        return arrMUR;
    }

    public IQueryable<object> Query(string name)
    {
        if (masterItems.TryGetValue(name, out var list))
        {
            return list.AsQueryable();
        }

        return default!;
    }

    public IQueryable<T> Query<T>(string name) where T : class
    {
        return Query(name).Cast<T>();
    }

    private void AddMasterItem(string name, object value)
    {
        if (!_masterItems.TryGetValue(name, out var list))
        {
            _masterItems.Add(name, new List<object>() { value });
        }

        list?.Add(value);
    }
}
