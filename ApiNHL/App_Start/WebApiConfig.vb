Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net.Http
Imports System.Web.Http
Imports System.Web.Http.Dispatcher
Imports ApiNHL.ApiNHL.Filters
Imports Microsoft.Extensions.Hosting

Public Module WebApiConfig
    Public Sub Register(ByVal config As HttpConfiguration)

        config.MapHttpAttributeRoutes()

        config.Filters.Add(New TokenAuthenticationFilter)

        Startup.Bootstrapper(config)
        config.MessageHandlers.Add(New CustomDelegatingMessageHandler())
        config.Routes.MapHttpRoute(
            name:="DefaultApi",
            routeTemplate:="api/{controller}/{action}",
            defaults:=New With {.id = RouteParameter.Optional}
        )
    End Sub
End Module
