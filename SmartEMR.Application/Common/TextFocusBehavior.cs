using DevExpress.Xpf.Core.Native;
using SmartEMR.Application.Core;
using SmartEMR.Application.ViewBase;
using SmartEMR.Application.Xpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SmartEMR.Application.Common;

public class TextFocusBehavior
{
    public static void SetFocusToFirstTextElement(ViewLayout viewLayout)
    {
        // 팝업뷰인 경우 입력 가능한 요소중 첫번째 요소를 찾아 포커스
        if (viewLayout.IsPopupView)
        {
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(viewLayout);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int count = VisualTreeHelper.GetChildrenCount(current);

                for (int i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);

                    if (child is TextBox textBox)
                    {
                        textBox.Focus();
                        return;
                    }
                    else if (child is StyleTextBox stb)
                    {
                        stb.Focus();
                        return;
                    }

                    queue.Enqueue(child);
                }
            }
        }
    }

    public static void SetFocusByName(string elementName)
    {
        if (string.IsNullOrWhiteSpace(elementName)) return;

        var currentView = SmartUI.CurrentView;
        if (currentView == null) return;

        var parent = currentView as DependencyObject;
        if (parent == null) return; // 안전을 위한 방어 코드 추가

        var queue = new Queue<DependencyObject>();
        queue.Enqueue(parent);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            var count = VisualTreeHelper.GetChildrenCount(current);

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(current, i);

                // ⭐ [핵심 수정] child가 FrameworkElement 패밀리가 맞는지 먼저 확인합니다.
                // TextBoxLineDrawingVisual 같은 저수준 객체들은 이 조건문에서 걸러집니다.
                if (child is FrameworkElement fe)
                {
                    // 이름이 일치하는지 바로 확인하는 방식을 쓰거나,
                    // 기존에 만드신 확장 메서드가 있다면 fe.GetElementByName(...) 형태로 안전하게 호출합니다.
                    if (fe.Name == elementName)
                    {
                        // 포커스를 줄 수 있는 요소인지 한 번 더 체크하면 좋습니다.
                        if (fe is UIElement uiElement)
                        {
                            uiElement.Focus();
                            System.Windows.Input.Keyboard.Focus(uiElement); // 키보드 포커스 확정
                        }
                        return;
                    }
                }

                // FrameworkElement가 아니더라도 자식의 자식이 있을 수 있으므로 큐에는 계속 넣어줍니다.
                queue.Enqueue(child);
            }
        }
    }

    public static void SetFocusToNext(TextEdit element)
    {
        if (element == null) return;

        var focusedElement = element as UIElement;
        var request = new TraversalRequest(FocusNavigationDirection.Next);

        // 무한 루프 방지를 위해 처음 출발한 컨트롤을 기억합니다.
        UIElement startingElement = focusedElement;

        while (focusedElement != null)
        {
            // 다음 컨트롤로 포커스 이동 시도
            focusedElement.MoveFocus(request);

            // 이동 후 실제로 포커스를 먹은 요소를 새로 가져옴
            var newFocused = Keyboard.FocusedElement as UIElement;

            // 1. 만약 포커스가 바뀐 컨트롤이 TextBox나 TextEdit라면 성공! 루프 탈출
            if (newFocused is TextBox || newFocused is TextEdit)
                break;

            // 2. [안전장치] 포커스가 안 바뀌었거나, 한 바퀴 돌아서 처음 출발지로 다시 왔다면 탈출
            if (newFocused == null || newFocused == focusedElement || newFocused == startingElement)
                break;

            // 다음 루프를 위해 갱신
            focusedElement = newFocused;
        }
    }
}
