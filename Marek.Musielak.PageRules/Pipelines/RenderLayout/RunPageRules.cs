using System;
using Marek.Musielak.PageRules.Rules.RuleContext;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;
using Sitecore.Rules;

namespace Marek.Musielak.PageRules.Pipelines.RenderLayout
{
    public class RunPageRules
    {
        private const string PageRulesFieldName = "Page Rules";

        public void Process(PipelineArgs args)
        {
            try
            {
                var rulesField = Context.Item?.Fields[PageRulesFieldName];

                if (rulesField == null || string.IsNullOrWhiteSpace(rulesField.Value))
                    return;

                var rules = RuleFactory.GetRules<PageRulesRuleContext>(rulesField);

                if (rules == null || rules.Count == 0)
                    return;

                var ruleContext = new PageRulesRuleContext();
                rules.Run(ruleContext);
            }
            catch (Exception exc)
            {
                Log.Error("Exception while running page rules", exc, this);
            }
        }
    }
}