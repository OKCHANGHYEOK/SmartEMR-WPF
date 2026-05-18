using System.Collections.ObjectModel;
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

    public NotificationService(ObservableCollection<NotiItem> items)
    {
        _Items = items;
    }

    public async void SetNotification(NotiItem item)
    {
        _Items.Add(item);

        await Task.Delay(5000);

        item.IsClosing = true;

        await Task.Delay(1000);

        _Items.Remove(item);
    }
}
