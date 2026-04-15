using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class Button : System.Windows.Controls.Button
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(Button),
        new PropertyMetadata(new CornerRadius(0)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public Button() : base()
    {
        this.MinWidth = 42;
        this.MinHeight = 22;
        this.Background = Brushes.White;
        this.BorderBrush = new SolidColorBrush(Color.FromRgb(220, 220, 220));
        this.BorderThickness = new Thickness(1);

        var template = new ControlTemplate(typeof(System.Windows.Controls.Button));
        var borderFactory = new FrameworkElementFactory(typeof(Border));

        // 1. 일반적인 속성 연결 (TemplateBinding 대신 SetBinding 사용)
        // RelativeSource를 TemplatedParent로 설정하면 코드비하인드에서 가장 확실하게 작동합니다.
        borderFactory.SetBinding(Border.BackgroundProperty, new Binding(nameof(Background)) { RelativeSource = RelativeSource.TemplatedParent });
        borderFactory.SetBinding(Border.BorderBrushProperty, new Binding(nameof(BorderBrush)) { RelativeSource = RelativeSource.TemplatedParent });
        borderFactory.SetBinding(Border.BorderThicknessProperty, new Binding(nameof(BorderThickness)) { RelativeSource = RelativeSource.TemplatedParent });

        // 2. ⭐ 우리가 만든 커스텀 CornerRadius 연결
        borderFactory.SetBinding(Border.CornerRadiusProperty, new Binding(nameof(CornerRadius)) { RelativeSource = RelativeSource.TemplatedParent });

        var presenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
        presenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
        presenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
        presenterFactory.SetValue(ContentPresenter.MarginProperty, new Thickness(5, 2, 5, 2));

        borderFactory.AppendChild(presenterFactory);
        template.VisualTree = borderFactory;

        this.Template = template;
    }
}