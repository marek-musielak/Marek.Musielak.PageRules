using System.Web;
using Marek.Musielak.PageRules.Rules.RuleContext;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules.Actions;

namespace Marek.Musielak.PageRules.Rules.Actions
{
    public class EndResponseWithStatusCodeAction<T> : RuleAction<T> where T : PageRulesRuleContext
    {
        private const string StatusCodeFieldName = "HTTP Status Code"; 

        public ID StatusCodeId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (ID.IsNullOrEmpty(StatusCodeId))
            {
                Log.Warn($"EndResponseWithStatusCodeAction for page '{Context.Item.Paths.FullPath}' - no status selected", this);
                return;
            }

            var statusCodeItem = Context.Database.GetItem(StatusCodeId);

            if (statusCodeItem == null)
            {
                Log.Warn($"EndResponseWithStatusCodeAction for page '{Context.Item.Paths.FullPath}' - status code item does not exist", this);
                return;
            }

            HttpContext.Current.Response.StatusCode = MainUtil.GetInt(statusCodeItem[StatusCodeFieldName], 200);
            HttpContext.Current.Response.End();
        }
    }
}