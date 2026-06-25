using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class SearchEdit : TextEdit
{
    public SearchEdit()
    {
        this.BorderBrush = new SolidColorBrush(Color.FromRgb(210, 215, 220));
        this.Background = new SolidColorBrush(Color.FromRgb(243, 246, 250));
        this.NullTextForeground = Brushes.Gray;
        this.HorizontalContentAlignment = HorizontalAlignment.Center;
        this.MaxLength = 100;
        this.AcceptsReturn = false;
    }

    protected override void OnPreviewKeyDown_TextEdit(object sender, KeyEventArgs e)
    {
        var element = sender as SearchEdit;
        if (element == null) return;

        if (e.Key == Key.Enter)
        {
            // Enter 이동 금지
            e.Handled = false;
            return;
        }
    }
}
