namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class BeginExpressionEvaluationArgs : LGEventArgs
    {
        /// <summary>
        /// Gets or sets expression string.
        /// </summary>
        /// <value>
        /// Expression string.
        /// </value>
        public string Expression { get; set; }
    }
}
