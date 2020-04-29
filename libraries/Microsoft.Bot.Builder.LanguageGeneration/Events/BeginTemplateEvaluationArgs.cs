namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class BeginTemplateEvaluationArgs : LGEventArgs
    {
        /// <summary>
        /// Gets or sets template name.
        /// </summary>
        /// <value>
        /// Template name.
        /// </value>
        public string TemplateName { get; set; }
    }
}
