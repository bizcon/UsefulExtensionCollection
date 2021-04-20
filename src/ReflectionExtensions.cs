using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace bizconAG.Extensions
{
    public static class ReflectionExtensions
    {
        public static void SetPropertyValue(this object theObject, string name, object value, ILogger logger = null)
        {
            try
            {
                PropertyInfo property = theObject.GetType().GetProperty(name);
                if (null != property)
                {
                    Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    object safeValue = (value == null) ? null : Convert.ChangeType(value, type);
                    property.SetValue(theObject, safeValue, null);
                }
                else
                {
                    if (null != logger)
                        logger.LogError("Reflection.SetPropertyValue: property={0} not exists within object. value={1} not set", name, value);
                }
            }
            catch (Exception e)
            {
                if (null != logger)
                    logger.LogError(e, "Reflection.SetPropertyValue: property={0} not set to value={1}.message={2}", name, value, e.Message);
            }
        }

        public static object GetPropertyValue(this object theObject, string name, ILogger logger = null)
        {
            object ret = null;
            try
            {
                PropertyInfo property = theObject.GetType().GetProperty(name);
                if (null != property)
                {
                    ret = property.GetValue(theObject);
                }
                else
                {
                    if (null != logger)
                        logger.LogError("Reflection.SetPropertyValue: property={0} not exists within object.ret=null", name);
                }
            }
            catch (Exception e)
            {
                if (null != logger)
                    logger.LogError(e, "Reflection.SetPropertyValue: error getting property={0} ret=null.message={2}", name, e.Message);
            }
            return ret;
        }
    }
}
