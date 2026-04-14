using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public class PasswordBox : ContentControl
{
    private readonly System.Windows.Controls.PasswordBox _passwordBox;

    #region Dependency Properties


    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Password
    {
        get => (string)GetValue(PasswordProperty);
        set => SetValue(PasswordProperty, value);
    }

    public static new readonly DependencyProperty BorderThicknessProperty =
        DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(PasswordBox),
            new PropertyMetadata(new Thickness(1), (d, e) =>
            {
                if (d is PasswordBox pb && e.NewValue is Thickness thickness)
                {
                    pb._passwordBox.BorderThickness = thickness;
                }
            }));

    public new Thickness BorderThickness
    {
        get => (Thickness)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    public static new readonly DependencyProperty PaddingProperty =
        DependencyProperty.Register(nameof(Padding), typeof(Thickness), typeof(PasswordBox),
            new PropertyMetadata(new Thickness(0), (d, e) =>
            {
                if (d is PasswordBox pb && e.NewValue is Thickness padding)
                {
                    pb._passwordBox.Padding = padding;
                }
            }));

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    #endregion

    public PasswordBox()
    {
        _passwordBox = new System.Windows.Controls.PasswordBox

        {
            BorderThickness = this.BorderThickness,
            Padding = this.Padding,
            Background = System.Windows.Media.Brushes.Transparent,
            BorderBrush = System.Windows.Media.Brushes.Transparent
        };

        _passwordBox.PasswordChanged += PasswordBox_OnPasswordChanged;

        this.Content = _passwordBox;

        // 포커스가 이 컨트롤에 오면 내부 PasswordBox로 전달
        this.Focusable = true;
        this.GotFocus += (s, e) => _passwordBox.Focus();
    }

    private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        // 1. 내부 PasswordBox의 값을 의존성 속성인 Password에 동기화 (가장 중요)
        if (sender is System.Windows.Controls.PasswordBox pb)
        {
            this.Password = pb.Password;
        }

        // 기존 리플렉션 로직 (필요 시 유지)
        if (this.DataContext is not null)
        {
            var model = this.DataContext.GetType().GetProperty("Model")?.GetValue(this.DataContext);
            // ... 생략
        }
    }
}