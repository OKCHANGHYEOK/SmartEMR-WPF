using System.IO;
using System.Text.Json;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Core;

/// <summary>
/// ReferenceData.json 항목 중 이름/값 쌍으로 구성된 항목입니다.
/// ValueMember의 선언 타입을 실제 모델 값과 일치시켜 DevExpress가 불필요한 문자열 변환을 하지 않도록 합니다.
/// </summary>
public class AttrItem<T>
{
    public string? attrName { get; set; }
    public T attrValue { get; set; } = default!;
}

/// <summary>
/// 앱 시작 시 Master가 로드하는 정적 참조데이터 (ReferenceData.json)
/// </summary>
public class MasterReferenceData
{
    public List<Patient> PAT_Sex { get; set; } = new();
    public List<AttrItem<string>> PAT_IsSolar { get; set; } = new();
    public List<AttrItem<string>> PAT_IsForegin { get; set; } = new();
    public List<AttrItem<string>> PAT_IsAgreePersonalInfo { get; set; } = new();
    public List<Insurance> IRC_CoName { get; set; } = new();
    public List<Order> ORDC_Cd { get; set; } = new();
    public List<AttrItem<int>> PAY_CutUnit { get; set; } = new();
}

public static class ReferenceDataLoader
{
    public static MasterReferenceData Load()
    {
        try
        {
            var dataPath = Path.Combine(AppContext.BaseDirectory, "referenceData.json");

            if (File.Exists(dataPath))
            {
                var json = File.ReadAllText(dataPath);

                var data = JsonSerializer.Deserialize<MasterReferenceData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data != null) return data;
            }
        }
        catch (Exception)
        {
            SmartUI.SetNotification("참조데이터 파일을 읽는 중 오류가 발생했습니다.", NotificationType.Error);
        }

        return new MasterReferenceData();
    }
}
