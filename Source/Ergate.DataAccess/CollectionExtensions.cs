namespace Ergate.DataAccess
{
    using System.Collections.Generic;

    internal static class CollectionExtensions
	{
		public static void AddIfNotExists<T>(this ICollection<T> list, T item)
		{
			if (!list.Contains(item))
				list.Add(item);
		}
	}
}