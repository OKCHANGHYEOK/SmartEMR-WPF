using SmartEMR.Application.Xpf;
using SmartEMR.Domain.Entities;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace SmartEMR.Application.Views.Shared;

/// <summary>
/// vSearchViewResult.xaml에 대한 상호 작용 논리
/// </summary>
public partial class vSearchViewResult : CustomControl
{
    public ObservableCollection<Patient> Items { get; set; }

    public vSearchViewResult()
    {
        Items = new ObservableCollection<Patient>();
    }

    public void UpdateItemsSource(IQueryable<Patient> arrPAT)
    {
        if (arrPAT == null || !arrPAT.Any()) return;

        Items.Clear();
    
        foreach (var item in arrPAT)
        {
            Items.Add(item);
        }
    }
}
