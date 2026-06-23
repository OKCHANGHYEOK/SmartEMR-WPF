namespace SmartEMR.Application.Xpf;

public class ComboBoxEdit : DevExpress.Xpf.Editors.ComboBoxEdit
{
    public ComboBoxEdit()
    {
        this.MinHeight = 20;
        this.MinWidth = 40;
        this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        this.ShowNullTextForEmptyValue = false;
        this.IsTextEditable = false;
    }
}