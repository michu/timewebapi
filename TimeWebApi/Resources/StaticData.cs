namespace TimeWebApi.Resources;

public static class StaticData
{
    public static class ConnectionStrings
    {
        public const string DefaultConnectionString = "TimeWebApi";
    }

    public static class ConfigurationOptions
    {
        public const string JwtIssuer = "Jwt:Issuer";
        public const string JwtKey = "Jwt:Key";
    }

    public static class DateTimeFormats
    {
        public const string UtcIso = "yyyy-MM-dd HH:mm:ss.fffZ";
    }

    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Employee = "Employee";
    }

    public static class RouteValueKeys
    {
        public const string EmployeeId = "id";
        public const string TimeEntryId = "entryId";
    }
}
