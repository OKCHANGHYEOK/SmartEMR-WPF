using SmartEMR.Domain.Entities;
using System.Windows;
using System.Windows.Input;

namespace SmartEMR.Application.Xpf;

/// <summary>
/// Xpf.xaml에 대한 상호 작용 논리
/// </summary>
public partial class SearchPanel : CustomControl
{
    public static readonly DependencyProperty SearchCommandProperty =
        DependencyProperty.Register(
            nameof(SearchCommand),
            typeof(ICommand),
            typeof(SearchPanel));

    public ICommand? SearchCommand
    {
        get => (ICommand?)GetValue(SearchCommandProperty);
        set => SetValue(SearchCommandProperty, value);
    }

    public SearchEdit txtSearch => SearchEdit;

    public string? NullText { get; set; }

    public void SetFocusToSearchEdit()
    {
        SearchEdit.Focus();
    }

    public void SetSelectedPatient(Patient item)
    {
        if (item is null) return;

        SearchEdit.EditValue = $"{item.PAT_Name}({item.PAT_ChartNo})";
    }

    public void ClearData()
    {
        SearchEdit.EditValue = "";
    }

    private void OnPreviewKeyDown_SearchEdit(object sender, KeyEventArgs e)
    {
        var element = sender as SearchEdit;
        if (element is null) return;

        if (element.IsKeyboardFocusWithin && e.Key is Key.Enter) 
        {
            SearchCommand?.Execute(null);

            e.Handled = true;
        }
    }
}
