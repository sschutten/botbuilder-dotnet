// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs.Debugging;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;
using Microsoft.Bot.Builder.LanguageGeneration;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Generators
{
    /// <summary>
    /// ILanguageGenerator implementation which uses LGFile. 
    /// </summary>
    public class TemplateEngineLanguageGenerator : LanguageGenerator
    {
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.TemplateEngineLanguageGenerator";

        private static readonly TaskFactory TaskFactory = new TaskFactory(
            CancellationToken.None,
            TaskCreationOptions.None,
            TaskContinuationOptions.None,
            TaskScheduler.Default);

        private readonly LanguageGeneration.Templates lg;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineLanguageGenerator"/> class.
        /// </summary>
        /// <param name="engine">template engine.</param>
        public TemplateEngineLanguageGenerator(LanguageGeneration.Templates engine = null)
        {
            this.lg = engine ?? new LanguageGeneration.Templates();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateEngineLanguageGenerator"/> class.
        /// </summary>
        /// <param name="resource">File resource.</param>
        /// <param name="resourceMapping">template resource loader delegate (locale) -> <see cref="ImportResolverDelegate"/>.</param>
        public TemplateEngineLanguageGenerator(FileResource resource, Dictionary<string, IList<Resource>> resourceMapping)
        {
            this.Id = resource.FullName;

            var (_, locale) = LGResourceLoader.ParseLGFileName(resource.Id);
            var importResolver = LanguageGeneratorManager.ResourceExplorerResolver(locale, resourceMapping);
            this.lg = LanguageGeneration.Templates.ParseFile(resource.FullName, importResolver);
            RegisterSourcemap(lg);
        }

        /// <summary>
        /// Gets or sets id of the source of this template (used for labeling errors).
        /// </summary>
        /// <value>
        /// Id of the source of this template (used for labeling errors).
        /// </value>
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Method to generate text from given template and data.
        /// </summary>
        /// <param name="dialogContext">Context for the current turn of conversation.</param>
        /// <param name="template">template to evaluate.</param>
        /// <param name="data">data to bind to.</param>
        /// <param name="cancellationToken">the <see cref="CancellationToken"/> for the task.</param>
        /// <returns>generated text.</returns>
        public override Task<object> GenerateAsync(DialogContext dialogContext, string template, object data, CancellationToken cancellationToken = default)
        {
            EventHandler onEvent = (s, e) => RunSync(() => HandlerLGEventAsync(dialogContext, s, e, cancellationToken));

            try
            {
                return Task.FromResult(lg.EvaluateText(template, data, new EvaluationOptions { OnEvent = onEvent }));
            }
            catch (Exception err)
            {
                if (!string.IsNullOrEmpty(this.Id))
                {
                    throw new Exception($"{Id}:{err.Message}");
                }

                throw;
            }
        }

        private static void RunSync(Func<Task> func)
        {
            TaskFactory.StartNew(() =>
            {
                return func();
            }).Unwrap().GetAwaiter().GetResult();
        }

        private static void RegisterSourcemap(LanguageGeneration.Templates templates)
        {
            foreach (var template in templates.AllTemplates)
            {
                RegisterSourcemap(template, template.SourceRange);
                foreach (var expressionRef in template.Expressions)
                {
                    RegisterSourcemap(expressionRef, expressionRef.SourceRange);
                }
            }
        }

        private static void RegisterSourcemap(object item, LanguageGeneration.SourceRange sr)
        {
            if (Path.IsPathRooted(sr.Source))
            {
                var debugSM = new Debugging.SourceRange(
                    sr.Source,
                    sr.Range.Start.Line,
                    sr.Range.Start.Character + 1,
                    sr.Range.End.Line,
                    sr.Range.End.Character + 1);

                if (!DebugSupport.SourceMap.TryGetValue(item, out var _))
                {
                    DebugSupport.SourceMap.Add(item, debugSM);
                }
            }
        }

        private async Task HandlerLGEventAsync(DialogContext dialogContext, object sender, EventArgs eventArgs, CancellationToken cancellationToken = default)
        {
            // skip the events that is not LG event or the event path is invalid.
            if (!(eventArgs is LGEventArgs lgEventArgs) || !Path.IsPathRooted(lgEventArgs.Source))
            {
                await Task.CompletedTask.ConfigureAwait(false);
            }

            if (eventArgs is BeginTemplateEvaluationArgs || eventArgs is BeginExpressionEvaluationArgs)
            {
                // Send debugger event
                await dialogContext.GetDebugger().StepAsync(dialogContext, sender, DialogEvents.LGEvents, cancellationToken).ConfigureAwait(false);
            }
            else if (eventArgs is MessageArgs message && dialogContext.GetDebugger() is IDebugger dda)
            {
                // send debugger message
                await dda.OutputAsync(message.Text, sender, message.Text, cancellationToken).ConfigureAwait(false);
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
}
