namespace PhysiXSharp.Core.Utility;

public static class ListExtension
{
    public static void RemoveRange<T>(this List<T> list, List<T> items)
    {
        foreach (T item in items)
        {
            list.Remove(item);
        }
    }
}