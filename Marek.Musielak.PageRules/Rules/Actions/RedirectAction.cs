using System.Web;
using Marek.Musielak.PageRules.Rules.RuleContext;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Rules.Actions;

namespace Marek.Musielak.PageRules.Rules.Actions
{
    public class RedirectAction<T> : RuleAction<T> where T : PageRulesRuleContext
    {
        public ID TargetId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (ID.IsNullOrEmpty(TargetId))
            {
                Log.Warn($"RedirectAction for page '{Context.Item.Paths.FullPath}' - no item selected", this);
                return;
            }

            var target = Sitecore.Context.Database.GetItem(TargetId);
            if (target != null)
            {
                HttpContext.Current.Response.Redirect(LinkManager.GetItemUrl(target), true);
            }
            else
            {
                Log.Warn($"RedirectAction for page '{Context.Item.Paths.FullPath}' executed without target item", this);
            }
        }
    }
}