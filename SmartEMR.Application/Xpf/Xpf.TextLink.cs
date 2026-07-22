using System.Windows;

namespace SmartEMR.Application.Xpf;

public class TextLink : System.Windows.Controls.TextBlock
{
    static TextLink()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(TextLink), new FrameworkPropertyMetadata(typeof(TextLink)));
    }
}
