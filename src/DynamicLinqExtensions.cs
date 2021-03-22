using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Text.Json;

namespace Bizcon.Extensions
{
    [DynamicLinqType]
    public static class DynamicLinqTypeProvider
    {
        public static string GetPropertyGetString(JsonElement jsonElement, string name)
        {
            return jsonElement.GetProperty(name).GetString();
        }
    }
}
