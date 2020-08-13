// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Connector.Authentication
{
    /// <summary>
    /// Values and Constants used for Authentication and Authorization by the Bot Framework Protocol to US Government DataCenters.
    /// </summary>
    public static class UsSecGovernmentAuthenticationConstants
    {
        /// <summary>
        /// Government Channel Service property value.
        /// </summary>
        public const string ChannelService = "https://botframework.microsoft.scloud";

        /// <summary>
        /// TO GOVERNMENT CHANNEL FROM BOT: Login URL.
        /// </summary>
        public const string ToChannelFromBotLoginUrl = "https://login.microsoftonline.microsoft.scloud/MicrosoftServices.onms.microsoft.scloud";

        /// <summary>
        /// TO GOVERNMENT CHANNEL FROM BOT: OAuth scope to request.
        /// </summary>
        public const string ToChannelFromBotOAuthScope = "https://api.botframework.microsoft.scloud";

        /// <summary>
        /// TO BOT FROM GOVERNMENT CHANNEL: Token issuer.
        /// </summary>
        public const string ToBotFromChannelTokenIssuer = "https://api.botframework.microsoft.scloud";

        /// <summary>
        /// OAuth Url used to get a token from OAuthApiClient.
        /// </summary>
        public const string OAuthUrlGov = "https://api.botframework.microsoft.scloud";

        /// <summary>
        /// TO BOT FROM GOVERNMANT CHANNEL: OpenID metadata document for tokens coming from MSA.
        /// </summary>
        public const string ToBotFromChannelOpenIdMetadataUrl = "https://login.botframework.microsoft.scloud/v1/.well-known/openidconfiguration";

        /// <summary>
        /// TO BOT FROM GOVERNMENT EMULATOR: OpenID metadata document for tokens coming from MSA.
        /// </summary>
        public const string ToBotFromEmulatorOpenIdMetadataUrl = "https://login.microsoftonline.microsoft.scloud/MicrosoftServices.onms.microsoft.scloud/v2.0/.well-known/openid-configuration";
    }
}
