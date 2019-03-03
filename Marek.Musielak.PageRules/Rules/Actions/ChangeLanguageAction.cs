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
    public class ChangeLanguageAction<T> : RuleAction<T> where T : PageRulesRuleContext
    {
        public ID LanguageId { get; set; }

        public override void Apply(T ruleContext)
        {
            if (ID.IsNullOrEmpty(LanguageId))
            {
                Log.Warn($"ChangeLanguageAction for page '{Context.Item.Paths.FullPath}' - no language selected", this);
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
                        var item = db.GetItem(Context.Item.ID);
                        if (item?.Versions?.Count > 0)
                        {
                            var options = UrlOptions.DefaultOptions;
                            options.LanguageEmbedding = LanguageEmbedding.Always;
                            var itemUrl = LinkManager.GetItemUrl(item, options);

                            HttpContext.Current.Response.Redirect(itemUrl, true);
                        }
                        else
                        {
                            Log.Warn($"ChangeLanguageAction - page '{Context.Item.Paths.FullPath}' doesn' have any version in '{targetLanguage.Name}' language", this);
                        }
                    }
                }
                else
                {
                    Log.Warn($"ChangeLanguageAction - language '{targetLanguage.Name}' does not exist", this);
                }
            }
            else
            {
                Log.Warn($"ChangeLanguageAction for page '{Context.Item.Paths.FullPath}' executed without target language", this);
            }
        }
    }
}