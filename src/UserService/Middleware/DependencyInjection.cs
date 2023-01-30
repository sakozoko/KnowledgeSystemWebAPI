using MediatR;

namespace UserService.Middleware;

public static class DependencyInjection
{
    public static IServiceCollection AddMiddleware(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient<ExceptionHandlingMiddleware>();
        return services;
    }

    public static IApplicationBuilder UseMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        return app;
    }
}