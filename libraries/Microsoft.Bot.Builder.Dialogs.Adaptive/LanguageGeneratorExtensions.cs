// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Generators;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive
{
    public static class LanguageGeneratorExtensions
    {
        private static Dictionary<ResourceExplorer, LanguageGeneratorManager> languageGeneratorManagers = new Dictionary<ResourceExplorer, LanguageGeneratorManager>();

        /// <summary>
        /// Register default LG file as language generation.
        /// </summary>
        /// <param name="dialogManager">The <see cref="BotAdapter"/> to add services to.</param>
        /// <param name="defaultLg">Default LG Resource Id (default: main.lg).</param>
        /// <param name="defaultFallback">Default Language policy fallback. (default: emptystring).</param>
        /// <returns>The BotAdapter.</returns>
        public static DialogManager UseLanguageGeneration(
            this DialogManager dialogManager,
            string defaultLg = null,
            string defaultFallback = "")
        {
            if (defaultLg == null)
            {
                defaultLg = "main.lg";
            }

            var resourceExplorer = dialogManager.InitialTurnState.Get<ResourceExplorer>();

            if (resourceExplorer.TryGetResource(defaultLg, out var resource))
            {
                dialogManager.UseLanguageGeneration(new ResourceMultiLanguageGenerator(defaultLg), new LanguagePolicy(defaultFallback));
            }
            else
            {
                dialogManager.UseLanguageGeneration(new TemplateEngineLanguageGenerator(), new LanguagePolicy(defaultFallback));
            }

            return dialogManager;
        }

        /// <summary>
        /// Register ILanguageGenerator as default langugage generator.
        /// </summary>
        /// <param name="dialogManager">botAdapter to add services to.</param>
        /// <param name="languageGenerator">LanguageGenerator to use.</param>
        /// <param name="policy"> Register language policy as default policy.</param>
        /// <returns>botAdapter.</returns>
        public static DialogManager UseLanguageGeneration(this DialogManager dialogManager, LanguageGenerator languageGenerator, LanguagePolicy policy = null)
        {
            var resourceExplorer = dialogManager.InitialTurnState.Get<ResourceExplorer>();

            lock (languageGeneratorManagers)
            {
                if (!languageGeneratorManagers.TryGetValue(resourceExplorer ?? throw new ArgumentNullException(nameof(resourceExplorer)), out var lgm))
                {
                    lgm = new LanguageGeneratorManager(resourceExplorer, policy);
                    languageGeneratorManagers[resourceExplorer] = lgm;
                }

                dialogManager.InitialTurnState.Add<LanguagePolicy>(policy);
                dialogManager.InitialTurnState.Add<LanguageGeneratorManager>(lgm);
                dialogManager.InitialTurnState.Add<LanguageGenerator>(languageGenerator ?? throw new ArgumentNullException(nameof(languageGenerator)));

                return dialogManager;
            }
        }
    }
}
