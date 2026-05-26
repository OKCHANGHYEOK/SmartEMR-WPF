using SmartEMR.Application.Core;
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
    public Patient? SelectedItem
    {
        get
        {
            if (ResultListBox.SelectedIndex >= 0)
            {
                return ResultListBox.SelectedItem as Patient;
            }
            else
            {
                return null;
            }
        }
    }

    public int SelectedIndex
    {
        get
        {
            return ResultListBox.SelectedIndex;
        }
    }

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

        FocusToResultListBox();
    }

    public void FocusToResultListBox()
    {
        if (ResultListBox.Items != null && ResultListBox.Items.Count > 0)
        {
            // 1. 먼저 첫 번째 아이템을 선택 상태로 만듭니다.
            ResultListBox.SelectedIndex = 0;

            // 2. WPF 렌더링/입력 큐가 정리된 후 아이템 컨테이너에 직접 포커스를 주도록 Dispatcher 활용
            SmartUI.BeginInvoke(new Action(() =>
            {
                // 첫 번째 인덱스의 비주얼 컨테이너(ListBoxItem)를 가져옵니다.
                var item = ResultListBox.ItemContainerGenerator.ContainerFromIndex(0) as ListBoxItem;

                if (item != null)
                {
                    // 생성된 아이템 컨테이너에 직접 포커스를 찌릅니다. (가장 확실함)
                    item.Focus();
                }
                else
                {
                    // 만약 순간적으로 아이템이 로드되지 않았다면 리스트박스 자체에 백업 포커스
                    ResultListBox.Focus();
                }
            }), System.Windows.Threading.DispatcherPriority.Input); // Input 우선순위로 제어권을 잡음
        }
        else
        {
            ResultListBox.SelectedIndex = -1;
        }
    }

    public void SetSelectedIndex(int selectedIndex)
    {
        ResultListBox.SelectedIndex = selectedIndex;
    }
}
