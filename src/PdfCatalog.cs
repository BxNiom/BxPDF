using BxPDF.IO;

namespace BxPDF;

public class PdfCatalog : PdfObject {
    public PdfPages? Pages { get; internal set; }

    private PdfCatalog(int id, string name) : base(id, name) {
    }

    protected override bool WriteToPdf(ByteBuffer buffer) {
        buffer.WriteLine("/Type /Catalog");
        buffer.WriteLine($"/Pages {Pages?.ReferenceString() ?? throw new InvalidDataException()}");
        return true;
    }
}