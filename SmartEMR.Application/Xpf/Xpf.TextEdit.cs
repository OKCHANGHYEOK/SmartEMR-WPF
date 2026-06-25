using SmartEMR.Application.Common;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public class TextEdit : DevExpress.Xpf.Editors.TextEdit
{
    public TextEdit ()
    {
        this.MinHeight = 26;
        this.BorderBrush = Brushes.Transparent;
        this.BorderThickness = new Thickness(1);

        this.PreviewKeyDown += OnPreviewKeyDown_TextEdit;
    }

    protected virtual void OnPreviewKeyDown_TextEdit(object sender, System.Windows.Input.KeyEventArgs e)
    {
        var element = sender as TextEdit;
        if (element == null) return;

        if (e.Key == Key.Enter || e.Key == Key.Tab)
        {
            bool bFlag = TextFocusBehavior.SetFocusToNext(element);
            if (bFlag)
            {
                e.Handled = true;
            }
        }
    }
}
