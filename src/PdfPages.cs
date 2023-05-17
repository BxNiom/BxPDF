using BxPDF.IO;

namespace BxPDF;

public class PdfPages : PdfObject {
    private readonly List<PdfPage> _pages = new List<PdfPage>();

    public IReadOnlyList<PdfPage> Pages => _pages;
    public PdfPage? FirstPage => _pages.FirstOrDefault();

    private PdfPages(int id, string name) : base(id, name) {
    }

    public void AddPage(PdfPage page) {
        _pages.Add(page);
        page.Pages = this;
    }

    protected override bool WriteToPdf(ByteBuffer buffer) {
        buffer.WriteLine("/Type /Pages");
        buffer.WriteLine($"/Kids [ {string.Join(' ', _pages.Select(p => p.ReferenceString()))} ]");
        buffer.WriteLine($"/Count {_pages.Count}");

        return true;
    }
}