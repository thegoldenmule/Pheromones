public static class ArrayExtensions
{
    public static int IndexOf<T>(this T[] @this, T element)
    {
        for (int i = 0, len = @this.Length; i < len; i++)
        {
            if (@this[i].Equals(element))
            {
                return i;
            }
        }

        return -1;
    }

    public static void Fill<T>(this T[] @this, T element)
    {
        for (int i = 0, len = @this.Length; i < len; i++)
        {
            @this[i] = element;
        }
    }
}