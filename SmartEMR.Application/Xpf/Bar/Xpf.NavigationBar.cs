using SmartEMR.Application.Common;
using SmartEMR.Application.Core;
using SmartEMR.Application.Views;
using SmartEMR.Application.Views.SmartEMRCST;
using SmartEMR.Application.Views.SmartEMRRES.SmartEMRRESCalendarTab;
using System.Collections.ObjectModel; // 추가
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace SmartEMR.Application.Xpf.Bar
{
    [ContentProperty(nameof(BarItems))]
    public class NavigationBar : UserControl
    {
        private Border LayoutBorder = new();
        private ItemsControl LayoutRoot = new();

        // UIElementCollection 대신 ObservableCollection을 사용합니다.
        public ObservableCollection<UIElement> BarItems { get; } = new();

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
                            btn.Click += OnBarItem_Click;
                        }
                    }
                }
            }
        }

        private async void OnBarItem_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as Button;

            if (element != null)
            {
                var bFlag = Enum.TryParse<eSmartEMRLocation>(element.Tag.ToString(), out var location);

                if (bFlag)
                {
                    switch (location)
                    {
                        case eSmartEMRLocation.RES:
                            await SmartUI.NavigateToPage(new vSmartEMRRESCalendarTab());
                            break;

                        case eSmartEMRLocation.DSK:
                            await SmartUI.NavigateToPage(new vSmartEMRDeskTab());
                            break;

                        case eSmartEMRLocation.CST:
                            await SmartUI.NavigateToPage(new vSmartEMRConsultationTab());
                            break;

                        case eSmartEMRLocation.PAY:
                            break;

                        case eSmartEMRLocation.CRM:
                            break;

                        case eSmartEMRLocation.CONFIG:
                            break;
                    }
                }
            }
        }
    }
}