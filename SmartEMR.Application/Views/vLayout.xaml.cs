using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartEMR.Application.Controls
{
    public partial class vLayout : UserControl
    {
        public vLayout()
        {
            InitializeComponent();
        }

        // 컨텐츠를 담을 속성 정의
        public static readonly DependencyProperty MainContentProperty =
            DependencyProperty.Register("MainContent", typeof(object), typeof(vLayout));

        public object MainContent
        {
            get => GetValue(MainContentProperty);
            set => SetValue(MainContentProperty, value);
        }

        // 상단 추가 정보(환자 정보 등)를 담을 속성
        public static readonly DependencyProperty HeaderExtraProperty =
            DependencyProperty.Register("HeaderExtra", typeof(object), typeof(vLayout));

        public object HeaderExtra
        {
            get => GetValue(HeaderExtraProperty);
            set => SetValue(HeaderExtraProperty, value);
        }

        // 창 이동 로직
        private void TitleGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Window.GetWindow(this)?.DragMove();
            }
        }

        // 창 닫기 로직
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}