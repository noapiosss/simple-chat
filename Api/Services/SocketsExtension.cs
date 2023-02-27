using System.Reflection;

namespace Api.Services
{
    public static class SocketExtension
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            _ = services.AddTransient<ConnectionManager>();
            foreach (Type type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandler))
                {
                    _ = services.AddSingleton(type);
                }
            }

            return services;
        }

        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path, SocketHandler socketHandler)
        {
            return app.Map(path, (x) => x.UseMiddleware<SocketMiddleware>(socketHandler));
        }
    }
}