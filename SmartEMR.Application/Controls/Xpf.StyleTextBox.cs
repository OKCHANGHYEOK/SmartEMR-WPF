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

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(StyleTextBox),
            new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnTextChanged)); // 콜백 추가

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public static readonly DependencyProperty PlaceHolderProperty =
        DependencyProperty.Register(nameof(PlaceHolder), typeof(string), typeof(StyleTextBox),
            new PropertyMetadata(string.Empty, OnPlaceHolderChanged));

    public string? PlaceHolder
    {
        get => (string?)GetValue(PlaceHolderProperty);
        set => SetValue(PlaceHolderProperty, value);
    }

    #endregion

    private readonly TextBox _textBox = new TextBox();
    private readonly PasswordBox _passwordBox = new PasswordBox();
    private readonly Border _contentBorder = new Border();
    private readonly Label lblPlaceHolder = new Label();

    public StyleTextBox()
    {
        InitializeLayout();
    }

    public StyleTextBox(TextBoxType type) : this()
    {
        TextBoxType = type;

        InitializeLayout();
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

        // 플레이스홀더 스타일
        lblPlaceHolder.FontWeight = FontWeights.SemiBold;
        lblPlaceHolder.Foreground = new SolidColorBrush(Color.FromRgb(187, 187, 187));
        lblPlaceHolder.VerticalAlignment = VerticalAlignment.Center;
        lblPlaceHolder.Margin = new Thickness(5, 0, 0, 0);
        lblPlaceHolder.IsHitTestVisible = false; // PlaceHolder는 클릭 이벤트를 받지 않도록 설정     

        this.AddElement(_contentBorder, 0, 0);
        this.AddElement(lblPlaceHolder, 0, 0);

        // 바인딩
        _textBox.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding(nameof(Text)) { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay, UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });
        _passwordBox.SetBinding(PasswordBox.PasswordProperty, new System.Windows.Data.Binding(nameof(Text)) { Source = this, Mode = System.Windows.Data.BindingMode.TwoWay, UpdateSourceTrigger = System.Windows.Data.UpdateSourceTrigger.PropertyChanged });

        UpdateLayoutByType();
        UpdatePlaceHolder();
    }

    private static void OnTextBoxTypeChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is not StyleTextBox control) return;

        control.UpdateLayoutByType();
    }

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is StyleTextBox control)
        {
            control.UpdatePlaceHolder();
        }
    }

    private static void OnPlaceHolderChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
    {
        if (element is not StyleTextBox control) return;

        control.UpdatePlaceHolder();
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

    private void UpdatePlaceHolder()
    {
        if (string.IsNullOrWhiteSpace(this.PlaceHolder))
        {
            lblPlaceHolder.Visibility = Visibility.Collapsed;
            return;
        }

        lblPlaceHolder.Content = this.PlaceHolder;

        // Text 속성이 비어있는지 확인 (PasswordBox와 바인딩되어 있으므로 동기화됨)
        bool isTextEmpty = string.IsNullOrEmpty(this.Text);

        lblPlaceHolder.Visibility = isTextEmpty ? Visibility.Visible : Visibility.Collapsed;
    }
}