// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;

namespace Microsoft.Bot.Connector.Authentication
{
    /// <summary>
    /// Represents an Azure Cloud Environment
    /// Used to help determine which set of Bot and Channel service to service authentication constants to use.
    /// </summary>
    public class CloudEnvironment
    {
        /// <summary>
        /// Public Azure Cloud.
        /// </summary>
        public static CloudEnvironment PublicCloud = new CloudEnvironment(
            string.Empty,
            false,
            AuthenticationConstants.ToChannelFromBotLoginUrl,
            AuthenticationConstants.ToChannelFromBotOAuthScope,
            AuthenticationConstants.ToBotFromChannelTokenIssuer,
            AuthenticationConstants.OAuthUrl,
            AuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl,
            AuthenticationConstants.ToBotFromEmulatorOpenIdMetadataUrl,
            CallerIdConstants.PublicAzureChannel);

        /// <summary>
        /// US Governement Cloud.
        /// </summary>
        public static CloudEnvironment UsGovernment = new CloudEnvironment(
            GovernmentAuthenticationConstants.ChannelService,
            true,
            GovernmentAuthenticationConstants.ToChannelFromBotLoginUrl,
            GovernmentAuthenticationConstants.ToChannelFromBotOAuthScope,
            GovernmentAuthenticationConstants.ToBotFromChannelTokenIssuer,
            GovernmentAuthenticationConstants.OAuthUrlGov,
            GovernmentAuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl,
            GovernmentAuthenticationConstants.ToBotFromEmulatorOpenIdMetadataUrl,
            CallerIdConstants.USGovChannel);

        /// <summary>
        /// US National Governement Cloud.
        /// </summary>
        public static CloudEnvironment UsNatGovernment = new CloudEnvironment(
            UsNatGovernmentAuthenticationConstants.ChannelService,
            true,
            UsNatGovernmentAuthenticationConstants.ToChannelFromBotLoginUrl,
            UsNatGovernmentAuthenticationConstants.ToChannelFromBotOAuthScope,
            UsNatGovernmentAuthenticationConstants.ToBotFromChannelTokenIssuer,
            UsNatGovernmentAuthenticationConstants.OAuthUrlGov,
            UsNatGovernmentAuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl,
            UsNatGovernmentAuthenticationConstants.ToBotFromEmulatorOpenIdMetadataUrl,
            CallerIdConstants.USNatChannel);

        /// <summary>
        /// US Secure Governement Cloud.
        /// </summary>
        public static CloudEnvironment UsSecGovernment = new CloudEnvironment(
            UsSecGovernmentAuthenticationConstants.ChannelService,
            true,
            UsSecGovernmentAuthenticationConstants.ToChannelFromBotLoginUrl,
            UsSecGovernmentAuthenticationConstants.ToChannelFromBotOAuthScope,
            UsSecGovernmentAuthenticationConstants.ToBotFromChannelTokenIssuer,
            UsSecGovernmentAuthenticationConstants.OAuthUrlGov,
            UsSecGovernmentAuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl,
            UsSecGovernmentAuthenticationConstants.ToBotFromEmulatorOpenIdMetadataUrl,
            CallerIdConstants.USSecChannel);

        private CloudEnvironment(
            string channelService,
            bool isGovernment,
            string toChannelFromBotLoginUrl,
            string toChannelFromBotOAuthScope,
            string toBotFromChannelTokenIssuer,
            string oAuthUrl,
            string toBotFromChannelOpenIdMetadataUrl,
            string toBotFromEmulatorOpenIdMetadataUrl,
            string callerId)
        {
            ChannelService = channelService;
            IsGovernment = isGovernment;
            ToChannelFromBotLoginUrl = toChannelFromBotLoginUrl;
            ToChannelFromBotOAuthScope = toChannelFromBotOAuthScope;
            ToBotFromChannelTokenIssuer = toBotFromChannelTokenIssuer;
            OAuthUrl = oAuthUrl;
            ToBotFromChannelOpenIdMetadataUrl = toBotFromChannelOpenIdMetadataUrl;
            ToBotFromEmulatorOpenIdMetadataUrl = toBotFromEmulatorOpenIdMetadataUrl;
            CallerId = callerId;
        }

