using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Bizcon.Extensions
{
    public static class FilterExtensions
    {
        public static bool FilterApplies<B>(this B filterObject, string filter, ILogger logger = null)
        {
            bool ret = true;
            if (!string.IsNullOrEmpty(filter))
            {
                ret = new List<B>() { filterObject }.AsQueryable().Where(filter).Select(x => x).Count().Equals(1);
            }

            if (null != logger)
                logger.LogDebug("Filtering.FilterApplies: filter={0}.return={1}", filter, ret);

            return ret;
        }
    }
}
