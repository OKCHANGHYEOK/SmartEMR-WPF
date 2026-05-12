using DevExpress.Xpf.Core.Internal;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf.Bar
{
    public enum BarItemStyle
    {
        Default = 0,
        Emphasis = 1
    }


    public class BarItem : Button
    {
        // 1. 이미지 소스 (SVG나 ImagePath)
        public static readonly DependencyProperty GlyphProperty =
            DependencyProperty.Register("Glyph", typeof(ImageSource), typeof(BarItem), new PropertyMetadata(null));

        [TypeConverter(typeof(SvgImageSourceConverter))]
        public ImageSource Glyph
        {
            get => (ImageSource)GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(BarItem), new PropertyMetadata(60.0));

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register("ImageHeight", typeof(double), typeof(BarItem), new PropertyMetadata(60.0));

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public static DependencyProperty BarItemStyleProperty =
            DependencyProperty.Register("BarItemStyle", typeof(BarItemStyle), typeof(BarItem), new PropertyMetadata(BarItemStyle.Default, OnBarItemStylePropertyChanged));

        private static void OnBarItemStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BarItem element && Enum.TryParse<BarItemStyle>(e.NewValue?.ToString(), out var style))
            {
                SetBarItemStyle(element, style);
            }
        }

        public BarItemStyle BarItemStyle
        {
            get => (BarItemStyle)GetValue(BarItemStyleProperty);
            set => SetValue(BarItemStyleProperty, value);
        }

        static BarItem()
        {
            // 기본 스타일을 Generic.xaml이 아닌 코드에서 직접 정의하거나 스타일을 주입할 수 있습니다.
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BarItem), new FrameworkPropertyMetadata(typeof(BarItem)));
        }

        public BarItem()
        {
            // 기본 디자인 설정
            this.MinWidth = 60;
            this.Background = Brushes.Transparent;
            this.BorderThickness = new Thickness(0);
            this.Margin = new Thickness(3);
            this.Cursor = System.Windows.Input.Cursors.Hand;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.VerticalContentAlignment = VerticalAlignment.Center;
            this.FontSize = 11;

            SetBarItemStyle(this, this.BarItemStyle);
        }

        static void SetBarItemStyle(BarItem element, BarItemStyle style)
        {
            switch (style)
            {
                case BarItemStyle.Default:
                    element.Foreground = new SolidColorBrush(Color.FromRgb(68, 68, 68));
                    element.FontWeight = FontWeights.SemiBold;
                    break;

                case BarItemStyle.Emphasis:
                    element.Foreground = new SolidColorBrush(Color.FromRgb(25, 25, 25));
                    element.FontWeight = FontWeights.Bold;
                    break;
            }
        }
    }
}