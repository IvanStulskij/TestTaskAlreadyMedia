using System.Diagnostics.CodeAnalysis;

namespace TestTaskAlreadyMedia.Core.Helpers;

public class StringsCustomComparer : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
    {
        return x.Equals(y, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode([DisallowNull] string obj)
    {
        return obj.GetHashCode();
    }
}
