using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Collections.ObjectModel;

namespace SmartEMR.Application.Views.Shared
{
    /// <summary>
    /// vSmartEMRNotificationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRNotificationView : CustomControl
    {
        public ObservableCollection<NotiItem> NotiItems = new();

        public vSmartEMRNotificationView()
        {
        }
    }
}
