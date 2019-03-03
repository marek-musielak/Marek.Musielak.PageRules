using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Text;

namespace Marek.Musielak.PageRules.SpeakRequests
{
    public class GeneratePageRulesEditorUrl : PipelineProcessorRequest<ItemContext>
    {
        private string GenerateUrl()
        {
            var fieldList = CreateFieldDescriptors(RequestContext.Argument);
            var fieldEditorOptions = new FieldEditorOptions(fieldList) {SaveItem = true};
            return fieldEditorOptions.ToUrlString().ToString();
        }

        private List<FieldDescriptor> CreateFieldDescriptors(string fields)
        {
            var fieldList = new List<FieldDescriptor>();

            foreach (string field in new ListString(fields))
                fieldList.Add(new FieldDescriptor(RequestContext.Item, field));

            return fieldList;
        }

        public override PipelineProcessorResponseValue ProcessRequest()
        {
            return new PipelineProcessorResponseValue
            {
                Value = GenerateUrl()
            };
        }
    }
}