namespace SampleOrg.Foo.Website.Models;

public record ColorEntry(int Id, string Name, string Slug, string Category, int Hue, string Hex)
{
    public byte R => Convert.ToByte(Hex[1..3], 16);
    public byte G => Convert.ToByte(Hex[3..5], 16);
    public byte B => Convert.ToByte(Hex[5..7], 16);
}
