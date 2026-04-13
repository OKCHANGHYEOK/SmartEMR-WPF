using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartEMR.Application.Controls;

public enum TextBoxType { Text, Password }

public class StyleTextBox : StyleGrid
{
    #region Dependency Properties
    public static readonly DependencyProperty TextBoxTypeProperty =
        DependencyProperty.Register(nameof(TextBoxType), typeof(TextBoxType), typeof(StyleTextBox),
            new PropertyMetadata(TextBoxType.Text, OnTextBoxTypeChanged));

    public TextBoxType TextBoxType
    {
        get => (TextBoxType)GetValue(TextBoxTypeProperty);
        set => SetValue(TextBoxTypeProperty, value);
    }

    // 텍스트 값을 바인딩하기 위한 추가 속성 (PasswordBox는 보안상 내부에서 따로 처리 필요)
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(StyleTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion

    private readonly TextBox _textBox = new TextBox();
    private readonly PasswordBox _passwordBox = new PasswordBox();
    private readonly Border _contentBorder = new Border();

    public StyleTextBox()
    {
        InitializeLayout();
        UpdateLayoutByType();
    }

    private void InitializeLayout()
    {
        // 보더 스타일 설정 
        _contentBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(209, 209, 209));
        _contentBorder.BorderThickness = new Thickness(1);
        _contentBorder.CornerRadius = new CornerRadius(5);
        _contentBorder.Background = Brushes.White;
        _contentBorder.Padding = new Thickness(5);

        // 기본 TextBox/PasswordBox 스타일
        _textBox.BorderThickness = new Thickness(0);
        _textBox.VerticalContentAlignment = VerticalAlignment.Center;
        _passwordBox.BorderThickness = new Thickness(0);
        _passwordBox.VerticalContentAlignment = VerticalAlignment.Center;

        // 부모인 StyleGrid(LayoutRoot)에 보더 추가
        // StyleGrid 내부의 LayoutRoot를 사용하도록 AddElement 호출
        this.AddElement(_contentBorder, 0, 0);
    }

    private static void OnTextBoxTypeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is StyleTextBox control)
        {
            control.UpdateLayoutByType();
        }
    }

    private void UpdateLayoutByType()
    {
        // 보더 안의 자식을 타입에 따라 교체
        if (TextBoxType == TextBoxType.Text)
        {
            _contentBorder.Child = _textBox;
        }
        else
        {
            _contentBorder.Child = _passwordBox;
        }
    }
}