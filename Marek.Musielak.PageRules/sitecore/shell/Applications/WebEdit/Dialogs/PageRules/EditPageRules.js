define(["sitecore", "/-/speak/v1/ExperienceEditor/ExperienceEditor.js"], function (Sitecore, ExperienceEditor) {
    Sitecore.Commands.EditPageRules =
    {
        canExecute: function (context) {
            return true;
        },
        execute: function (context) {
            context.currentContext.argument = context.button.viewModel.$el[0].title;

            ExperienceEditor.PipelinesUtil.generateRequestProcessor("ExperienceEditor.GeneratePageRulesEditorUrl", function (response) {
                var dialogUrl = response.responseValue.value;
                var dialogFeatures = "header: Edit Page Rules; dialogHeight: 350px;dialogWidth: 740px; edge:raised; center:yes; help:no; resizable:yes; status:no; scroll:no";
                ExperienceEditor.Dialogs.showModalDialog(dialogUrl, '', dialogFeatures, null);
            }).execute(context);

        }
    };
});