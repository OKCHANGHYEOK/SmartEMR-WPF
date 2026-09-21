using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartEMR.Application.Core;

public static class AppConfig
{
    private static readonly Lazy<AppSettings> _settings = new(Load);

    public static AppSettings Settings => _settings.Value;

    private static AppSettings Load()
    {
        try
        {
            var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);

                var settings = JsonSerializer.Deserialize<AppSettings>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });

                if (settings != null) return settings;
            }
        }
        catch (Exception)
        {
            SmartUI.SetNotification("설정 파일을 읽는 중 오류가 발생했습니다. 기본값을 사용합니다.", NotificationType.Warning);
        }

        return new AppSettings();
    }
}

public class AppSettings
{
    public ApiSettings Api { get; set; } = new();
    public DevSettings Development { get; set; } = new();
}

public class ApiSettings
{
    public string BaseUrl { get; set; } = "http://127.0.0.1:8000/";
    public int RequestTimeoutSeconds { get; set; } = 180;
}

public class DevSettings
{
    public int AutoLoginUserIdx { get; set; } = 100000;
}