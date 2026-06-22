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
        this.ShowNullTextForEmptyValue = false;
        this.IsTextEditable = false;
        this.PreviewMouseLeftButtonDown += ComboBoxEdit_PreviewMouseLeftButtonDown;
    }

    private void ComboBoxEdit_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        var cmb = sender as ComboBoxEdit;
        if (cmb == null) return;

        if (cmb.ItemsSource != null) 
        {
            cmb.ShowPopup();
            e.Handled = true;
        }
    }
}