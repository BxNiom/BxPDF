using System.IO.Compression;
using System.Text;
using BxPDF.IO;

namespace BxPDF;

public class PdfStream {
    private readonly List<StreamFilter> _filters = new List<StreamFilter>();

    public byte[] Data { get; set; }

    public PdfStream(byte[] rawData, params StreamFilter[] filters) {
        Data = rawData;
        _filters.AddRange(filters);
    }

    public void AddFilter(StreamFilter filter) {
        _filters.Insert(0, filter);
    }

    public void ASCIIHexEncode() {
        var buffer = new ByteBuffer();
        buffer.WriteString(BitConverter.ToString(Data).Replace("-", ""));
        Data = buffer.ToArray();
        AddFilter(StreamFilter.ASCIIHexDecode);
    }

    public void ASCII85Encode() {
        var ascii85 = new Ascii85();
        ascii85.PrefixMark = "";
        ascii85.LineLength = 0;
        Data = Encoding.ASCII.GetBytes(ascii85.Encode(Data));
        AddFilter(StreamFilter.ASCII85Decode);
    }

    public void FlateEncode() {
        using var ms = new MemoryStream();
        using var zip = new DeflateStream(ms, CompressionMode.Compress);
        zip.Write(Data);
        zip.Close();
        Data = ms.ToArray();
        AddFilter(StreamFilter.FlateDecode);
    }

    internal void Write(ByteBuffer buffer) {
        buffer.WriteLine($"/Length {Data.Length}");
        if (_filters.Count > 0) {
            if (_filters.Count > 1)
                buffer.WriteLine($"/Filter [{string.Join(' ', _filters.Select(f => $"/{f}"))}]");
            else
                buffer.WriteLine($"/Filter /{_filters[0]}");
        }

        buffer.WriteLine(">>");
        buffer.WriteLine("stream");
        buffer.Write(Data);
        buffer.Write((byte)0x0A);
        buffer.WriteLine("endstream");
    }
}