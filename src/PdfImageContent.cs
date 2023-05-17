using System.Text;

namespace BxPDF;

public class PdfImageContent : PdfContent {
    public PdfImage? Image { get; set; }

    public double Width { get; set; }
    public double Height { get; set; }

    private PdfImageContent(int id, string name) : base(id, name) {
        Width = 595.2;
        Height = 841.92;
    }

    protected override string GetContent() {
        var sb = new StringBuilder();
        sb.AppendLine("q");
        sb.AppendLine($"{Width:0.000000} 0 0 {Height:0.000000} 0 0 cm".Replace(',', '.'));
        sb.AppendLine($"/{Image?.Name ?? throw new InvalidDataException()} Do");
        sb.Append("Q");
        return sb.ToString();
    }
}