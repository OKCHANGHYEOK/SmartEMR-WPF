using DevExpress.Xpf.Core;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartEMR.Application.Windows;

/// <summary>
/// StartUpWindow.xaml에 대한 상호 작용 논리
/// </summary>
public partial class StartUpWindow : Window
{
    public StartUpWindow() : base()
    {
        InitializeComponent();

        StartUpView.SuccessLogin += (s, e) =>
        {
            this.Close();
        };
    }

    private void OnMouseLeftButtonDown_Header(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (sender is not Border headerBorder || headerBorder != HeaderBorder) return;

        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void OnClick_SimpleButton(object sender, RoutedEventArgs e)
    {
        if (sender is not SimpleButton element) return;

        switch (element.Name)
        {
            case nameof(btnMinimize):
                WindowState = WindowState.Minimized;
                break;

            case nameof(btnClose):
                if (MessageBox.Show("SmartEMR 프로그램을 종료하시겠습니까?", "확인", MessageBoxButton.YesNo, MessageBoxImage.Question) is MessageBoxResult.Yes)
                {
                    Close();
                }

                break;
        }
    }
}
