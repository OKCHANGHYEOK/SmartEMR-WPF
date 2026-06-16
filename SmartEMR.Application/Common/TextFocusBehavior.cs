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
        // isReadOnly = true 인 요소는 건너뜀
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

                    if (child is TextBox textBox && !textBox.IsReadOnly)
                    {
                        textBox.Focus();
                        return;
                    }
                    else if (child is StyleTextBox stb && !stb.IsReadOnly)
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

    public static bool SetFocusToNext(TextEdit element)
    {
        if (element == null || element.AcceptsReturn == true)
            return false;

        var currentElement = element as UIElement;
        if (currentElement == null)
            return false;

        // WPF 표준 탐색 요청 객체 생성 (Next = 다음 탭 인덱스)
        var request = new TraversalRequest(FocusNavigationDirection.Next);

        // 무한 루프와 예외를 방지하기 위해 최대 순회 횟수를 화면 내 컨트롤 개수 수준(예: 30회)으로 강제 제한합니다.
        int maxAttempts = 30;
        int attempts = 0;

        // 최초 출발 지점의 컨트롤 기억
        UIElement startingElement = currentElement;

        while (currentElement != null && attempts < maxAttempts)
        {
            attempts++;

            // 🎯 실제로 다음 컨트롤로 포커스를 1칸 이동시킵니다.
            // MoveFocus는 내부적으로 유효성 검사를 거치므로 Enum 오류가 발생하지 않습니다.
            bool isMoved = currentElement.MoveFocus(request);

            // 포커스 이동에 실패했거나 키보드 포커스 객체를 못 가져오면 중단
            if (!isMoved) break;

            var newFocused = Keyboard.FocusedElement as UIElement;

            // 안전장치: 포커스가 비었거나, 한 바퀴 돌아서 자기 자신에게 다시 왔다면 종료
            if (newFocused == null || newFocused == startingElement || newFocused == currentElement)
                break;

            // 💡 성공 조건: 새로 포커스를 잡은 놈이 TextBox류이거나 DevExpress 에디터 종류일 때
            string typeName = newFocused.GetType().Name;
            if (newFocused is TextBox || typeName.Contains("TextEdit") || typeName.Contains("TextBox"))
            {
                // 원하는 입력창에 안착했으므로 즉시 루프 탈출!
                break;
            }

            // 만약 새로 간 곳이 버튼, 라벨, 체크박스 등등 입력창이 아니라면
            // 그 컨트롤을 기준점으로 삼아 다음 칸으로 한 번 더 이동하도록 갱신합니다.
            currentElement = newFocused;
        }

        return true;
    }
}
