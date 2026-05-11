using MahApps.Metro.Controls;
using SmartEMR.Infrastructure;
using System.Windows;
using System.Windows.Threading;

namespace SmartEMR.Application.Core;

public class SmartMVVM
{
    private SmartMVVM() { }

    private static readonly SmartMVVM _Instance = new();

    public static readonly ApplicationSession AppSession = new();
    public static readonly DataStore DataStore = new(AppSession);
    public static readonly Common.Common Common = new();

    private static RoutedEventArgs? _lastProcessedEventArgs = null;

    // 한 번에 단 하나의 진입만 허용하는 세마포어
    private static readonly SemaphoreSlim _clickSemaphore = new SemaphoreSlim(1,1);

    /// <summary>
    /// 클릭 이벤트 방지를 위한 로직
    /// </summary>
    public static async Task<bool> PreventClickFiring(RoutedEventArgs e)
    {
        // 1. 동일한 이벤트 인자가 즉시 또 들어오면 차단
        if (_lastProcessedEventArgs == e) return true;

        // 2. 세마포어 확인 (0초 대기)
        bool entered = await _clickSemaphore.WaitAsync(0);

        if (entered)
        {
            _lastProcessedEventArgs = e; // 성공 시에만 마지막 인자 기록
            return false; // 진행 가능
        }

        return true; // 진행 불가 (이미 실행 중)
    }

    /// <summary>
    /// 클릭 이벤트 작업 종료시 락을 해제합니다.
    /// </summary>

    public static void ReleaseClick()
    {
        _lastProcessedEventArgs = null;

        if (_clickSemaphore.CurrentCount == 0)
        {
            _clickSemaphore.Release();
        }
    }

    public static void BeginInvoke(Action action, DispatcherPriority priority)
    {
        var currentWindow = SmartUI.CurrentWindow as Window ?? App.Current.MainWindow;

        if (currentWindow != null)
        {
            currentWindow.Dispatcher?.BeginInvoke(action, priority);
        }
    } 
}
