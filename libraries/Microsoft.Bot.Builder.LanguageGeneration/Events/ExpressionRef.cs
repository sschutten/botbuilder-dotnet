namespace Microsoft.Bot.Builder.LanguageGeneration
{
    /// <summary>
    /// Expression container with source range.
    /// </summary>
    public class ExpressionRef
    {
        public ExpressionRef(string expression, SourceRange sourceRange)
        {
            this.Expression = expression;
            this.SourceRange = sourceRange;
        }

        /// <summary>
        /// Gets or sets expression string.
        /// </summary>
        /// <value>
        /// Expression string.
        /// </value>
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets expression source range.
        /// </summary>
        /// <value>
        /// Expression source range.
        /// </value>
        public SourceRange SourceRange { get; set; }

        public override string ToString()
        {
            return Expression;
        }

        /// <summary>
        /// Get the unique id of erxpression context.
        /// </summary>
        /// <returns>id string.</returns>
        public string GetId()
        {
            return SourceRange?.Source + ":" + SourceRange?.Range + ":" + Expression;
        }
    }
}
