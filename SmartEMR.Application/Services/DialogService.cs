using SmartEMR.Application.Core;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects; // DropShadowEffect 사용을 위해 필수 추가!

namespace SmartEMR.Application.Services;

public class DialogService
{
    private UIWindow? _window
    {
        get
        {
            return SmartUI.CurrentWindow ?? default;
        }
    }

    public MessageBoxResult MsgConfirm(string msg)
    {
        var msgBox = new SmartMessageBox();
        msgBox.SetMessage(msg);
        msgBox.SetButtonVisibility(MessageBoxType.OK);

        if (_window != null)
        {
            msgBox.Owner = _window;
            msgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        msgBox.ShowDialog();
        return msgBox.Result;
    }

    public MessageBoxResult MsgYesNo(string msg)
    {
        var msgBox = new SmartMessageBox();
        msgBox.SetMessage(msg);
        msgBox.SetButtonVisibility(MessageBoxType.YesNo);

        if (_window != null)
        {
            msgBox.Owner = _window;
            msgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        msgBox.ShowDialog();
        return msgBox.Result;
    }

    public MessageBoxResult MsgYesNo(List<Run> arrRun)
    {
        var msgBox = new SmartMessageBox();
        msgBox.SetMessage(arrRun);
        msgBox.SetButtonVisibility(MessageBoxType.YesNo);

        if (_window != null)
        {
            msgBox.Owner = _window;
            msgBox.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        msgBox.ShowDialog();
        return msgBox.Result;
    }

    private class SmartMessageBox : Window
    {
        public MessageBoxResult Result { get; private set; } = MessageBoxResult.None;

        // 섀도우 렌더링 영역 확보용 최상위 Grid
        private Grid ShadowContainerGrid = new();

        // 실제 메시지박스의 모양과 그림자를 담당할 마스터 보더
        private Border OuterBorder = new();
        private StyleGrid Grid = new();
        private ContentControl TextContentControl = new();
        private Xpf.TextBlock txtContent = new();
        private StackPanel ButtonPanel = new();

        private MessageBoxButton btnOK = new();
        private MessageBoxButton btnYes = new();
        private MessageBoxButton btnNo = new();

        public SmartMessageBox()
        {
            // 1. 윈도우 스타일 초기화 및 투명화 설정
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            this.AllowsTransparency = true;
            this.Background = Brushes.Transparent;

            // 크기는 요청하신 360x300 비율을 기본으로 유지
            this.Width = 375;  // 그림자 마진 여유 공간을 위해 15px 정도 늘림
            this.Height = 315;

            // 최상위 컨테이너로 그림자 마진용 Grid 지정
            this.Content = ShadowContainerGrid;

            // 2. 부드럽고 고급스러운 그림자 효과(Drop Shadow) 적용
            OuterBorder.Background = Brushes.White;
            OuterBorder.BorderThickness = new Thickness(0); // 바깥쪽 선(보더) 완전 제거
            OuterBorder.CornerRadius = new CornerRadius(8); // 모서리를 더 부드럽고 이쁘게 라운딩 (8px)
            OuterBorder.Margin = new Thickness(10);        // 그림자가 잘리지 않고 은은하게 퍼질 여백 확보

            var dropShadow = new DropShadowEffect
            {
                Color = Color.FromRgb(0, 0, 0),
                BlurRadius = 12,      // 그림자 번짐 정도 (부드럽게 설정)
                Direction = 270,      // 아래 방향으로 그림자 투사
                ShadowDepth = 3,       // 그림자 깊이
                Opacity = 0.18         // 너무 어둡지 않고 은은하게 처리
            };
            OuterBorder.Effect = dropShadow;

            // 그림자 보더를 컨테이너에 배치하고 내부에 메인 Grid 장착
            ShadowContainerGrid.Children.Add(OuterBorder);
            OuterBorder.Child = Grid;

            // 3. 내부 레이아웃 분할
            Grid.SetLayout(1, 2);
            Grid.LayoutRoot.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
            Grid.LayoutRoot.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Auto);
            Grid.AddElement(TextContentControl, 0, 0);
            Grid.AddElement(ButtonPanel, 0, 1);

            // 4. 상단 텍스트 영역 스타일 수정 (트렌디한 파스텔 그라데이션)
            StyleGrid TextGrid = new StyleGrid();

            // SmartEMR의 신뢰감 있는 블루톤과 가독성을 모두 챙긴 리니어 그라데이션 브러시 조합
            LinearGradientBrush textBgGradient = new LinearGradientBrush();
            textBgGradient.StartPoint = new Point(0, 0);
            textBgGradient.EndPoint = new Point(1, 1);
            textBgGradient.GradientStops.Add(new GradientStop(Color.FromRgb(227, 242, 253), 0.0)); // 매우 화사하고 맑은 스카이블루
            textBgGradient.GradientStops.Add(new GradientStop(Color.FromRgb(240, 244, 248), 1.0)); // 소프트한 그레이시 블루

            TextGrid.Background = textBgGradient;
            TextGrid.SetLayout(1, 1);
            TextGrid.AddElement(txtContent, 0, 0);

            TextContentControl.Content = TextGrid;
            TextContentControl.VerticalContentAlignment = VerticalAlignment.Center;
            TextContentControl.HorizontalContentAlignment = HorizontalAlignment.Center;
            TextContentControl.Padding = new Thickness(25, 35, 25, 30);

            // 텍스트 가독성 최적화
            txtContent.MaxWidth = 280;
            txtContent.FontSize = 15;
            txtContent.FontWeight = FontWeights.DemiBold;
            txtContent.Foreground = new SolidColorBrush(Color.FromRgb(43, 58, 66)); // 본문 텍스트용 깊은 차콜 블루
            txtContent.VerticalAlignment = VerticalAlignment.Center;
            txtContent.HorizontalAlignment = HorizontalAlignment.Center;
            txtContent.TextWrapping = TextWrapping.Wrap;
            txtContent.TextAlignment = TextAlignment.Center;

            // 5. 하단 버튼 영역 설정
            ButtonPanel.Children.Add(btnOK);
            ButtonPanel.Children.Add(btnYes);
            ButtonPanel.Children.Add(btnNo);

            ButtonPanel.Orientation = Orientation.Horizontal;
            ButtonPanel.HorizontalAlignment = HorizontalAlignment.Center;
            ButtonPanel.VerticalAlignment = VerticalAlignment.Center;
            ButtonPanel.Height = 55; // 버튼 배치 안정감을 위해 높이 소폭 상향
            ButtonPanel.Margin = new Thickness(0, 0, 0, 5);

            // 6. 버튼 속성 및 디자인 깔끔하게 다듬기
            btnOK.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            btnOK.Foreground = Brushes.White;
            btnOK.Content = "확인";
            btnOK.IsDefault = true;
            btnOK.Click += (s, e) => { Result = MessageBoxResult.OK; this.Close(); };

            btnYes.Background = new SolidColorBrush(Color.FromRgb(0, 122, 204));
            btnYes.Foreground = Brushes.White;
            btnYes.Content = "예";
            btnYes.Margin = new Thickness(0, 0, 8, 0);
            btnYes.IsDefault = true;
            btnYes.Click += (s, e) => { Result = MessageBoxResult.Yes; this.Close(); };

            btnNo.Background = new SolidColorBrush(Color.FromRgb(230, 235, 240)); // 아니오 버튼도 톤 매칭
            btnNo.Foreground = new SolidColorBrush(Color.FromRgb(90, 105, 120));
            btnNo.Content = "아니오";
            btnNo.Margin = new Thickness(8, 0, 0, 0);
            btnNo.IsCancel = true;
            btnNo.Click += (s, e) => { Result = MessageBoxResult.No; this.Close(); };
        }

        public void SetMessage(string message)
        {
            txtContent.Text = message;
        }

        public void SetMessage(List<Run> runs)
        {
            txtContent.Inlines.Clear();
            if (runs != null)
            {
                txtContent.Inlines.AddRange(runs);
            }
        }

        public void SetButtonVisibility(MessageBoxType type)
        {
            switch (type)
            {
                case MessageBoxType.YesNo:
                    btnOK.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    break;

                case MessageBoxType.OK:
                    btnOK.Visibility = Visibility.Visible;
                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }

    private enum MessageBoxType
    {
        YesNo,
        OK
    }

    private class MessageBoxButton : Xpf.Button
    {
        public MessageBoxButton()
        {
            this.Width = 95;
            this.Height = 34;
            this.BorderBrush = Brushes.Transparent;
            this.FontWeight = FontWeights.SemiBold;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
        }
    }
}