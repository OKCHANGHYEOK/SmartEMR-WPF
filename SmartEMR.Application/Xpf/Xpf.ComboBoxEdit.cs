using System.Windows.Input;
using DevExpress.Xpf.Editors;

namespace SmartEMR.Application.Xpf;

public class ComboBoxEdit : DevExpress.Xpf.Editors.ComboBoxEdit
{
    public ComboBoxEdit()
    {
        this.MinHeight = 20;
        this.MinWidth = 40;
        this.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
        this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
        this.ShowNullTextForEmptyValue = false;
        this.IsTextEditable = false;

        // ⭐️ [해결 코드] 마우스로 클릭할 때 레이아웃 충돌로 팝업이 씹히는 현상을 방지합니다.
        this.PreviewMouseDown += ComboBoxEdit_PreviewMouseDown;
    }

    private void ComboBoxEdit_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is not ComboBoxEdit cmb) return;

        // 이미 팝업이 열려있다면 닫아주고, 닫혀있다면 강제로 이벤트를 선점하여 확실하게 열어줍니다.
        if (!cmb.IsPopupOpen)
        {
            cmb.Focus();
            cmb.ShowPopup();
            e.Handled = true; // 💡 이벤트가 위로 올라가서 다른 갱신 로직과 타이밍 꼬이는 것을 방지
        }
    }
}