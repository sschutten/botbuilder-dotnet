// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Connector.Authentication
{
    /// <summary>
    /// Values and Constants used for Authentication and Authorization by the Bot Framework Protocol to US Government DataCenters.
    /// </summary>
    public static class UsNatGovernmentAuthenticationConstants
    {
        /// <summary>
        /// Government Channel Service property value.
        /// </summary>
        public const string ChannelService = "https://botframework.azure.gov";

        /// <summary>
        /// TO GOVERNMENT CHANNEL FROM BOT: Login URL.
        /// </summary>
        public const string ToChannelFromBotLoginUrl = "https://login.microsoftonline.us/MicrosoftServices.onmicrosoft.gov";

        /// <summary>
        /// TO GOVERNMENT CHANNEL FROM BOT: OAuth scope to request.
        /// </summary>
        public const string ToChannelFromBotOAuthScope = "https://api.botframework.gov";

        /// <summary>
        /// TO BOT FROM GOVERNMENT CHANNEL: Token issuer.
        /// </summary>
        public const string ToBotFromChannelTokenIssuer = "https://api.botframework.gov";

        /// <summary>
        /// OAuth Url used to get a token from OAuthApiClient.
        /// </summary>
        public const string OAuthUrlGov = "https://api.botframework.azure.gov";

        /// <summary>
        /// TO BOT FROM GOVERNMANT CHANNEL: OpenID metadata document for tokens coming from MSA.
        /// </summary>
        public const string ToBotFromChannelOpenIdMetadataUrl = "https://login.botframework.azure.gov/v1/.well-known/openidconfiguration";

        /// <summary>
        /// TO BOT FROM GOVERNMENT EMULATOR: OpenID metadata document for tokens coming from MSA.
        /// </summary>
        public const string ToBotFromEmulatorOpenIdMetadataUrl = "https://login.microsoftonline.gov/cab8a31a-1906-4287-a0d8-4eef66b95f6e/v2.0/.well-known/openid-configuration";
    }
}
