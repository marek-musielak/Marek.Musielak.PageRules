using System.Runtime.Serialization;

namespace Marek.Musielak.PageRules.Rules.RuleContext
{
    public class PageRulesRuleContext : Sitecore.Rules.RuleContext, ISerializable
    {
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
        }
    }
}