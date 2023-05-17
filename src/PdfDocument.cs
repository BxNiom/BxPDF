using System.Reflection;
using BxPDF.IO;

namespace BxPDF;

public class PdfDocument {
    private readonly List<PdfObject> _objects = new List<PdfObject>();

    public PdfMetadata Metadata { get; }
    public PdfPages Pages { get; }
    public PdfCatalog Catalog { get; }

    public PdfDocument() {
        Metadata = CreateObject<PdfMetadata>("META")!;
        Catalog = CreateObject<PdfCatalog>("CATALOG")!;
        Pages = CreateObject<PdfPages>("PAGES")!;

        Catalog.Pages = Pages;
    }

    public T? FindObject<T>(string name) where T : PdfObject {
        return (T?)(from o in _objects
                    where o.GetType() == typeof(T) && o.Name.Equals(name, StringComparison.Ordinal)
                    select o).FirstOrDefault();
    }

    public T? FindObject<T>(int id) where T : PdfObject {
        return (T?)(from o in _objects
                    where o.GetType() == typeof(T) && o.Id == id
                    select o).FirstOrDefault();
    }

    public T? CreateObject<T>(string name) where T : PdfObject {
        var ctor = typeof(T).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null, CallingConventions.HasThis, new[] { typeof(int), typeof(string) }, null) ?? throw new InvalidDataException();

        var obj = ctor.Invoke(new object?[] { _objects.Count + 1, name });
        var pdfObj = (T)obj ?? throw new InvalidDataException();
        _objects.Add(pdfObj);

        return pdfObj;
    }

    public void SaveToFile(string filename) {
        var buffer = new ByteBuffer();

        var xref = new List<long>();

        buffer.WriteLine("%PDF-1.4");

        foreach (var obj in _objects) {
            xref.Add(buffer.Position);
            obj.InternalWrite(buffer);
        }

        // Write xref table
        var xrefStart = buffer.Position;
        buffer.WriteLine("xref");
        buffer.WriteLine($"0 {xref.Count + 1}");
        buffer.WriteLine("0000000000 65535 f ");
        foreach (var pos in xref) {
            buffer.WriteLine($"{pos.ToString().PadLeft(10, '0')} 00000 n ");
        }

        // Write trailer
        buffer.WriteLine("trailer");
        buffer.WriteLine("<<");
        buffer.WriteLine($"/Size {xref.Count + 1}");
        buffer.WriteLine($"/Info {Metadata.ReferenceString()}");
        buffer.WriteLine($"/Root {Catalog.ReferenceString()}");
        buffer.WriteLine(">>");
        buffer.WriteLine("startxref");
        buffer.WriteLine(xrefStart.ToString());
        buffer.WriteLine("%%EOF");

        File.WriteAllBytes(filename, buffer.ToArray());
    }
}