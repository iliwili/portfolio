namespace Portfolio.Api.Errors;

public static class FieldPath
{
    public static string Normalize(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return propertyName;

        var parts = propertyName.Split('.', StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < parts.Length; i++)
            parts[i] = ToCamelPreservingIndexer(parts[i]);

        return string.Join('.', parts);
    }

    private static string ToCamelPreservingIndexer(string s)
    {
        var idx = s.IndexOf('[');
        var head = idx >= 0 ? s[..idx] : s;
        var tail = idx >= 0 ? s[idx..] : "";

        if (string.IsNullOrEmpty(head))
            return s;

        return char.ToLowerInvariant(head[0]) + head[1..] + tail;
    }
}