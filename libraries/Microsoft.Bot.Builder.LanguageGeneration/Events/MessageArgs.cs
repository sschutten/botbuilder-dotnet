using System;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    public class MessageArgs : LGEventArgs
    {
        /// <summary>
        /// Gets or sets message content.
        /// </summary>
        /// <value>
        /// Message content.
        /// </value>
        public string Text { get; set; }
    }
}
