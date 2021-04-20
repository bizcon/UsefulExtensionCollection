using ServiceStack;

namespace bizconAG.Extensions
{
    public static class LogExtensions
    {
        public static string LineDump(this object dumpObject)
        {
            return dumpObject.SerializeToString();
        }
    }
}
