using SmartEMR.Application.Common;
using System.Windows.Input;

namespace SmartEMR.Application.Xpf;

public class TextEdit : DevExpress.Xpf.Editors.TextEdit
{
    public TextEdit ()
    {
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
