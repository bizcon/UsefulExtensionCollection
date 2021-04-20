using System;
using System.Linq;
using System.Reflection;

namespace bizconAG.Extensions
{
    public static class MergeExtensions
    {
        public static B MergeType<B, T>(this B baseObject, T mergeObject, String[] properties = null, String[] omitProperties = null, bool omitNullValues = true)
        {
            var baseProperties = typeof(B).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var mergeProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var mergeProperty in mergeProperties)
            {
                Type PropertyType = Nullable.GetUnderlyingType(mergeProperty.PropertyType) ?? mergeProperty.PropertyType;
                string PropertyName = mergeProperty.Name;
                object mergePropertyValue = mergeProperty.GetValue(mergeObject);

                if (null != mergePropertyValue || !omitNullValues)
                {
                    foreach (var baseProperty in baseProperties)
                    {
                        if (mergeProperty.Name.Equals(baseProperty.Name) &&
                            (null == omitProperties || !omitProperties.Contains(baseProperty.Name)) &&
                            (null == properties || properties.Contains(baseProperty.Name)))
                        {
                            baseProperty.SetValue(baseObject, mergePropertyValue, null);
                        }
                    }
                }
            }
            return baseObject;
        }
    }
}
