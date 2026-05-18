using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows.Controls;
using System.Windows.Data;

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
    }
}
