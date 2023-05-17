using BxPDF.IO;

namespace BxPDF;

public abstract class PdfObject {
    public int Id { get; }
    public string Name { get; set; }

    protected PdfObject(int id, string name) {
        Id = id;
        Name = name;
    }

    internal void InternalWrite(ByteBuffer buffer) {
        buffer.WriteLine($"{Id} 0 obj");
        buffer.WriteLine("<<");
        if (WriteToPdf(buffer)) {
            buffer.WriteLine(">>");
            buffer.WriteLine("endobj");
        }
    }

    public string ReferenceString() {
        return $"{Id} 0 R";
    }

    protected abstract bool WriteToPdf(ByteBuffer buffer);
}