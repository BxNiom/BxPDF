using BxPDF.IO;

namespace BxPDF;

public abstract class PdfImage : PdfObject {
    private PdfStream? _stream;

    public byte[]? ImageData {
        get {
            return _stream?.Data;
        }
        set {
            if (_stream == null && value != null) {
                _stream = new PdfStream(value);
                OnStreamCreated();
            }
        }
    }

    public PdfStream? Stream => _stream;

    public int Width { get; set; }
    public int Height { get; set; }
    public int BitsPerComponent { get; set; }
    public ColorSpace ColorSpace { get; set; }

    protected PdfImage(int id, string name) : base(id, name) {
        Width = 0;
        Height = 0;
        BitsPerComponent = 8;
        ColorSpace = ColorSpace.RGB;
    }

    protected override bool WriteToPdf(ByteBuffer buffer) {
        buffer.WriteLine("/Type /XObject /Subtype /Image");
        buffer.WriteLine($"/Width {Width}");
        buffer.WriteLine($"/Height {Height}");
        buffer.WriteLine($"/ColorSpace /Device{ColorSpace}");
        buffer.WriteLine($"/BitsPerComponent {BitsPerComponent}");

        _stream?.Write(buffer);

        buffer.WriteLine("endobj");

        return false;
    }

    protected virtual void OnStreamCreated() {
    }
}