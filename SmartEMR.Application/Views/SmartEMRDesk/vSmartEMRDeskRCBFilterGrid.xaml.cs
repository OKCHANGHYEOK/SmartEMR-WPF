using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Views.SmartEMRDesk
{
    /// <summary>
    /// vSmartEMRDeskRCBFilterGrid.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRDeskRCBFilterGrid : UserControl
    {
        public vSmartEMRDeskRCBFilterGrid()
        {
            InitializeComponent();
        }

        private async void OnClick_ImageButton(object sender, RoutedEventArgs e)
        {
            var element = sender as ImageButton;
            if (element == null) return;

            switch (element.Name)
            {
                case "btnSearch":
                    await SmartUI.SendMessage("SearchData"); 
                    break;

                case "btnClearFilter":
                    await SmartUI.SendMessage("ClearFilter");
                    break;
            }
        }
    }
}
