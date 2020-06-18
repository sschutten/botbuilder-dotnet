// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    /// <summary>
    /// Provide basic event data of LG.
    /// </summary>
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
        /// Gets or sets parse tree context, may include evaluation stack.
        /// </summary>
        /// <value>
        /// LGContext.
        /// </value>
        public object Context { get; set; }
    }
}
