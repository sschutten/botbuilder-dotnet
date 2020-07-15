// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Microsoft.Bot.Builder.Integration.AspNet.Core
{
    /// <summary>
    /// Interface to express the relationship between an mvc api Controller and a Bot Builder Adapter.
    /// This interface can be used for Dependency Injection.
    /// </summary>
    public interface IDeclarativeAdapter : IBotFrameworkHttpAdapter
    {
        /// <summary>
        /// Gets or sets the adapter name.
        /// </summary>
        /// <value>
        /// The adapter name.
        /// </value>
        string Name { get; set; }
    }
}
