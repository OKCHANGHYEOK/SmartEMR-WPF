using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.ViewModels;
using SmartEMR.Application.Xpf;

namespace SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab
{
    /// <summary>
    /// vSmartEMRRESCalendarTab.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRRESCalendarTab : ModelViewLayout<ReservationViewModel>
    {
        public vSmartEMRRESCalendarTab() { }

        protected override void Initialize()
        {
        }

        public override void OnBindGrid_BindClick(object? sender, BindClickEventArgs e)
        {
        }

        public override void OnBindGrid_BindItemChanged(object? sender, BindItemChangedEventArgs e)
        {
        }

        public override async Task<ViewMessageResponse?> ReceiveMessage(ViewMessageRequest request)
        {
            var response = new ViewMessageResponse { IsSuccess = true };

            switch (request.MessageAction)
            {
                case "UpdateCalendar":
                    await SmartEMRCalendar.UpdateCalendar();
                    break;
            }

            return response;
        }
    }
}