using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;

namespace SmartEMR.Application.Core;

public class NotiItem : INotifyPropertyChanged
{
    public string? NotiMessage { get; set; }
    public Brush? NotiColor { get; set; }
    public ImageSource? NotiImage { get; set; }

    private bool _IsClosing;
    public bool IsClosing
    {
        get => _IsClosing;
        set
        {
            if (_IsClosing != value)
            {
                _IsClosing = value;
                OnPropertyChanged(nameof(IsClosing));
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class NotificationService
{
    private static Lazy<NotificationService> _instance = new(() => new NotificationService(new ObservableCollection<NotiItem>()));

    public static NotificationService Instance => _instance.Value;

    private readonly ObservableCollection<NotiItem> _Items;
    public ObservableCollection<NotiItem> NotiItems => _Items;

    private readonly Queue<NotiItem> _waitingQueue = new();
    private const int MAX_VISIBLE_COUNT = 10;
    private readonly object _lockObj = new();

    public NotificationService(ObservableCollection<NotiItem> items)
    {
        _Items = items;
    }

    public async void SetNotification(NotiItem item)
    {
        lock (_lockObj)
        {
            _waitingQueue.Enqueue(item);
        }

        ProcessNotificationQueue();
    }

    private async void ProcessNotificationQueue()
    {
        NotiItem? targetItem = null;

        lock (_lockObj)
        {
            if (_Items.Count < MAX_VISIBLE_COUNT && _waitingQueue.Count > 0)
            {
                targetItem = _waitingQueue.Dequeue();
            }
        }

        if (targetItem == null) return;

        _Items.Add(targetItem);

        await Task.Delay(5000);

        targetItem.IsClosing = true;

        await Task.Delay(1000);

        _Items.Remove(targetItem);

        ProcessNotificationQueue();
    }

    public async void CloseNotification(NotiItem item)
    {
        // 이미 닫히는 중이거나 리스트에 없는 경우 예외 처리
        if (item == null || item.IsClosing || !_Items.Contains(item))
            return;

        // 1. 즉시 닫힘 애니메이션 트리거 (XAML DataTrigger가 반응함)
        item.IsClosing = true;

        // 2. 애니메이션 Duration(0.5초)만큼 대기
        await Task.Delay(500);

        // 3. 컬렉션에서 안전하게 제거
        lock (_lockObj)
        {
            if (_Items.Contains(item))
            {
                _Items.Remove(item);
            }
        }

        // 4. 대기열(Queue)에 다음 알림이 있다면 하나 꺼내서 출력
        ProcessNotificationQueue();
    }
}
