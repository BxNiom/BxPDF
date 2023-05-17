using BxPDF.IO;

namespace BxPDF;

public class PdfPage : PdfObject {
    public List<PdfObject> Resources { get; }
    public PdfContent? Content { get; set; }
    public PdfPages? Pages { get; internal set; }

    public double Top { get; set; }
    public double Left { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }

    private PdfPage(int id, string name) : base(id, name) {
        Resources = new List<PdfObject>();
        Content = null;

        Top = 0;
        Left = 0;
        Width = 595.20;
        Height = 841.92;
    }

    protected override bool WriteToPdf(ByteBuffer buffer) {
        buffer.WriteLine("/Type /Page");
        buffer.WriteLine($"/Parent {Pages?.ReferenceString() ?? throw new InvalidDataException()}");
        buffer.WriteLine($"/MediaBox [ {Top} {Left} {Width} {Height} ]".Replace(',', '.'));

        buffer.WriteString("/Resources << ");
        if (Resources.Count > 0) {
            var res = string.Join(' ', Resources.Select(p => $"/{p.Name} {p.ReferenceString()}"));
            buffer.WriteString($"/XObject << {res} >> ");
        }
        buffer.WriteLine(">>");

        if (Content != null)
            buffer.WriteLine($"/Contents {Content?.ReferenceString() ?? throw new InvalidDataException()}");

        return true;
    }
}