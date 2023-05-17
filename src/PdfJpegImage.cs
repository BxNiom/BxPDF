namespace BxPDF;

public class PdfJpegImage : PdfImage {

    private PdfJpegImage(int id, string name) : base(id, name) {
    }

    protected override void OnStreamCreated() {
        Stream?.AddFilter(StreamFilter.DCTDecode);
    }
}