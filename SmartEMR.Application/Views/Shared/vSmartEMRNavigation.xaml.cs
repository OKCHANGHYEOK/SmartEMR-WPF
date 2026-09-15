using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views.SmartEMRCST;
using SmartEMR.Application.Views.SmartEMRPay;
using SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.Shared
{
    /// <summary>
    /// vSmartEMRNavigation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRNavigation : CustomControl
    {
        public vSmartEMRNavigation()
        {
        }

        private async void OnNavigationBar_BarItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var element = sender as Button;

            if (element != null)
            {
                var bFlag = Enum.TryParse<eSmartEMRLocation>(element.Tag.ToString(), out var location);

                if (bFlag)
                {
                    switch (location)
                    {
                        case eSmartEMRLocation.RES:
                            await SmartUI.NavigateToPage(new vSmartEMRRESCalendarTab());
                            break;

                        case eSmartEMRLocation.DSK:
                            await SmartUI.NavigateToPage(new vSmartEMRDeskTab());
                            break;

                        case eSmartEMRLocation.CST:
                            await SmartUI.NavigateToPage(new vSmartEMRConsultationTab());
                            break;

                        case eSmartEMRLocation.PAY:
                            await SmartUI.NavigateToPage(new vSmartEMRPayTab());
                            break;

                        case eSmartEMRLocation.CRM:
                            break;

                        case eSmartEMRLocation.CONFIG:
                            break;
                    }
                }
            }
        }
    }
}
