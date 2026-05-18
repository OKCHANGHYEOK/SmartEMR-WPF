using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SmartEMR.Application.Views.Shared
{
    /// <summary>
    /// vSmartEMRNotificationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRNotificationView : CustomControl
    {
        public vSmartEMRNotificationView()
        {
            NotiItemsControl.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("NotiItems") { Source = NotificationService.Instance });
        }

        public void OnMouseLeftButtonDown_NotiItem(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is NotiItem notiItem)
            {
                SmartUI.CloseNotification(notiItem);
            }
        }
    }
}
