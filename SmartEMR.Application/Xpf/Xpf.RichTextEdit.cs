using System.Windows;
using System.Windows.Controls;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit.API.Native;

namespace SmartEMR.Application.Xpf;

public class RichTextEdit : UserControl
{
    static RichTextEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextEdit), new FrameworkPropertyMetadata(typeof(RichTextEdit)));
    }

    private ComboBoxEdit? _fontFamilyComboBox;
    private RichEditControl? _richEdit;

    public override void OnApplyTemplate()
    {
        // 기존 이벤트 연결 제거
        if (_fontFamilyComboBox is not null)
        {
            _fontFamilyComboBox.EditValueChanged -= OnEditValueChanged_FontFamily;
        }

        base.OnApplyTemplate();

        // Template 내부 컨트롤 가져오기
        _fontFamilyComboBox = GetTemplateChild("cmbFontFamiliy") as ComboBoxEdit;
        _richEdit = GetTemplateChild("RichEdit") as RichEditControl;

        // 이벤트 연결
        if (_fontFamilyComboBox is not null)
        {
            _fontFamilyComboBox.EditValueChanged += OnEditValueChanged_FontFamily;
        }
    }

    private void OnEditValueChanged_FontFamily(object sender, EditValueChangedEventArgs e)
    {
        if (_richEdit is null) return;

        // 현재 선택 영역
        DocumentRange range = _richEdit.Document.Selection;

        // 선택 영역이 없으면 일단 무시 -> 선택 영역 없을 때 폰트 바꿔놓고 입력하는 경우 처리해야함
        if (range.Length == 0) return;

        string? fontName = e.NewValue?.ToString();

        if (string.IsNullOrWhiteSpace(fontName)) return;

        CharacterProperties properties = _richEdit.Document.BeginUpdateCharacters(range);

        try
        {
            properties.FontName = fontName;
        }
        finally
        {
            _richEdit.Document.EndUpdateCharacters(properties);
        }
    }
}
