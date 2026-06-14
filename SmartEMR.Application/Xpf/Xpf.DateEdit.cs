using System.Windows;


namespace SmartEMR.Application.Xpf;

public class DateEdit : DevExpress.Xpf.Editors.DateEdit
{
    public DateEdit ()
    {
        this.MinHeight = 21;
        this.MinWidth = 45;
        this.HorizontalContentAlignment = HorizontalAlignment.Center;
    }
}
