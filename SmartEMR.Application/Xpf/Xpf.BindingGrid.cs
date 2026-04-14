using CommunityToolkit.Mvvm.ComponentModel;
using SmartEMR.Application.Common;
using System.Collections.ObjectModel; 
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf;

[ObservableObject]
[ContentProperty(nameof(BindingElements))]
public partial class BindingGrid : StyleGrid
{
    [ObservableProperty]
    public int m_ItemSpace = 5;

    private object? Model = null;

    // 1. UIElementCollection 대신 ObservableCollection<BindingElement> 사용
    public ObservableCollection<BindingElement> BindingElements { get; }
        = new ObservableCollection<BindingElement>();

    public BindingGrid() : base()
    {
        this.BindingElements.CollectionChanged += OnBindingElementsChanged;
        this.DataContextChanged += (s, e) => UpdateModel();
    }

    private void UpdateModel()
    {
        if (this.DataContext == null) return;

        this.Model = this.DataContext.GetType().GetProperty("Model")?.GetValue(this.DataContext);
    }

    private void AddElement(BindingElement element)
    {
        FrameworkElement? visualChild = null;

        if (element.BindingType == BindingType.TextBox || element.BindingType == BindingType.PasswordBox)
        {
            visualChild = new StyleTextBox();
            visualChild.DataContext = this.Model;  

            if (element.BindingType == BindingType.TextBox)
            {
                ((StyleTextBox)visualChild).TextBoxType = TextBoxType.Text;
            }
            else if (element.BindingType == BindingType.PasswordBox)
            {
                ((StyleTextBox)visualChild).TextBoxType = TextBoxType.Password;
            }

            if (!string.IsNullOrWhiteSpace(element.Placeholder))
            {
                ((StyleTextBox)visualChild).PlaceHolder = element.Placeholder;
            }
        }

        if (visualChild == null) return;

        visualChild.SetValue(MarginProperty, new Thickness(this.ItemSpace));
        visualChild.SetValue(DataContextProperty, this.Model);

        BindingExtensions.SetBinding(visualChild, element.FieldName ?? "");

        AddElement(visualChild, element.Col, element.Row);
    }

    private void OnBindingElementsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
        {
            if (e.NewItems == null) return;

            foreach (BindingElement item in e.NewItems)
            {
                // 3. BindingElement 정보를 바탕으로 실제 UI 생성
                AddElement(item);
            }
        }
    }
}