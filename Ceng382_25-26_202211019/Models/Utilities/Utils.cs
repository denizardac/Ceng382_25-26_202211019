using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;

namespace MyRazorApp.Utilities
{
    public sealed class Utils
    {
        private static readonly Utils _instance = new Utils();
        public static Utils Instance => _instance;
        private Utils() { }

        public string ExportToJson<T>(IEnumerable<T> data, List<string>? selectedColumns = null)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            bool hasSelection = selectedColumns?.Count > 0;
            var list = new List<Dictionary<string, object>>();
            foreach (var item in data)
            {
                var dict = new Dictionary<string, object>();
                foreach (var prop in item!.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (hasSelection && !selectedColumns!.Contains(prop.Name)) continue;
                    dict[prop.Name] = prop.GetValue(item) ?? "";
                }
                list.Add(dict);
            }
            return JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
