using System;
using System.Collections.Generic;
using System.Reflection;

namespace bizconAG.Extensions
{
    public static class CompareExtensions
    {
        static readonly Dictionary<Type, object> cache = new();

        public static object GetDefaultValue(Type t)
        {
            if (!t.IsValueType)
            {
                return null;
            }
            if (cache.TryGetValue(t, out object ret))
            {
                return ret;
            }
            ret = Activator.CreateInstance(t);
            cache[t] = ret;
            return t;
        }

        public class Variance
        {
            public Type PropertyType { get; set; }
            public string PropertyName { get; set; }
            public object Default { get; set; }
            public object Object1 { get; set; }
            public object Object2 { get; set; }
        }

        public static List<Variance> CompareGetVariance<T>(this T compareObject, T withObject)
        {
            var variances = new List<Variance>();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var variance = new Variance
                {
                    PropertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType,
                    PropertyName = property.Name,
                    Object1 = property.GetValue(compareObject),
                    Object2 = property.GetValue(withObject)
                };
                variance.Default = GetDefaultValue(variance.PropertyType);

                if ((null == variance.Object1 || variance.Object1 == variance.Default)
                    && (null == variance.Object2 || variance.Object2 == variance.Default))
                {
                    continue;
                }
                if (((null == variance.Object1 || variance.Object1 == variance.Default) && null != variance.Object2 && variance.Object2 != variance.Default)
                    || (null != variance.Object1 && variance.Object1 != variance.Default && (null == variance.Object2 || variance.Object2 == variance.Default)))
                {
                    variances.Add(variance);
                    continue;
                }
                if (null != variance.Object1 && null != variance.Object2 && !variance.Object1.Equals(variance.Object2))
                {
                    variances.Add(variance);
                }
            }
            return variances;
        }

        public static List<Variance> CompareGetVarianceDifferentTypes<C, W>(this C compareObject, W withObject)
        {
            var variances = new List<Variance>();
            var properties = typeof(C).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var withProperties = typeof(W).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var variance = new Variance
                {
                    PropertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType,
                    PropertyName = property.Name,
                    Object1 = property.GetValue(compareObject)
                };
                variance.Default = GetDefaultValue(variance.PropertyType);
                variance.Object2 = GetDefaultValue(variance.PropertyType);

                foreach (var withProperty in withProperties)
                {
                    if (property.Name.Equals(withProperty.Name))
                    {
                        variance.Object2 = withProperty.GetValue(withObject);
                    }
                }

                if (variance.Object1 == variance.Default && variance.Object2 == variance.Default)
                {
                    continue;
                }
                if ((variance.Object1 == variance.Default && variance.Object2 != variance.Default) || (variance.Object1 != variance.Default && variance.Object2 == variance.Default))
                {
                    variances.Add(variance);
                    continue;
                }
                if (!variance.Object1.Equals(variance.Object2))
                {
                    variances.Add(variance);
                }
            }
            return variances;
        }

        public static T CompareGetObject<T>(this T compareObject, T withObject) where T : new()
        {
            T returnObject = new();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var defaultValue = GetDefaultValue(Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                var sourceObjectValue = property.GetValue(compareObject);
                var newObject2Value = property.GetValue(withObject);

                PropertyInfo prop = returnObject.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop && prop.CanWrite)
                {
                    if (sourceObjectValue == defaultValue && newObject2Value == defaultValue)
                    {
                        continue;
                    }
                    if ((sourceObjectValue == defaultValue && newObject2Value != defaultValue) || (sourceObjectValue != defaultValue && newObject2Value == defaultValue))
                    {
                        prop.SetValue(returnObject, newObject2Value, null);
                        continue;
                    }
                    if (!sourceObjectValue.Equals(newObject2Value))
                    {
                        prop.SetValue(returnObject, newObject2Value, null);
                    }
                }
            }
            return returnObject;
        }

        public static Tuple<T, T> CompareGetObjects<T>(this T compareObject, T withObject) where T : new()
        {
            T returnObject1 = new();
            T returnObject2 = new();

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                var defaultValue = GetDefaultValue(Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);
                var object1Value = property.GetValue(compareObject);
                var object2Value = property.GetValue(withObject);

                PropertyInfo prop1 = returnObject1.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo prop2 = returnObject2.GetType().GetProperty(property.Name, BindingFlags.Public | BindingFlags.Instance);
                if (null != prop1 && prop1.CanWrite && null != prop2 && prop2.CanWrite)
                {
                    if (object1Value == defaultValue && object2Value == defaultValue)
                    {
                        continue;
                    }
                    if (
                        (object1Value == defaultValue && object2Value != defaultValue)
                        ||
                        (object1Value != defaultValue && object2Value == defaultValue)
                    )
                    {
                        prop1.SetValue(returnObject1, object1Value, null);
                        prop2.SetValue(returnObject2, object2Value, null);

                        continue;
                    }
                    if (!object1Value.Equals(object2Value))
                    {
                        prop1.SetValue(returnObject1, object1Value, null);
                        prop2.SetValue(returnObject2, object2Value, null);
                    }
                }
            }
            return new Tuple<T, T>(returnObject1, returnObject2);
        }
    }
}
