using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SmartEMR.Application.Core;

namespace SmartEMR.Application.Xpf;

public partial class RichTextEdit : UserControl
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(RichTextEdit), new PropertyMetadata(string.Empty));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private PopupColorEdit? _colorEdit;
    private ComboBoxEdit? _fontFamilyComboBox;
    private ComboBoxEdit? _fontSizeComboBox;
    private RichEditControl? _richEdit;

    static RichTextEdit()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RichTextEdit), new FrameworkPropertyMetadata(typeof(RichTextEdit)));
    }

    [RelayCommand]
    public void ClearDocument()
    {
        if (_richEdit is null) return;

        DocumentRange range = _richEdit.Document.Range;
        if (range.Length == 0) return;

        if (SmartUI.MsgYesNo("입력된 내용이 모두 지워집니다. 지우시겠습니까?") is MessageBoxResult.Yes)
        {
            _richEdit.Document.Delete(range);
        }
    }

    public override void OnApplyTemplate()
    {
        // 기존 이벤트 연결 제거
        if (_fontFamilyComboBox is not null)
        {
            _fontFamilyComboBox.EditValueChanged -= OnEditValueChanged_FontFamily;
        }

        base.OnApplyTemplate();

        // Template 내부 컨트롤 가져오기
        _richEdit = GetTemplateChild("RichEdit") as RichEditControl;
        _colorEdit = GetTemplateChild("ColorEdit") as PopupColorEdit;
        _fontFamilyComboBox = GetTemplateChild("cmbFontFamiliy") as ComboBoxEdit;
        _fontSizeComboBox = GetTemplateChild("cmbFontSize") as ComboBoxEdit;

        if (_richEdit is not null)
        {
            _richEdit.TextChanged += OnTextChanged_RichEdit;
        }

        if (_colorEdit is not null)
        {
            _colorEdit.EditValueChanged += OnEditValueChanged_ColorEdit;
        }

        // 이벤트 연결
        if (_fontFamilyComboBox is not null)
        {
            _fontFamilyComboBox.EditValueChanged += OnEditValueChanged_FontFamily;
        }

        if (_fontSizeComboBox is not null)
        {
            _fontSizeComboBox.EditValueChanged += OnEditValueChanged_FontSize;
        }
    }

    private void OnTextChanged_RichEdit(object? sender, EventArgs e)
    {
        if (sender is not RichEditControl element) return;

        SetValue(TextProperty, element.RtfText);
    }

    private void OnEditValueChanged_ColorEdit(object sender, EditValueChangedEventArgs e)
    {
        if (_richEdit is null) return;

        DocumentRange range = _richEdit.Document.Selection;
        if (range.Length == 0) return;

        if (e.NewValue is not Color color) return;

        CharacterProperties properties = _richEdit.Document.BeginUpdateCharacters(range);

        try
        {
            properties.ForeColor = color.ToDrawingColor();
        }
        finally
        {
            _richEdit.Document.EndUpdateCharacters(properties);
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

    private void OnEditValueChanged_FontSize(object sender, EditValueChangedEventArgs e)
    {
        if (_richEdit is null) return;

        DocumentRange range = _richEdit.Document.Selection;

        if (range.Length == 0) return;

        if (e.NewValue is not double fontSize) return;

        CharacterProperties properties = _richEdit.Document.BeginUpdateCharacters(range);

        try
        {
            properties.FontSize = (float)fontSize;
        }
        finally
        {
            _richEdit.Document.EndUpdateCharacters(properties);
        }
    }
}
