using BxPDF.IO;

namespace BxPDF;

public class PdfMetadata : PdfObject {
    private const string DateTimeFormat = "yyyyMMddHHmmss";

    private PdfMetadata(int id, string name) : base(id, name) {
        CreationDate = DateTime.Now;
        ModDate = DateTime.Now;
    }

    public string? Title { get; set; }
    public string? Author { get; set; }
    public string? Subject { get; set; }
    public string? Keywords { get; set; }
    public string? Creator { get; set; }
    public string? Producer { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime ModDate { get; set; }

    protected override bool WriteToPdf(ByteBuffer buffer) {
        if (Title != null) buffer.WriteLine($"/Title ({Title})");
        if (Author != null) buffer.WriteLine($"/Author ({Author})");
        if (Subject != null) buffer.WriteLine($"/Subject ({Subject})");
        if (Keywords != null) buffer.WriteLine($"/Keywords ({Keywords})");
        if (Creator != null) buffer.WriteLine($"/Creator ({Creator})");
        if (Producer != null) buffer.WriteLine($"/Producer ({Producer})");
        buffer.WriteLine($"/CreationDate (D:{CreationDate.ToUniversalTime().ToString(DateTimeFormat)}Z)");
        buffer.WriteLine($"/ModDate (D:{ModDate.ToUniversalTime().ToString(DateTimeFormat)}Z)");

        return true;
    }
}