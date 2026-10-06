using SmartEMR.Domain.Entities;
using SmartEMR.Domain.Enums;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.Core;

public class Master
{
    public const string MUR_DEPARTMENT_ADM = "ADM";
    public const string MUR_DEPARTMENT_MED = "MED";

    private readonly Dictionary<string, List<object>> _masterItems = new();

    public IReadOnlyDictionary<string, ReadOnlyCollection<object>> masterItems =>
        _masterItems.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.AsReadOnly());

    private List<Member> _members { get; set; } = new();
    private List<MemberUser> _memberUsers { get; set; } = new();

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
       await SetMembers();
       await SetMemberUsers();
    }

    public async Task SetMembers()
    {
        if (SmartMVVM.AppSession.Member?.MEM_Idx.GetValueOrDefault(0) > 0) return;

        var item = new Member
        {

        };

        var retMEM = await SmartMVVM.DataStore.GetItems<Member>(eAPI.Member_GetMember, item);
        if (retMEM != null && retMEM.Any())
        {
            _members = [.. retMEM];
        }    
    }

    private async Task SetMemberUsers()
    {
        if (SmartMVVM.AppSession.Member?.MEM_Idx.GetValueOrDefault(0) == 0) return;

        var item = new MemberUser
        {
            MEM_Idx = SmartMVVM.AppSession.Member?.MEM_Idx,
            MUR_Role = "USR"
        };

        var retMUR = await SmartMVVM.DataStore.GetItems<MemberUser>(eAPI.MemberUser_GetMemberUser, item);
        if (retMUR != null && retMUR.Any())
        {
            _memberUsers = [.. retMUR];
        }
    }

    private void SetMasterData()
    {
        _masterItems.Clear();

        var reference = ReferenceDataLoader.Load();

        // MEM_BizType 
        foreach (var item in reference.MEM_BizType) AddMasterItem("MEM_BizType", item); 

        // MUR_Deparment
        foreach (var item in reference.MUR_Department) AddMasterItem("MUR_Department", item);

        // MUR_JobCode
        foreach (var item in reference.MUR_JobCode) AddMasterItem("MUR_JobCode", item);

        // MUR_EmailDomain
        foreach (var item in reference.MUR_EmailDomain) AddMasterItem("MUR_EmailDomain", item);

        // PAT_Sex
        foreach (var item in reference.PAT_Sex) AddMasterItem("PAT_Sex", item);

        // PAT_IsSolar
        foreach (var item in reference.PAT_IsSolar) AddMasterItem("PAT_IsSolar", item);

        // PAT_IsForegin
        foreach (var item in reference.PAT_IsForegin) AddMasterItem("PAT_IsForegin", item);

        // PAT_IsAgreePersonalInfo
        foreach (var item in reference.PAT_IsAgreePersonalInfo) AddMasterItem("PAT_IsAgreePersonalInfo", item);

        // IRC_CoName
        foreach (var item in reference.IRC_CoName) AddMasterItem("IRC_CoName", item);

        // ORDC_Cd
        foreach (var item in reference.ORDC_Cd) AddMasterItem("ORDC_Cd", item);

        // PAY_CutUnit
        foreach (var item in reference.PAY_CutUnit) AddMasterItem("PAY_CutUnit", item);
    }

    public List<Member> GetMembers(string MEM_BizType, string keyword = "", bool isDefault = false, string defaultText = "전체")
    {
        var members = new List<Member>();

        if (isDefault)
        {
            members.Add(new Member { MEM_Idx = 0, MEM_Name = defaultText });
        }

        var targetItems = _members.Where(x => x.MEM_BizType == MEM_BizType);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            targetItems = targetItems.Where(x => !string.IsNullOrWhiteSpace(x.MEM_Name) && x.MEM_Name.Contains(keyword));
        }

        return [.. targetItems];
    }

    public List<MemberUser> GetMemberUsers(string MUR_JobCode = "", bool isDefault = false, string defaultText = "전체")
    {
        var arrMUR = new List<MemberUser>();

        if (isDefault)
        {
            arrMUR.Add(new MemberUser { MUR_Idx = 0, MUR_Name = defaultText });
        }

        var targetItems = _memberUsers.Where(x => x.MUR_JobCode == MUR_JobCode).AsQueryable();
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
