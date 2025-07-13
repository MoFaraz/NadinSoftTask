using System.Text.RegularExpressions;

namespace NadinSoft.WebFramework.Extensions;

public static class RegexHelpers
{
   public static bool MatchesApiVersion(string apiVersion, string version)
   {
      string pattern = $@"(?<=\/|^){Regex.Escape(apiVersion)}(?=\/|$)";
      Regex regex = new Regex(pattern, RegexOptions.Compiled);
      
      return regex.IsMatch(version);
   }
}