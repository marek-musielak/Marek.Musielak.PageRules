using System.Web;
using Marek.Musielak.PageRules.Rules.RuleContext;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Rules.Actions;

namespace Marek.Musielak.PageRules.Rules.Actions
{
    public class RedirectAndChangeLanguageAction<T> : RuleAction<T> where T : PageRulesRuleContext
    {
        public ID TargetId { get; set; }
        public ID LanguageId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (ID.IsNullOrEmpty(TargetId))
            {
                Log.Warn($"RedirectAndChangeLanguageAction for page '{Context.Item.Paths.FullPath}' - no item selected", this);
                return;
            }

            if (ID.IsNullOrEmpty(LanguageId))
            {
                Log.Warn($"RedirectAndChangeLanguageAction for page '{Context.Item.Paths.FullPath}' - no language selected", this);
                return;
            }

            var db = Context.Database;

            var targetLanguage = db.GetItem(LanguageId);
            if (targetLanguage != null)
            {
                if (Language.TryParse(targetLanguage.Name, out var language))
                {
                    using (new LanguageSwitcher(language))
                    {
                        var item = db.GetItem(TargetId);
                        if (item?.Versions?.Count > 0)
                        {
                            var options = UrlOptions.DefaultOptions;
                            options.LanguageEmbedding = LanguageEmbedding.Always;
                            var itemUrl = LinkManager.GetItemUrl(item, options);

                            HttpContext.Current.Response.Redirect(itemUrl, true);
                        }
                        else
                        {
                            Log.Warn($"RedirectAndChangeLanguageAction - page '{Context.Item.Paths.FullPath}' doesn' have any version in '{targetLanguage.Name}' language", this);
                        }
                    }
                }
                else
                {
                    Log.Warn($"RedirectAndChangeLanguageAction - language '{targetLanguage.Name}' does not exist", this);
                }
            }
            else
            {
                Log.Warn($"RedirectAndChangeLanguageAction for page '{Context.Item.Paths.FullPath}' executed without target language", this);
            }
        }
    }
}