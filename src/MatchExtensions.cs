using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace bizconAG.Extensions
{
    public static class MatchExtensions
    {
        public static string ReplaceMatches(this MatchCollection matches, string source, string replacement)
        {
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                source = match.ReplaceMatch(source, replacement);
            }
            return source;
        }

        public static string ReplaceMatch(this Match match, string source, string replacement)
        {
            return source.Substring(0, match.Index) + replacement + source.Substring(match.Index + match.Length);
        }

        public static string ReplaceMatchesGroups(this MatchCollection matches, string source, string replacement, string[] groups)
        {
            foreach (var match in matches.Cast<Match>().Reverse())
            {
                source = match.ReplaceMatchGroups(source, replacement, groups);
            }
            return source;
        }

        public static string ReplaceMatchGroups(this Match match, string source, string replacement, string[] groups)
        {
            foreach (var group in match.Groups.Cast<Group>().Reverse())
            {
                if (null == groups || groups.Contains(group.Name))
                {
                    source = source.Substring(0, group.Index) + replacement + source.Substring(group.Index + group.Length);
                }
            }
            return source;
        }
    }
}
