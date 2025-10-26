using PdfSharpCore.Fonts;

namespace Shops.Application.Resources.Fonts;

public class CustomFontResolver : IFontResolver
{
    public static readonly CustomFontResolver Instance = new();

    public string DefaultFontName => "Arial";

    public byte[] GetFont(string faceName)
    {
        var basePath = Path.Combine(AppContext.BaseDirectory, "Resources", "Fonts");

        if (faceName.EndsWith("Bold"))
            return File.ReadAllBytes(Path.Combine(basePath, "ARIALBD.TTF"));

        return File.ReadAllBytes(Path.Combine(basePath, "ARIAL.TTF"));
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        var name = "Arial";
        if (isBold)
            name += "-Bold";
        return new FontResolverInfo(name);
    }
}
