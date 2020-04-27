namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class BeginTemplateEvaluationArgs : TemplatesEventArgs
    {
        public string Type { get; } = EventTypes.BeginTemplateEvaluation;

        /// <summary>
        /// Gets or sets template name.
        /// </summary>
        /// <value>
        /// Template name.
        /// </value>
        public string TemplateName { get; set; }
    }
}
