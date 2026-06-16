using System.Windows;

namespace SmartEMR.Application.Xpf;

public class ComboBoxEdit : DevExpress.Xpf.Editors.ComboBoxEdit
{
    public ComboBoxEdit()
    {
        this.MinHeight = 20;
        this.MinWidth = 40;
        this.VerticalAlignment = VerticalAlignment.Stretch;
        this.HorizontalAlignment = HorizontalAlignment.Center;
        this.IsTextEditable = false;
    }
}
