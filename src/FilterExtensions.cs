using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace bizconAG.Extensions
{
    public static class FilterExtensions
    {
        public static Dictionary<Type, Func<string, string>> RegisteredTypedFilterPatcher = new()
        {
            { typeof(JsonElement), JsonElementFilterPatcher }
        };

        public static string JsonElementFilterPatcher(string filter)
        {
            string pattern = @"\b(?<!(""|\.))(?<name>\w+)(?=(=|\.|\b))\b";
            MatchCollection mc = Regex.Matches(filter, pattern);
            foreach (Match m in mc.Reverse())
            {
                string replacement = $"DynamicLinqTypeProvider.GetPropertyGetString(x, \"{m.Groups[0]}\")";
                filter = filter.Remove(m.Index, m.Groups[0].Length).Insert(m.Index, replacement);
            }
            return $"x => {filter}";
        }

        public static bool FilterApplies<B>(this B filterObject, string filter, ILogger logger = null)
        {
            bool ret = true;
            if (RegisteredTypedFilterPatcher.ContainsKey(typeof(B)))
            {
                filter = RegisteredTypedFilterPatcher[typeof(B)](filter);
            }
            if (!string.IsNullOrEmpty(filter))
            {
                ret = false;
                try
                {
                    ret = new List<B>() { filterObject }.AsQueryable().Where(filter).Select(x => x).Count().Equals(1);
                }
                catch (Exception e)
                {
                    logger?.LogError(e, "Filtering.FilterApplies: filter={0}.message={1}", filter, e.Message);
                }
            }

            logger?.LogDebug("Filtering.FilterApplies: filter={0}.return={1}", filter, ret);

            return ret;
        }
        public static bool DynamicFilterApplies(dynamic filterObject, string filter, ILogger logger = null)
        {
            bool ret = true;
            if (!string.IsNullOrEmpty(filter))
            {
                ret = false;
                try
                {
                    ret = new List<dynamic>() { filterObject }.AsQueryable().Where(filter).Select(x => x).Count().Equals(1);
                }
                catch (Exception e)
                {
                    logger?.LogError(e, "Filtering.ExpandoFilterApplies: filter={0}.message={1}", filter, e.Message);
                }
            }

            if (null != logger)
                logger?.LogDebug("Filtering.ExpandoFilterApplies: filter={0}.return={1}", filter, ret);

            return ret;
        }
    }
}
