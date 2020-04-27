using System;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class LGEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets source id of the lg file.
        /// </summary>
        /// <value>
        /// source id of the lg file.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets LGContext, may include evaluation stack.
        /// </summary>
        /// <value>
        /// LGContext.
        /// </value>
        public object Context { get; set; }
    }
}
