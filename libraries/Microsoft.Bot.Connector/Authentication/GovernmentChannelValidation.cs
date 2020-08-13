// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Bot.Connector.Authentication
{
    /// <summary>
    /// Valies JWT tokens from a Government channel.
    /// </summary>
    public sealed class GovernmentChannelValidation
    {
        /// <summary>
        /// TO BOT FROM GOVERNMENT CHANNEL: Token validation parameters when connecting to a bot.
        /// </summary>
        public static readonly TokenValidationParameters ToBotFromGovernmentChannelTokenValidationParameters =
            new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuers = new[] { GovernmentAuthenticationConstants.ToBotFromChannelTokenIssuer },

                // Audience validation takes place in JwtTokenExtractor
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
            };

        /// <summary>
        /// TO BOT FROM USNAT GOVERNMENT CHANNEL: Token validation parameters when connecting to a bot.
        /// </summary>
        public static readonly TokenValidationParameters ToBotFromUsNatGovernmentChannelTokenValidationParameters =
            new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuers = new[] { UsNatGovernmentAuthenticationConstants.ToBotFromChannelTokenIssuer },

                // Audience validation takes place in JwtTokenExtractor
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
            };

        /// <summary>
        /// TO BOT FROM USSEC GOVERNMENT CHANNEL: Token validation parameters when connecting to a bot.
        /// </summary>
        public static readonly TokenValidationParameters ToBotFromUsSecGovernmentChannelTokenValidationParameters =
            new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuers = new[] { UsSecGovernmentAuthenticationConstants.ToBotFromChannelTokenIssuer },

                // Audience validation takes place in JwtTokenExtractor
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5),
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
            };

        /// <summary>
        /// Gets or sets the metadata address.
        /// </summary>
        /// <value>
        /// The metadata address.
        /// </value>
#pragma warning disable CA1056 // Uri properties should not be strings (we can't change this without breaking binary compat)
        public static string OpenIdMetadataUrl { get; set; } = GovernmentAuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl;

        /// <summary>
        /// Gets or sets UsNatOpenIdMetadataUrl.
        /// </summary>
        /// <value>
        /// UsNatOpenIdMetadataUrl.
        /// </value>
        public static Uri UsNatOpenIdMetadataUrl { get; set; } = new Uri(UsNatGovernmentAuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl);

        /// <summary>
        /// Gets or sets UsSecOpenIdMetadataUrl.
        /// </summary>
        /// <value>
        /// UsSecOpenIdMetadataUrl.
        /// </value>
        public static Uri UsSecOpenIdMetadataUrl { get; set; } = new Uri(UsSecGovernmentAuthenticationConstants.ToBotFromChannelOpenIdMetadataUrl);
#pragma warning restore CA1056 // Uri properties should not be strings

        /// <summary>
        /// Validate the incoming Auth Header as a token sent from a Bot Framework Government Channel Service.
        /// </summary>
        /// <param name="authHeader">The raw HTTP header in the format: "Bearer [longString]".</param>
        /// <param name="credentials">The user defined set of valid credentials, such as the AppId.</param>
        /// <param name="serviceUrl">The service url from the request.</param>
        /// <param name="httpClient">Authentication of tokens requires calling out to validate Endorsements and related documents. The
        /// HttpClient is used for making those calls. Those calls generally require TLS connections, which are expensive to
        /// setup and teardown, so a shared HttpClient is recommended.</param>
        /// <param name="channelId">The ID of the channel to validate.</param>
        /// <returns>ClaimsIdentity.</returns>
#pragma warning disable UseAsyncSuffix // Use Async suffix (can't change this without breaking binary compat)
        public static async Task<ClaimsIdentity> AuthenticateChannelToken(string authHeader, ICredentialProvider credentials, string serviceUrl, HttpClient httpClient, string channelId)
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            return await AuthenticateChannelToken(authHeader, credentials, serviceUrl, httpClient, channelId, new AuthenticationConfiguration()).ConfigureAwait(false);
        }

        /// <summary>
        /// Validate the incoming Auth Header as a token sent from a Bot Framework Government Channel Service.
        /// </summary>
        /// <param name="authHeader">The raw HTTP header in the format: "Bearer [longString]".</param>
        /// <param name="credentials">The user defined set of valid credentials, such as the AppId.</param>
        /// <param name="serviceUrl">The service url from the request.</param>
        /// <param name="httpClient">Authentication of tokens requires calling out to validate Endorsements and related documents. The
        /// HttpClient is used for making those calls. Those calls generally require TLS connections, which are expensive to
        /// setup and teardown, so a shared HttpClient is recommended.</param>
        /// <param name="channelId">The ID of the channel to validate.</param>
        /// <param name="authConfig">The authentication configuration.</param>
        /// <returns>ClaimsIdentity.</returns>
#pragma warning disable UseAsyncSuffix // Use Async suffix (can't change this without breaking binary compat)
        public static Task<ClaimsIdentity> AuthenticateChannelToken(string authHeader, ICredentialProvider credentials, string serviceUrl, HttpClient httpClient, string channelId, AuthenticationConfiguration authConfig)
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            return AuthenticateChannelToken(authHeader, credentials, serviceUrl, httpClient, channelId, authConfig, CloudEnvironment.UsGovernment);
        }

        /// <summary>
        /// Validate the incoming Auth Header as a token sent from a Bot Framework Government Channel Service.
        /// </summary>
        /// <param name="authHeader">The raw HTTP header in the format: "Bearer [longString]".</param>
        /// <param name="credentials">The user defined set of valid credentials, such as the AppId.</param>
        /// <param name="serviceUrl">The service url from the request.</param>
        /// <param name="httpClient">Authentication of tokens requires calling out to validate Endorsements and related documents. The
        /// HttpClient is used for making those calls. Those calls generally require TLS connections, which are expensive to
        /// setup and teardown, so a shared HttpClient is recommended.</param>
        /// <param name="channelId">The ID of the channel to validate.</param>
        /// <param name="authConfig">The authentication configuration.</param>
        /// <param name="cloudEnvironment">The cloud environment.</param>
        /// <returns>ClaimsIdentity.</returns>
