using System.Windows;
using System.Windows.Controls;

namespace SmartEMR.Application.Xpf;

public class RichTextEdit : UserControl
{
    //public static readonly DependencyProperty TextProperty =
    //    DependencyProperty.Register(nameof(Text), typeof(string), typeof(RichTextEdit), new PropertyMetadata(string.Empty));

    //public string Text
    //{
    //    get => (string)GetValue(TextProperty);
    //    set => SetValue(TextProperty, value);
    //}

    //public static readonly DependencyProperty RtfTextProperty =
    //    DependencyProperty.Register(nameof(RtfText), typeof(string), typeof(RichTextEdit), new PropertyMetadata(string.Empty));

    //public string RtfText
    //{
    //    get => (string)GetValue(RtfTextProperty);
    //    set => SetValue(RtfTextProperty, value);
    //}

    static RichTextEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextEdit), new FrameworkPropertyMetadata(typeof(RichTextEdit)));
    }
}
