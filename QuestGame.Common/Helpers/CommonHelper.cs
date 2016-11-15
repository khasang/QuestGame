using System.Configuration;

namespace QuestGame.Common.Helpers
{
    public class CommonHelper
    {
        public static string GetParamOrDefaultValue(string paramKey, string paramVale)
        {
            return ConfigurationManager.AppSettings[paramKey] ?? paramVale;
        }
    }
}