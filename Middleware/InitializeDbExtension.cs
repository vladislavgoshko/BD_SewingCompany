namespace SewingCompany.Middleware
{

    public static class InitializeDbExtension
    {
        public static IApplicationBuilder UseInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbUserInitMiddleware>();
        }
    }
}
