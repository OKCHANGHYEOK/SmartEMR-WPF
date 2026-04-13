using SmartEMR.Application.Common;
using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Controls;

public class PasswordBox : ContentControl
{
    private readonly System.Windows.Controls.PasswordBox _passwordBox;

    #region Dependency Properties

    public static readonly DependencyProperty FieldNameProperty =
        DependencyProperty.Register(nameof(FieldName), typeof(string), typeof(PasswordBox),
            new PropertyMetadata(null));

    public string FieldName
    {
        get => (string)GetValue(FieldNameProperty);
        set => SetValue(FieldNameProperty, value);
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
        if (string.IsNullOrWhiteSpace(this.FieldName)) return;

        // 패턴 매칭을 이용한 안전한 형변환 및 데이터 처리
        if (sender is System.Windows.Controls.PasswordBox pb && this.DataContext is not null)
        {
            // 리플렉션을 사용하여 ViewModel의 Model 객체 접근
            var model = this.DataContext.GetType().GetProperty("Model")?.GetValue(this.DataContext);

            if (model != null)
            {
                // FieldName에 해당하는 속성에 패스워드 값 설정
                var prop = model.GetType().GetProperty(this.FieldName);
                prop?.SetValue(model, pb.Password);
            }
        }
    }
}