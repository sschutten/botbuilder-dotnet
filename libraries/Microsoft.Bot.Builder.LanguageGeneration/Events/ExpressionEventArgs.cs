using System;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class ExpressionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets source id of the lg file.
        /// </summary>
        /// <value>
        /// source id of the lg file.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets Expression Context, may include evaluation stack.
        /// </summary>
        /// <value>
        /// LGContext.
        /// </value>
        public object Context { get; set; }
    }
}
