﻿namespace Timesheet.Domain
{
    public static class TimesheetConstants
    {
        public const string AuthorizeEndpoint = "https://timesheetsts.azurewebsites.net/connect/authorize/";
        public const string TokenEndpoint = "https://timesheetsts.azurewebsites.net/connect/token/";
        public const string UserInfoEndpoint = "https://timesheetsts.azurewebsites.net/connect/userinfo/";

        public const string ClientId = "timesheet-mobile-app";

        public const string ApiScope = "timesheet-api";

        public const string VaultUserName = "token";
    }
}