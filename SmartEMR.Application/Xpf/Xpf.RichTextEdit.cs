using CommunityToolkit.Mvvm.Input;
using DevExpress.Xpf.Editors;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using SmartEMR.Application.Core;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartEMR.Application.Xpf;

public partial class RichTextEdit : UserControl
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(RichTextEdit), new PropertyMetadata(string.Empty, OnTextChanged));

    private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RichTextEdit element && e.NewValue != null)
        {
            if (!element._isUpdatedByUserInput)
            {
                var rtf = e.NewValue as string;

                if (!string.IsNullOrWhiteSpace(rtf))
                {
                    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(rtf));

                    element._richEdit?.LoadDocument(stream, DocumentFormat.Rtf);
                }
            } 
        }
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty NullTextProperty =
        DependencyProperty.Register(nameof(NullText), typeof(string), typeof(RichTextEdit), new PropertyMetadata(string.Empty));

    public string NullText
    {
        get => (string)GetValue(NullTextProperty);
        set => SetValue(NullTextProperty, value);
    }

    private PopupColorEdit? _colorEdit;
    private System.Windows.Controls.Primitives.ToggleButton? _fontBoldToggle;
    private ComboBoxEdit? _fontFamilyComboBox;
    private ComboBoxEdit? _fontSizeComboBox;
    private RichEditControl? _richEdit;

    private bool _isUpdatedByUserInput = false;

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
        //ClearEventHandler();

        base.OnApplyTemplate();

        // Template 내부 컨트롤 가져오기
        _richEdit = GetTemplateChild("RichEdit") as RichEditControl;
        _colorEdit = GetTemplateChild("ColorEdit") as PopupColorEdit;
        _fontBoldToggle = GetTemplateChild("FontBoldToggle") as System.Windows.Controls.Primitives.ToggleButton;
        _fontFamilyComboBox = GetTemplateChild("cmbFontFamiliy") as ComboBoxEdit;
        _fontSizeComboBox = GetTemplateChild("cmbFontSize") as ComboBoxEdit;

        _richEdit?.TextChanged += OnTextChanged_RichEdit;
        _richEdit?.AutoCorrect += OnAutoCorrect_RichEdit;
        _colorEdit?.EditValueChanged += OnEditValueChanged_ColorEdit;
        _fontBoldToggle?.Click += OnClick_ToggleButton;
        _fontFamilyComboBox?.EditValueChanged += OnEditValueChanged_FontFamily;
        _fontSizeComboBox?.EditValueChanged += OnEditValueChanged_FontSize;
    }

    private void OnClick_ToggleButton(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Primitives.ToggleButton element) return;
        if (_richEdit is null) return;

        DocumentRange range = _richEdit.Document.Selection;
        if (range.Length == 0) return;
        
        CharacterProperties properties = _richEdit.Document.BeginUpdateCharacters(range);

        try
        {
            properties.Bold = element.IsChecked.GetValueOrDefault(false);
        }
        finally
        {
            _richEdit.Document.EndUpdateCharacters(properties);
        }
    }

    private void OnTextChanged_RichEdit(object? sender, EventArgs e)
    {
        if (sender is not RichEditControl element) return;

        _isUpdatedByUserInput = true;

        SetValue(TextProperty, element.RtfText);

        _isUpdatedByUserInput = false;
    }

    private void OnAutoCorrect_RichEdit(object? sender, AutoCorrectEventArgs e)
    {
        if (_richEdit is null || _fontBoldToggle is null)
            return;

        if (e.AutoCorrectInfo.Text.Length <= 0)
            return;

        Document document = _richEdit.Document;

        int position = document.CaretPosition.ToInt() - 1;

        if (position < 0)
            return;

        DocumentRange range = document.CreateRange(position, 1);

        CharacterProperties properties =
            document.BeginUpdateCharacters(range);

        try
        {
            properties.Bold = _fontBoldToggle.IsChecked.GetValueOrDefault(false);
        }
        finally
        {
            document.EndUpdateCharacters(properties);
        }
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