#pragma warning disable UseAsyncSuffix // Use Async suffix (can't change this without breaking binary compat)
        public static async Task<ClaimsIdentity> AuthenticateChannelToken(string authHeader, ICredentialProvider credentials, string serviceUrl, HttpClient httpClient, string channelId, AuthenticationConfiguration authConfig, CloudEnvironment cloudEnvironment)
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            if (authConfig == null)
            {
                throw new ArgumentNullException(nameof(authConfig));
            }

            TokenValidationParameters tokenValidationParameters = null;
            string openIdMetadataUrl = null;

            if (cloudEnvironment == CloudEnvironment.UsGovernment)
            {
                tokenValidationParameters = ToBotFromGovernmentChannelTokenValidationParameters;
                openIdMetadataUrl = OpenIdMetadataUrl;
            }
            else if (cloudEnvironment == CloudEnvironment.UsNatGovernment)
            {
                tokenValidationParameters = ToBotFromUsNatGovernmentChannelTokenValidationParameters;
                openIdMetadataUrl = UsNatOpenIdMetadataUrl.OriginalString;
            }
            else if (cloudEnvironment == CloudEnvironment.UsSecGovernment)
            {
                tokenValidationParameters = ToBotFromUsSecGovernmentChannelTokenValidationParameters;
                openIdMetadataUrl = UsSecOpenIdMetadataUrl.OriginalString;
            }
            else
            {
                throw new ArgumentException(nameof(CloudEnvironment));
            }

            var tokenExtractor = new JwtTokenExtractor(
                httpClient,
                tokenValidationParameters,
                openIdMetadataUrl,
                AuthenticationConstants.AllowedSigningAlgorithms);

            var identity = await tokenExtractor.GetIdentityAsync(authHeader, channelId, authConfig.RequiredEndorsements).ConfigureAwait(false);

            await ValidateIdentity(identity, credentials, serviceUrl, cloudEnvironment).ConfigureAwait(false);

            return identity;
        }

        /// <summary>
        /// Validate the ClaimsIdentity as sent from a Bot Framework Government Channel Service.
        /// </summary>
        /// <param name="identity">The claims identity to validate.</param>
        /// <param name="credentials">The user defined set of valid credentials, such as the AppId.</param>
        /// <param name="serviceUrl">The service url from the request.</param>
        /// <param name="cloudEnvironment">The cloud environment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
#pragma warning disable UseAsyncSuffix // Use Async suffix (can't change this without breaking binary compat)
        public static async Task ValidateIdentity(ClaimsIdentity identity, ICredentialProvider credentials, string serviceUrl, CloudEnvironment cloudEnvironment)
#pragma warning restore UseAsyncSuffix // Use Async suffix
        {
            if (identity == null)
            {
                // No valid identity. Not Authorized.
                throw new UnauthorizedAccessException();
            }

            if (!identity.IsAuthenticated)
            {
                // The token is in some way invalid. Not Authorized.
                throw new UnauthorizedAccessException();
            }

            // Now check that the AppID in the claimset matches
            // what we're looking for. Note that in a multi-tenant bot, this value
            // comes from developer code that may be reaching out to a service, hence the
            // Async validation.

            // Look for the "aud" claim, but only if issued from the Bot Framework
            Claim audienceClaim = identity.Claims.FirstOrDefault(
                c => c.Issuer == cloudEnvironment.ToBotFromChannelTokenIssuer && c.Type == AuthenticationConstants.AudienceClaim);

            if (audienceClaim == null)
            {
                // The relevant audience Claim MUST be present. Not Authorized.
                throw new UnauthorizedAccessException();
            }

            // The AppId from the claim in the token must match the AppId specified by the developer.
            // In this case, the token is destined for the app, so we find the app ID in the audience claim.
            string appIdFromClaim = audienceClaim.Value;
            if (string.IsNullOrWhiteSpace(appIdFromClaim))
            {
                // Claim is present, but doesn't have a value. Not Authorized.
                throw new UnauthorizedAccessException();
            }

            if (!await credentials.IsValidAppIdAsync(appIdFromClaim).ConfigureAwait(false))
            {
                // The AppId is not valid. Not Authorized.
                throw new UnauthorizedAccessException($"Invalid AppId passed on token: {appIdFromClaim}");
            }

            if (serviceUrl != null)
            {
                var serviceUrlClaim = identity.Claims.FirstOrDefault(claim => claim.Type == AuthenticationConstants.ServiceUrlClaim)?.Value;
                if (string.IsNullOrWhiteSpace(serviceUrlClaim))
                {
                    // Claim must be present. Not Authorized.
                    throw new UnauthorizedAccessException();
                }

                if (!string.Equals(serviceUrlClaim, serviceUrl, StringComparison.OrdinalIgnoreCase))
                {
                    // Claim must match. Not Authorized.
                    throw new UnauthorizedAccessException();
                }
            }
        }

        /// <summary>
        /// Sets the Open Id Metadata URL for all gov clouds.
        /// </summary>
        /// <param name="url">The open id metadata url.</param>
        public static void SetOpenIdMetadataUrl(string url)
        {
            OpenIdMetadataUrl = url;
            UsNatOpenIdMetadataUrl = new Uri(url);
            UsSecOpenIdMetadataUrl = new Uri(url);
        }
    }
}
