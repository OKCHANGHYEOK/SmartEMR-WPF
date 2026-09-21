using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartEMR.Domain.Entities;

namespace SmartEMR.Application.Core;

/// <summary>
/// ReferenceData.json 항목 중 이름/값 쌍으로 구성된 항목 (DisplayMember="attrName", ValueMember="attrValue" 바인딩용)
/// </summary>
public class AttrItem
{
    public string? attrName { get; set; }

    [JsonConverter(typeof(AttrItemValueConverter))]
    public object? attrValue { get; set; }
}

public class AttrItemValueConverter : JsonConverter<object?>
{
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                return reader.TryGetInt32(out int intValue) ? intValue : reader.GetDouble();

            case JsonTokenType.String:
                return reader.GetString();

            case JsonTokenType.True:
                return true;

            case JsonTokenType.False:
                return false;

            default:
                return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, value?.GetType() ?? typeof(object), options);
    }
}

/// <summary>
/// 앱 시작 시 Master가 로드하는 정적 참조데이터 (ReferenceData.json)
/// </summary>
public class MasterReferenceData
{
    public List<Patient> PAT_Sex { get; set; } = new();
    public List<AttrItem> PAT_IsSolar { get; set; } = new();
    public List<AttrItem> PAT_IsForegin { get; set; } = new();
    public List<AttrItem> PAT_IsAgreePersonalInfo { get; set; } = new();
    public List<Insurance> IRC_CoName { get; set; } = new();
    public List<Order> ORDC_Cd { get; set; } = new();
    public List<AttrItem> PAY_CutUnit { get; set; } = new();
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