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
            SmartUI.SetNotification("직원 정보를 불러오지 못했습니다.", NotificationType.Error);
            return;
        }

        _arrMUR.AddRange(retMUR);
    }

    private void SetMasterData()
    {
        _masterItems.Clear();

        var reference = ReferenceDataLoader.Load();

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
