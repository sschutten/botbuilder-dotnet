namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class BeginExpressionEvaluationArgs : TemplatesEventArgs
    {
        public string Type { get; } = EventTypes.BeginExpressionEvaluation;

        /// <summary>
        /// Gets or sets expression string.
        /// </summary>
        /// <value>
        /// Expression string.
        /// </value>
        public string Expression { get; set; }
    }
}
