using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace CampaignWikiLibrary1.Extensions
{
    public static class ExtensionMethods
    {
        public static void Use<T>(this T source, Action<T> action) where T : IDisposable
        {
            using (source)
            {
                action(source);
            }
        }
        public static bool TryFirst<T>(this T source, out T result) where T : IEnumerable<T>
        {
            result = source.FirstOrDefault();
            return result == null;
        }
    }
}
