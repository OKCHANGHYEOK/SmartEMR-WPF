using SmartEMR.Infrastructure;

namespace SmartEMR.Application.Core;

public class SmartMVVM
{
    private static readonly SmartMVVM _Instance = new();

    public static readonly ApplicationSession AppSession = new();
    public static readonly DataStore DataStore = new(AppSession);
    public static readonly Common.Common Common = new();

    private SmartMVVM() { }
}
