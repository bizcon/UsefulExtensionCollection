using ServiceStack;

namespace Bizcon.Extensions
{
    public static class LogExtensions
    {
        public static string LineDump(this object dumpObject)
        {
            return dumpObject.SerializeToString();
        }
    }
}
