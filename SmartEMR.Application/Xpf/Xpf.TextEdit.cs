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

        switch (e.Key)
        {
            case Key.Tab:
                if (TextFocusBehavior.IsInsideFocusScope(element))
                    return;

                if (TextFocusBehavior.SetFocusToNext(element))
                {
                    e.Handled = true;
                }

                break;

            case Key.Enter:
                if (TextFocusBehavior.SetFocusToNext(element))
                {
                    e.Handled = true;
                }

                break;

            default:
                return;
        }
    }
}
