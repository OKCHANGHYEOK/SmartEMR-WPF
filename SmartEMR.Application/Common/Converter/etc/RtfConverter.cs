using DevExpress.XtraRichEdit;

namespace SmartEMR.Application.Common.Converter.etc;

public class RtfConverter
{
    public static string ConvertRtfToPlainText(string rtfText)
    {
        if (string.IsNullOrWhiteSpace(rtfText))
        {
            return string.Empty;
        }

        using (RichEditDocumentServer server = new RichEditDocumentServer())
        {
            // Load the RTF text
            server.RtfText = rtfText;

            // Return the plain text representation
            return server.Text;
        }
    }
}
