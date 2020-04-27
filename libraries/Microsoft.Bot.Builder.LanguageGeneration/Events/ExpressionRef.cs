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

        public string Expression { get; set; }

        public SourceRange SourceRange { get; set; }

        public override string ToString()
        {
            return Expression;
        }

        public string GetId()
        {
            return SourceRange?.Source + ":" + SourceRange?.Range + ":" + Expression;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is ExpressionRef expRef)
            {
                return this.GetId() == expRef.GetId();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return GetId().GetHashCode();
        }
    }
}
