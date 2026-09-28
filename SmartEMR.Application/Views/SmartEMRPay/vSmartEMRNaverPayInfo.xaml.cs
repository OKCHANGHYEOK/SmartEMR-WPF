using Microsoft.Web.WebView2.Core;
using SmartEMR.Application.Core.NaverPay;
using SmartEMR.Application.Schemas;
using SmartEMR.Domain.Entities;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Web;
using System.Windows;

namespace SmartEMR.Application.Views.SmartEMRPay
{
    /// <summary>
    /// vSmartEMRNaverPayInfo.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRNaverPayInfo : Window
    {
        internal NaverPayResponse? response = null;

        private readonly Pay _model;

        private const string requestHost = "naverpay.local";
        private const string returnHost = "developers.pay.naver.com";

        public vSmartEMRNaverPayInfo(Pay item)
        {
            InitializeComponent();

            _model = item;
        }

        private async void OnLoaded_WebView(object sender, RoutedEventArgs e)
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

            NaverPayWebView.Source = new Uri("https://naverpay.local/naverpay.html");
        }

        private void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            if (sender is not CoreWebView2 webView || !e.IsSuccess) return;

            if (!Uri.TryCreate(webView.Source, UriKind.Absolute, out var currentUri)) return;

            if (currentUri.Host == requestHost)
            {
                RequestPayment();
            }
            else if (currentUri.Host == returnHost)
            {
                var response = HandlePaymentResult(currentUri);
                if (response.IsSuccess)
                {
                    this.response = response;
                }

                this.Close();
            }
        }

        private void RequestPayment()
        {
            var payPrice = _model.PAY_PriceForPay ?? 0;
            var request = new NaverPayRequest
            {
                merchantPayKey = NaverPayManager.CreateMerchantPayKey(_model.PAT_Idx),
                productName = $"{_model.PAT_Name}님 진료비",
                productCount = 1,

                totalPayAmount = payPrice,
                taxScopeAmount = payPrice,
                taxExScopeAmount = 0,

                returnUrl = "https://developers.pay.naver.com/user/sand-box/payment"
            };

            var json = JsonSerializer.Serialize(request);

            NaverPayWebView.CoreWebView2.PostWebMessageAsJson(json);
        }

        private NaverPayResponse HandlePaymentResult(Uri uri)
        {
            var response = new NaverPayResponse();

            if (uri.Query is not string query || string.IsNullOrWhiteSpace(query))
            {
                response.Message = "요청주소가 올바르지 않습니다.";
                return response;
            }
            
            try
            {
                var parameters = HttpUtility.ParseQueryString(uri.Query);

                if (!Enum.TryParse<NaverPayResultCode>(parameters["resultCode"], out var resultCode))
                {
                    response.Message = "응답을 해석하는데 실패했습니다.";
                    return response;
                }

                if (resultCode != NaverPayResultCode.Success)
                {
                    response.Message = parameters["resultMessage"];
                    return response;
                }

                response.Item.paymentId = parameters["paymentId"];
                response.IsSuccess = true;

                return response;

            }
            catch (ArgumentNullException)
            {
                response.Message = "내부오류가 발생했습니다.";
            }

            return response;
        }
    }
}