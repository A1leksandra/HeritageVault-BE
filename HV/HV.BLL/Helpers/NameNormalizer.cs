namespace HV.BLL.Helpers;

public static class NameNormalizer
{
    public static string Normalize(string name)
    {
        return string.Join(" ", name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .ToUpperInvariant();
    }
}

