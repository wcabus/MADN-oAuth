namespace Timesheet.Domain
{
    /// <summary>
    /// Constants used throughout the solution
    /// </summary>
    public static class TimesheetConstants
    {
        /// <summary>
        /// The STS endpoint needed to get the identity token (id_token) and authorization code (code)
        /// </summary>
        public const string AuthorizeEndpoint = "https://timesheetsts.azurewebsites.net/connect/authorize/";
        
        /// <summary>
        /// The STS endpoint used to retrieve the access token (token) and refresh token (offline_access)
        /// </summary>
        public const string TokenEndpoint = "https://timesheetsts.azurewebsites.net/connect/token/";
        
        /// <summary>
        /// The STS endpoint used to retrieve claims about a user
        /// </summary>
        public const string UserInfoEndpoint = "https://timesheetsts.azurewebsites.net/connect/userinfo/";

        /// <summary>
        /// The client ID for our mobile application
        /// </summary>
        public const string ClientId = "timesheet-mobile-app";

        /// <summary>
        /// The name of the resource scope for our API
        /// </summary>
        public const string ApiScope = "timesheet-api";

        /// <summary>
        /// A dummy username used to store the tokens in the PasswordVault.
        /// </summary>
        public const string VaultUserName = "token";
    }
}