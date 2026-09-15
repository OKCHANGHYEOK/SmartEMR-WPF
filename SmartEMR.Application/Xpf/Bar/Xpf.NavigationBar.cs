using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Collections.ObjectModel; 
using System.Collections.Specialized;

namespace SmartEMR.Application.Xpf.Bar
{
    [ContentProperty(nameof(BarItems))]
    public class NavigationBar : UserControl
    {
        private Border LayoutBorder = new();
        private ItemsControl LayoutRoot = new();

        // UIElementCollection 대신 ObservableCollection을 사용합니다.
        public ObservableCollection<UIElement> BarItems { get; } = new();

        public event RoutedEventHandler? BarItemClick;

        public NavigationBar()
        {
            // 1. 가로 정렬 설정
            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(StackPanel));
            factory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            LayoutRoot.ItemsPanel = new ItemsPanelTemplate(factory);

            // 2. 데이터 소스 연결
            LayoutRoot.ItemsSource = BarItems;

            // 3. 레이아웃 구성
            LayoutBorder.Child = LayoutRoot;

            this.Content = LayoutBorder;

            BarItems.CollectionChanged += OnBarItemsCollectionChanged;
        }

        private void OnBarItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        if (item is Button btn)
                        {
                            btn.Click += BarItemClick;
                        }
                    }
                }
            }
        }
    }
}