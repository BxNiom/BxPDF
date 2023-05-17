using System.IO;
using System.Text;

namespace BxPDF.IO;

internal static class BinaryWriterEx
{

    /// <summary>
    /// Writes the string without length prefix.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The value.</param>
    /// <param name="encoding">The encoding, if null ASCII will be used</param>
    public static void WriteString(this BinaryWriter writer, string value, Encoding? encoding = null, bool writeLength = false)
    {
        encoding ??= Encoding.ASCII;
        var bytes = encoding.GetBytes(value);

        if (writeLength)
            writer.Write(bytes.Length);

        writer.Write(bytes);
    }

    public static void WriteLine(this BinaryWriter writer, string value, Encoding? encoding = null, bool writeLength = false)
    {
        writer.WriteString(value + "\n", encoding, writeLength);
    }
}