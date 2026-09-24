using Microsoft.Web.WebView2.Core;
using SmartEMR.Domain.Entities;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRPay
{
    /// <summary>
    /// vSmartEMRNaverPayInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRNaverPayInfo : Window
    {
        private readonly Pay _model;

        public vSmartEMRNaverPayInfo(Pay item)
        {
            InitializeComponent();

            _model = item;
        }

        private async void OnLoaded_NaverPayWebView(
            object sender,
            RoutedEventArgs e)
        {
            if (NaverPayWebView is null) return;

            await NaverPayWebView.EnsureCoreWebView2Async();

            var resourcePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Resources",
                "NaverPay");

            var webView = NaverPayWebView.CoreWebView2;

            webView.SetVirtualHostNameToFolderMapping(
                "naverpay.local",
                resourcePath,
                CoreWebView2HostResourceAccessKind.Allow);

            // HTML 로딩 완료 후 Pay 정보 전달
            webView.NavigationCompleted += OnNavigationCompleted;

            NaverPayWebView.Source =
                new Uri("https://naverpay.local/naverpay.html");
        }

        private void OnNavigationCompleted(
            object? sender,
            CoreWebView2NavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess) return;

            var payPrice = _model.PAY_PriceForPay ?? 0;

            var data = new
            {
                merchantPayKey = CreateMerchantPayKey(),
                productName = $"{_model.PAT_Name ?? "님"} 진료비",
                productCount = 1,

                totalPayAmount = payPrice,
                taxScopeAmount = payPrice,
                taxExScopeAmount = 0,

                returnUrl =
                    "https://developers.pay.naver.com/user/sand-box/payment"
            };

            var json = JsonSerializer.Serialize(data);

            NaverPayWebView.CoreWebView2.PostWebMessageAsJson(json);
        }

        private string CreateMerchantPayKey()
        {
            if (_model.PAY_Idx.HasValue)
            {
                return $"PAY_{_model.PAY_Idx}";
            }

            return $"PAY_{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}