        /// <summary>
        /// The channel service for the cloud.
        /// </summary>
        public string ChannelService { get; private set; }

        /// <summary>
        /// The where this cloud environment is a government cloud.
        /// </summary>
        public bool IsGovernment { get; private set; }

        /// <summary>
        /// The channel service for the cloud.
        /// </summary>
        public string ToChannelFromBotLoginUrl { get; private set; }

        /// <summary>
        /// OAuth scope to request.
        /// </summary>
        public string ToChannelFromBotOAuthScope { get; private set; }

        /// <summary>
        /// Token issuer.
        /// </summary>
        public string ToBotFromChannelTokenIssuer { get; private set; }

        /// <summary>
        /// OAuth Url used to get a token from OAuthApiClient.
        /// </summary>
        public string OAuthUrl { get; private set; }

        /// <summary>
        /// OpenID metadata document for tokens coming from MSA.
        /// </summary>
        public string ToBotFromChannelOpenIdMetadataUrl { get; private set; }

        /// <summary>
        ///  OpenID metadata document for tokens coming from MSA.
        /// </summary>
        public string ToBotFromEmulatorOpenIdMetadataUrl{ get; private set; }

        /// <summary>
        /// A caller ID constant for this cloud environment
        /// </summary>
        public string CallerId { get; private set; }

        /// <summary>
        /// Gets a CloudEnvironment for a particular channel provider.
        /// </summary>
        /// <param name="channelProvider">The channel provider.</param>
        /// <returns>The CloudEnvironment.</returns>
        public static async Task<CloudEnvironment> GetCloudEnvironment(IChannelProvider channelProvider)
        {
            if (channelProvider == null)
            {
                return CloudEnvironment.PublicCloud;
            }

            var channelService = await channelProvider.GetChannelServiceAsync().ConfigureAwait(false);

            return GetCloudEnvironment(channelService);
        }

        /// <summary>
        /// Gets a CloudEnvironment for a particular channel service constant.
        /// </summary>
        /// <param name="channelService">The channel service.</param>
        /// <returns>The CloudEnvironment.</returns>
        public static CloudEnvironment GetCloudEnvironment(string channelService)
        {
            if (string.IsNullOrEmpty(channelService))
            {
                return CloudEnvironment.PublicCloud;
            }
            else if (channelService == UsGovernment.ChannelService)
            {
                return UsGovernment;
            }
            else if (channelService == UsNatGovernment.ChannelService)
            {
                return UsNatGovernment;
            }
            else if (channelService == UsSecGovernment.ChannelService)
            {
                return UsSecGovernment;
            }

            // Default is to use PublicCloud
            return CloudEnvironment.PublicCloud;
        }

        /// <summary>
        /// Gets an empty MicrosoftAppCredential for a particular channel service constant.
        /// </summary>
        /// <param name="channelProvider">The channel provider.</param>
        /// <returns>The empty credential.</returns>
        public static async Task<MicrosoftAppCredentials> GetEmptyCredential(IChannelProvider channelProvider)
        {
            var cloudEnvironment = await GetCloudEnvironment(channelProvider).ConfigureAwait(false);
            return GetEmptyCredential(cloudEnvironment.ChannelService);
        }

        /// <summary>
        /// Gets an empty MicrosoftAppCredential for a particular channel service constant.
        /// </summary>
        /// <param name="channelService">The channel service.</param>
        /// <returns>The empty credential.</returns>
        public static MicrosoftAppCredentials GetEmptyCredential(string channelService)
        {
            var cloudEnvironment = GetCloudEnvironment(channelService);
            if (cloudEnvironment == UsGovernment)
            {
                return MicrosoftGovernmentAppCredentials.Empty;
            }
            else if (cloudEnvironment == UsNatGovernment)
            {
                return MicrosoftGovernmentAppCredentials.UsNatEmpty;
            }
            else if (cloudEnvironment == UsSecGovernment)
            {
                return MicrosoftGovernmentAppCredentials.UsSecEmpty;
            }
            else
            {
                return MicrosoftAppCredentials.Empty;
            }
        }
    }
}
