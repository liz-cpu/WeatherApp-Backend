Imports System.Web.Http
Imports System.Web.Http.Controllers
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.DependencyInjection.Extensions

Partial Public Class Startup

    Public Shared Sub ConfigureServices(Services As IServiceCollection)
        Services.AddSingleton(Of ITokenManager, TokenManager)
        Services.AddControllersAsServices(GetType(Startup).Assembly.GetExportedTypes().
                                          Where(Function(T) Not T.IsAbstract And Not T.IsGenericTypeDefinition).
                                          Where(Function(T) GetType(IController).IsAssignableFrom(T) Or T.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                                          )
    End Sub
    Private Shared Function Configuration() As IServiceProvider
        Dim services As New ServiceCollection
        ConfigureServices(services)
        Return services.BuildServiceProvider()
    End Function

    Public Shared Sub Bootstrapper(Config As HttpConfiguration)
        Dim provider As ServiceProvider = Configuration()
        Dim resolver = New DefaultDependencyResolver(provider)
        'Config.DependencyResolver = resolver
        GlobalConfiguration.Configuration.DependencyResolver = resolver
        'DependencyResolver.SetResolver(resolver)
    End Sub
End Class
