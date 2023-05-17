using BxPDF.IO;

namespace BxPDF;

public abstract class PdfContent : PdfObject {

    protected PdfContent(int id, string name) : base(id, name) {
    }

    protected abstract string GetContent();

    protected override bool WriteToPdf(ByteBuffer buffer) {
        var content = GetContent();

        buffer.WriteLine($"/Length {content.Length}");
        buffer.WriteLine(">>");
        buffer.WriteLine("stream");
        buffer.WriteLine(content);
        buffer.WriteLine("endstream");
        buffer.WriteLine("endobj");

        return false;
    }
}