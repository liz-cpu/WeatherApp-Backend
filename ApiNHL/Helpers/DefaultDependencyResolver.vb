Imports Microsoft.Extensions.DependencyInjection
Imports System.Web.Http.Dependencies

Public Class DefaultDependencyResolver
    Implements IDependencyResolver

    Private serviceScope As IServiceScope
    Protected Property ServiceProvider As IServiceProvider

    Public Sub New(ByVal serviceProvider As IServiceProvider)
        Me.ServiceProvider = serviceProvider
    End Sub

    Public Function GetService(ByVal serviceType As Type) As Object Implements IDependencyResolver.GetService
        Return Me.ServiceProvider.GetService(serviceType)
    End Function

    Public Function GetServices(ByVal serviceType As Type) As IEnumerable(Of Object) Implements IDependencyResolver.GetServices
        Return Me.ServiceProvider.GetServices(serviceType)
    End Function

    Public Function BeginScope() As IDependencyScope Implements IDependencyResolver.BeginScope
        serviceScope = Me.ServiceProvider.CreateScope()
        Return New DefaultDependencyResolver(serviceScope.ServiceProvider)
    End Function

    Public Sub Dispose() Implements IDisposable.Dispose
        serviceScope?.Dispose()
    End Sub
End Class
